using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Security
{
    public class StorageSasProducer
    {
        public const double AccessPolicyDurationInMinutes = 1;
        private readonly string connectionString;


        public StorageSasProducer(string connectionString) 
        {
            this.connectionString = connectionString;
        }

        public StorageSasProducer(CloudStorageAccount storageAccount) 
        {
            this.storageAccount = storageAccount;
        }

        public string RequestSasToken(TableClaim claim)
        {
            if (claim.IsAuthorized)
            {
                return requestSasTokenImpl(claim.PartitionKey, claim.TableName, claim.Permissions);
            }
            throw new ClaimAuthorizationException(string.Format("Unauthorized Claim was used to request Sas Token"));
        }

        private CloudStorageAccount StorageAccount 
        {
            get {return storageAccount ?? (storageAccount = CloudStorageAccount.Parse(connectionString));}
        }
        private CloudStorageAccount storageAccount;

        private CloudTableClient TableClient
        {
            get {return tableClient ?? (tableClient = StorageAccount.CreateCloudTableClient());}
        }
        private CloudTableClient tableClient;


        protected virtual string requestSasTokenImpl(string partitionKey, string tableName, SharedAccessTablePermissions permissions)
        {
            var policy = getSharedAccessTablePolicy(permissions);
            var table = getCloudTableReference(tableName);
            table.CreateIfNotExists();
            return table.GetSharedAccessSignature(policy, null, partitionKey, null, partitionKey, null);
        }


        private SharedAccessTablePolicy getSharedAccessTablePolicy(SharedAccessTablePermissions permissions) 
        {
            return new SharedAccessTablePolicy
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(AccessPolicyDurationInMinutes),
                Permissions = permissions
            }; 
        }

        private CloudTable getCloudTableReference(string tableName) 
        {
            return TableClient.GetTableReference(tableName);
        }


    }
}
