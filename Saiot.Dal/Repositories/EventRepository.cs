using Saiot.Dal.TableEntities;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Saiot.Core.Security;
using Saiot.Core.Storage;

namespace Saiot.Dal.Repositories
{
    public class EventRepository : IProtectedRepository
    {

        public IStorageProvider StorageProvider { get; set; }
        public string EventTableName = TableName.Event.ToString();
        private TableClaim eventTableClaim;

        public EventRepository()
        {
            eventTableClaim = new TableClaim
            {
                TableName = EventTableName,
                Permissions = SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Update | SharedAccessTablePermissions.Query
            };
        }

        public IEnumerable<EventEntity> FindEvents(string tenantId, int limit = -1)
        {
            TableQuery<EventEntity> query = new TableQuery<EventEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, tenantId));
            eventTableClaim.PartitionKey = tenantId;
            var table = StorageProvider.AcquireReference(eventTableClaim);
            return table.ExecuteQuery(query).OrderByDescending(c => c.Timestamp).Take(limit);
        }
        public void Insert(EventEntity eventEntity)
        {
            eventTableClaim.PartitionKey = eventEntity.PartitionKey;
            var table = StorageProvider.AcquireReference(eventTableClaim);
            table.Execute(TableOperation.Insert(eventEntity));
        }



    }
}