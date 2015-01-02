using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saiot.Dal.Repositories;

namespace Saiot.Dal.Test
{
    [TestClass]
    public class CredentialRepositoryTest : ProtectedRepositoryTest
    {
        private CredentialRepository repo;
        private const string tenantId = "abc";

        [TestInitialize]
        public void Initialize()
        {
            base.InitializeStorageProvider();
            repo = new CredentialRepository();
            repo.StorageProvider = StorageProvier;
            StorageProvier.TableNameShould = TableName.Credential.ToString();
            StorageProvier.TenantIdShould = tenantId;

        }

        [TestMethod]
        public void testFindCredentials()
        {
            try { repo.FindCredentials(tenantId, null, null); }
            catch (TableNullException) { }
            StorageProvier.AssertMock().Reset();
        }
    }
}
