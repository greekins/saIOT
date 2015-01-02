using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saiot.Core.Storage;
using Saiot.Dal.Test.Mock;

namespace Saiot.Dal.Test
{
    [TestClass]
    public class ProtectedRepositoryTest
    {

        protected StorageProviderMock StorageProvier;

        public void InitializeStorageProvider()
        {
            StorageProvier = new StorageProviderMock();
        }

    }
}
