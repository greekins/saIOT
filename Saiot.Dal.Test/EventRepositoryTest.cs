using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saiot.Dal.Repositories;
using Saiot.Dal.TableEntities;

namespace Saiot.Dal.Test
{
    [TestClass]
    public class EventRepositoryTest : ProtectedRepositoryTest
    {

        private EventRepository repo;
        private const string tenantId = "abc";

        [TestInitialize]
        public void Initialize()
        {
            InitializeStorageProvider();
            repo = new EventRepository();
            repo.StorageProvider = StorageProvier;
            StorageProvier.TableNameShould = TableName.Event.ToString();
            StorageProvier.TenantIdShould = tenantId;

        }

        [TestMethod]
        public void testFindEvents()
        {
            try { repo.FindEvents(tenantId); }
            catch (TableNullException) { }
            StorageProvier.AssertMock().Reset();
        }


        [TestMethod]
        public void testInsert()
        {
            var entity = new EventEntity
            {
                PartitionKey = tenantId
            };
            try { repo.Insert(entity); }
            catch (TableNullException) { }
            StorageProvier.AssertMock().Reset();
        }
    }
}
