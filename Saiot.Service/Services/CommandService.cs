using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Saiot.Bll.Dto;
using Saiot.Bll.Exceptions;
using Saiot.Core.Messaging;
using Saiot.Dal.Repositories;
using Saiot.Dal.TableEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Bll.Services
{
    public class CommandService
    {
        internal CommandRepository CommandRepository { get; set; }
        internal CredentialRepository CredentialRepository { get; set; }
        public CommandCorrelationDto SendCommand(string tenantId, string command)
        {
            var guid = Guid.NewGuid().ToString();
            var helper = getHelper(tenantId, "cmd");
            helper.SendMessage(new BrokeredMessage(command) {Label = guid});
            if (helper.SenderState != ConnectionState.Success) throw new Exception("failed sending command");
            
            var correlation = new CommandCorrelationEntity 
            {
                PartitionKey = tenantId,
                RowKey = guid,
                Cmd = command
            };
            CommandRepository.InsertOrUpdate(correlation);
            return new CommandCorrelationDto(correlation);           
        }

        private QueueHelper getHelper(string tenantId, string entityName)
        {
            var credentials = CredentialRepository.FindCredentials(tenantId, entityName, "WebRoles");
            var context = entityName == "hub" ? new EventHubMessagingContext() : new MessagingContext();
            context.ServiceBusNamespace = ConfigurationManager.AppSettings["Microsoft.ServiceBus.Namespace"];
            context.SharedAccessKey = credentials.SharedAccessKey;
            context.SharedAccessKeyName = credentials.SharedAccessKeyName;
            context.EntityName = credentials.Entity;
            context.ClientIdentifier = credentials.PartitionKey;
            return new QueueHelper(context);
        }

        public void RegisterReply(string tenantId, string correlationId, string reply)
        {
            CommandCorrelationEntity correlation = null;
            try
            {
                correlation = CommandRepository.FindByCorrelationId(tenantId, correlationId);
            }
            catch
            {
                throw new CorrelationNotFoundException(new CommandCorrelationDto { CorrelationId = correlationId });
            }
            correlation.Rpl = reply;
            CommandRepository.InsertOrUpdate(correlation);
        }

        public IEnumerable<CommandCorrelationDto> GetUnrepliedCommands(string tenantId)
        {
            return (from row in CommandRepository.FindAllCorrelations(tenantId).OrderByDescending(r => r.Timestamp)
                    select new CommandCorrelationDto(row)).Where(d => d.Rpl == null).ToList();
        }

        public IEnumerable<CommandCorrelationDto> GetRepliedCommands(string tenantId)
        {
            return (from row in CommandRepository.FindAllCorrelations(tenantId).OrderByDescending(r => r.Timestamp)
                    select new CommandCorrelationDto(row)).Where(d => d.Rpl != null).ToList();
        }

        public IEnumerable<CommandCorrelationDto> GetAllCommands(string tenantId)
        {
            return (from row in CommandRepository.FindAllCorrelations(tenantId).OrderByDescending(r => r.Timestamp)
                    select new CommandCorrelationDto(row)).ToList();
        }
    }
}
