using Saiot.Core.Storage;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Dal.Test.Mock
{
    public class StorageProviderMock : IStorageProvider
    {
        public bool MethodCalled { get; private set; }
        public string TenantIdWas { get; set; }
        public string TenantIdShould { get; set; }
        public string TableNameWas { get; set; }
        public string TableNameShould { get; set; }

        public Microsoft.WindowsAzure.Storage.Table.CloudTable AcquireReference(Core.Security.TableClaim claim)
        {
            MethodCalled = true;
            TenantIdWas = claim.PartitionKey;
            TableNameWas = claim.TableName;
            throw new TableNullException();
        }

        public StorageProviderMock AssertMock()
        {
            Assert.IsTrue(MethodCalled);
            Assert.AreEqual(TenantIdShould, TenantIdWas);
            Assert.AreEqual(TableNameShould, TableNameWas);
            return this;
        }

        public StorageProviderMock Reset()
        {
            MethodCalled = false;
            return this;
        }
    }
}
