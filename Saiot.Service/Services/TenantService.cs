using Microsoft.ServiceBus;
using Saiot.Core.Messaging;
using Saiot.Core.Security;
using Saiot.Dal.Repositories;
using System;
using System.Configuration;
using System.Linq;

namespace Saiot.Bll.Services
{
    public class TenantService
    {
        internal TenantRepository TenantRepository { get; set; }
        internal CredentialRepository CredentialRepository { get; set; }
        internal StorageSasProducer SasProducer { get; set; }

        public bool UserBelongsToTenant(string username, string tenantId)
        {
            var users = TenantRepository.FindUsers(tenantId);
            return users.Any((u) => u.Identifier == username);
        }

        public string GetTenantOf(string username) 
        {
            return TenantRepository.FindTenants(username).FirstOrDefault().PartitionKey;
        }

        public string GetTableStorageToken(TableClaim claim)
        {
            return SasProducer.RequestSasToken(claim);
        }

        public string GetServiceBusSignature(string tenantId, string sbEntityName, string keyName)
        {
            var credentials = CredentialRepository.FindCredentials(tenantId, sbEntityName, keyName);
            var context = sbEntityName == "hub" ? new EventHubMessagingContext() : new MessagingContext();
            context.ServiceBusNamespace = ConfigurationManager.AppSettings["Microsoft.ServiceBus.Namespace"];
            context.SharedAccessKey = credentials.SharedAccessKey;
            context.SharedAccessKeyName = credentials.SharedAccessKeyName;
            context.EntityName = credentials.Entity;
            context.ClientIdentifier = credentials.PartitionKey;
            return context.SharedAccessSignature;

        }

    }
}
