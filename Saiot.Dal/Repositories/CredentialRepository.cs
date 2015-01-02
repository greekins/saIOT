using Microsoft.WindowsAzure.Storage.Table;
using Saiot.Core.Security;
using Saiot.Core.Storage;
using Saiot.Dal.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Dal.Repositories
{
    public class CredentialRepository : IProtectedRepository
    {
        public IStorageProvider StorageProvider { get; set; }

        private TableClaim credentialTableClaim;

        public CredentialRepository()
        {
            credentialTableClaim = new TableClaim
            {
                TableName = TableName.Credential.ToString(),
                Permissions = SharedAccessTablePermissions.Query
            };
        }

        public CredentialEntity FindCredentials(string tenantId,  string sbEntityName, string keyName)
        {
            credentialTableClaim.PartitionKey = tenantId;

            string byPartition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, tenantId);
            string byKeyName = TableQuery.GenerateFilterCondition("SharedAccessKeyName", QueryComparisons.Equal, keyName);
            string byEntity = TableQuery.GenerateFilterCondition("Entity", QueryComparisons.Equal, sbEntityName);
            string filter = TableQuery.CombineFilters(TableQuery.CombineFilters(byPartition, TableOperators.And, byKeyName), TableOperators.And, byEntity);
            TableQuery<CredentialEntity> query = new TableQuery<CredentialEntity>().Where(filter);

            var table = StorageProvider.AcquireReference(credentialTableClaim);
            var results = table.ExecuteQuery(query);
            return results.First();
        }
    }
}
