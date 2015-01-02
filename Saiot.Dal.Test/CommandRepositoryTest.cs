using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saiot.Dal.Repositories;
using Saiot.Dal.TableEntities;

namespace Saiot.Dal.Test
{
    [TestClass]
    public class CommandRepositoryTest : ProtectedRepositoryTest
    {

        private CommandRepository repo;
        private const string tenantId = "abc";

        [TestInitialize]
        public void Initialize() 
        {
            base.InitializeStorageProvider();
            repo = new CommandRepository();
            repo.StorageProvider = StorageProvier;
            StorageProvier.TableNameShould = TableName.CommandCorrelation.ToString();
            StorageProvier.TenantIdShould = tenantId;
        }

        [TestMethod]
        public void testFindByCorrelationId()
        {
            try { repo.FindByCorrelationId(tenantId, "correlationId"); }
            catch (TableNullException) { }
            StorageProvier.AssertMock().Reset();
        }


        [TestMethod]
        public void testInsertOrUpdate()
        {
            CommandCorrelationEntity cmd = new CommandCorrelationEntity
            {
                PartitionKey = tenantId
            };
            try { repo.InsertOrUpdate(cmd); }
            catch (TableNullException) { }
            StorageProvier.AssertMock().Reset();
        }
    }
}
