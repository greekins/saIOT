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
    public class ActorRepository : IProtectedRepository
    {
        public IStorageProvider StorageProvider { get; set; }
        private TableClaim claim;

        public ActorRepository()
        {
            claim = new TableClaim
            {
                TableName = TableName.Actor.ToString(),
                Permissions = SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Update | SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Delete
            };
        }

        public IEnumerable<AbstractActorEntity> FindAbstractActorByName(string tenantId, string actorName)
        {
            claim.PartitionKey = tenantId;
            var table = StorageProvider.AcquireReference(claim);
            TableQuery<AbstractActorEntity> query = new TableQuery<AbstractActorEntity>().Where(TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, actorName));
            return table.ExecuteQuery(query);
        }

        public AbstractActorEntity FindAbstractActorById(string tenantId, string actorId)
        {
            claim.PartitionKey = tenantId;
            var table = StorageProvider.AcquireReference(claim);
            TableOperation operation = TableOperation.Retrieve<AbstractActorEntity>(tenantId, actorId);
            return (AbstractActorEntity)table.Execute(operation).Result;
        }

        public IEnumerable<AbstractActorEntity> FindAllActors(string tenantId)
        {
            TableQuery<AbstractActorEntity> query = new TableQuery<AbstractActorEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, tenantId));
            claim.PartitionKey = tenantId;
            var table = StorageProvider.AcquireReference(claim);
            return table.ExecuteQuery(query).OrderBy(c => c.RowKey);
        }

        public void InsertOrUpdateAbstractActor(AbstractActorEntity actor)
        {
            claim.PartitionKey = actor.PartitionKey;
            var table = StorageProvider.AcquireReference(claim);
            TableOperation operation = TableOperation.InsertOrMerge(actor);
            table.Execute(operation);
        }

        public void DeleteAbstractActor(AbstractActorEntity actor)
        {
            claim.PartitionKey = actor.PartitionKey;
            var table = StorageProvider.AcquireReference(claim);
            TableOperation operation = TableOperation.Delete(actor);
            table.Execute(operation);
        }

    }
}
