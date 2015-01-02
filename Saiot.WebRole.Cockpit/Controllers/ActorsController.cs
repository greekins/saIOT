using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Saiot.Core.Messaging;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using Saiot.WebRole.Common.Controllers;
using System.Configuration;
using Saiot.Bll.Services;
using Newtonsoft.Json;
using System.Text;
using Saiot.Bll.Dto;

namespace Saiot.WebRole.Cockpit.Controllers
{

    [RoutePrefix("api/actors")]
    public class ActorsController : ProtectedServiceController
    {
        private MessagingContext context;
        private QueueHelper queueHelper;
        private Object thisLock = new Object();
        private ActorService actorService;
        private CommandService commandService;

        public ActorsController() : base(ConfigurationManager.AppSettings["StorageAccount.ConnectionString"]) 
        {
            actorService = Assembler.GetActorService();
            commandService = Assembler.GetCommandService();
        }

        [Route("{tenantId}/list")]
        public IHttpActionResult GetActors(string tenantId)
        {
            return PerformClaimBasedOperation(() => 
            {
                return Ok(actorService.GetAllActors(tenantId));
            });
        }


        [Route("{tenantId}/details/{actorId}")]
        public IHttpActionResult GetActorDetail(string tenantId, string actorId)
        {
            return PerformClaimBasedOperation(() => 
            {
                var dto = actorService.GetActor(tenantId, actorId);
                var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(dto.Config);
                var result = new
                {
                    ActorId = dto.ActorId,
                    Location = dto.Location,
                    Type = dto.Type,
                    Name = dto.Name,
                    Config = config
                };
                return Ok(result);
            });
        }

        [Route("{tenantId}/config/{actorId}")]
        public IHttpActionResult PutConfig(string tenantId, string actorId, [FromBody] AbstractActorDto dto)
        {
            return PerformClaimBasedOperation(() => 
            {
                var actor = actorService.GetActor(tenantId, actorId);
                var command = string.Format("set-config -name \"{0}\" -Location \"{1}\" -config \"{2}\"", actor.Name, actor.Location, Convert.ToBase64String(Encoding.UTF8.GetBytes(dto.Config)));
                commandService.SendCommand(tenantId, command);
                return Ok();
            });
            
        }

    }
}
