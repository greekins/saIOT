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
    public class CommandRepository : IProtectedRepository
    {
        public IStorageProvider StorageProvider { get; set; }
        private TableClaim claim;

        public CommandRepository()
        {
            claim = new TableClaim
            {
                TableName = TableName.CommandCorrelation.ToString(),
                Permissions = SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Update | SharedAccessTablePermissions.Query
            };
        }

        public void InsertOrUpdate(CommandCorrelationEntity entity)
        {
            var operation = TableOperation.InsertOrMerge(entity);
            claim.PartitionKey = entity.PartitionKey;
            var table = StorageProvider.AcquireReference(claim);
            table.Execute(operation);
        }

        public CommandCorrelationEntity FindByCorrelationId(string tenantId, string correlationId)
        {
            var operation = TableOperation.Retrieve<CommandCorrelationEntity>(tenantId, correlationId);
            claim.PartitionKey = tenantId;
            var table = StorageProvider.AcquireReference(claim);
            return (CommandCorrelationEntity) table.Execute(operation).Result;
        }

        public IEnumerable<CommandCorrelationEntity> FindAllCorrelations(string tenantId)
        {
            TableQuery<CommandCorrelationEntity> query = new TableQuery<CommandCorrelationEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, tenantId));
            claim.PartitionKey = tenantId;
            var table = StorageProvider.AcquireReference(claim);
            return table.ExecuteQuery(query);
        }
    }
}
