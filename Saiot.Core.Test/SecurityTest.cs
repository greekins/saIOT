using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saiot.Core.Security;
using Saiot.Core.Test.Fakes;

namespace Saiot.Core.Test
{
    [TestClass]
    public class SecurityTest
    {

        private IClaimAuthorizer authorizer;
        private StorageSasProducer sasProducer;

        [TestInitialize]
        public void Initialize()
        {
            authorizer = new ClaimAuthorizerFake(true, true, true);
            sasProducer = new StorageSasProducerFake();
        }

        [TestMethod]
        [ExpectedException(typeof(ClaimAuthorizationException))]
        public void TestClaimAuthorization_FailTable()
        {
            var authorizer = new ClaimAuthorizerFake(true, false, true);
            var claim = new TableClaim
            {
                TableName = "tableName",
                Permissions = Microsoft.WindowsAzure.Storage.Table.SharedAccessTablePermissions.None,
                PartitionKey = "partitionKey"
            };
            claim.AuthorizeWith(authorizer);
        }


        [TestMethod]
        [ExpectedException(typeof(ClaimAuthorizationException))]
        public void TestClaimAuthorization_FailPartition()
        {
            var authorizer = new ClaimAuthorizerFake(false, true, true);
            var claim = new TableClaim
            {
                TableName = "tableName",
                Permissions = Microsoft.WindowsAzure.Storage.Table.SharedAccessTablePermissions.None,
                PartitionKey = "partitionKey"
            };
            claim.AuthorizeWith(authorizer);
        }

        [TestMethod]
        [ExpectedException(typeof(ClaimAuthorizationException))]
        public void TestClaimAuthorization_FailPermissions()
        {
            var authorizer = new ClaimAuthorizerFake(true, true, false);
            var claim = new TableClaim
            {
                TableName = "tableName",
                Permissions = Microsoft.WindowsAzure.Storage.Table.SharedAccessTablePermissions.None,
                PartitionKey = "partitionKey"
            };
            claim.AuthorizeWith(authorizer);
        }

        [TestMethod]
        public void TestClaimAuthorization_Success()
        {
            var authorizer = new ClaimAuthorizerFake(true, true, true);
            var claim = new TableClaim
            {
                TableName = "tableName",
                Permissions = Microsoft.WindowsAzure.Storage.Table.SharedAccessTablePermissions.None,
                PartitionKey = "partitionKey"
            };
            claim.AuthorizeWith(authorizer);
        }

        [TestMethod]
        [ExpectedException(typeof(ClaimAuthorizationException))]
        public void TestSasProducer_Fail()
        {
            var claim = new TableClaim
            {
                TableName = "tableName",
                Permissions = Microsoft.WindowsAzure.Storage.Table.SharedAccessTablePermissions.None,
                PartitionKey = "partitionKey"
            };
            sasProducer.RequestSasToken(claim);
        }

        [TestMethod]
        public void TestSasProducer_Success()
        {
            var authorizer = new ClaimAuthorizerFake(true, true, true);
            var claim = new TableClaim
            {
                TableName = "tableName",
                Permissions = Microsoft.WindowsAzure.Storage.Table.SharedAccessTablePermissions.None,
                PartitionKey = "partitionKey"
            };
            claim.AuthorizeWith(authorizer);
            Assert.AreEqual("tokenstringfake", sasProducer.RequestSasToken(claim));
        }
    }
}
