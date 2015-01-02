using Microsoft.WindowsAzure.Storage.Table;
using Saiot.Dal.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Dal.Repositories
{
    public class TenantRepository
    {

        public CloudTable Table { get; set; }

        public IEnumerable<TenantEntity> FindUsers(string tenantId)
        {
            var query = new TableQuery<TenantEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, tenantId));
            return Table.ExecuteQuery(query);
        }

        public IEnumerable<TenantEntity> FindTenants(string username)
        {
            var query = new TableQuery<TenantEntity>().Where(TableQuery.GenerateFilterCondition("Identifier", QueryComparisons.Equal, username));
            return Table.ExecuteQuery(query);
        }

        public void InsertOrUpdate(TenantEntity tenant)
        {
            var operation = TableOperation.InsertOrReplace(tenant);
            Table.Execute(operation);
        }

    }
}
