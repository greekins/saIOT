using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saiot.Dal.Repositories;
using Saiot.Dal.TableEntities;

namespace Saiot.Dal.Test
{
    [TestClass]
    public class ActorRepositoryTest : ProtectedRepositoryTest
    {

        private ActorRepository repo;
        private const string tenantId = "abc";
        private const string actorName = "testname";
        private const string actorId = "testid";
        private AbstractActorEntity actor;

        [TestInitialize]
        public void Initialize()
        {
            base.InitializeStorageProvider();
            repo = new ActorRepository();
            repo.StorageProvider = StorageProvier;
            actor = new AbstractActorEntity
            {
                PartitionKey = tenantId,
                RowKey = actorId,
                Name = actorName
            };
            StorageProvier.TableNameShould = TableName.Actor.ToString();
            StorageProvier.TenantIdShould = tenantId;
        }


        [TestMethod]
        public void testDeleteActor()
        {
            try { repo.DeleteAbstractActor(actor); }
            catch (TableNullException) { }
            StorageProvier.AssertMock().Reset();
        }

        [TestMethod]
        public void testFindActorById()
        {
            try { repo.FindAbstractActorById(tenantId, actorId); }
            catch (TableNullException) { }
            StorageProvier.AssertMock().Reset();
        }

        [TestMethod]
        public void testFindActorByName()
        {
            try { repo.FindAbstractActorByName(tenantId, "TestName"); }
            catch (TableNullException) { }
            StorageProvier.AssertMock().Reset();
        }


        [TestMethod]
        public void testFindAllActors()
        {
            try { repo.FindAllActors(tenantId); }
            catch (TableNullException) { }
            StorageProvier.AssertMock().Reset();
        }

        [TestMethod]
        public void testInsertOrUpdateActor()
        {
            try { repo.InsertOrUpdateAbstractActor(actor); }
            catch (TableNullException) { }
            StorageProvier.AssertMock().Reset();
        }



    }
}
