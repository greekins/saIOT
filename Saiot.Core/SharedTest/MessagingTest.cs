using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ServiceBus.Messaging;
using Shared.Messaging;
using System.Configuration;
using SharedTest.Fakes;
using SharedTest.Poco;
using Shared.Messaging.Adapters;

namespace SharedTest
{
    [TestClass]
    public class MessagingTest
    {

        private MessagingHelperMock helperMock;

        [TestInitialize]
        public void TestInitialize()
        {
            helperMock = new MessagingHelperMock(new MessagingContextFake());
        }

        [TestMethod]
        public void Test_MessagingHelper_CreateMessagingFactory()
        {
            var factory = helperMock.CreateMessagingFactory();
            Assert.IsNull(factory);
            helperMock.AssertCreateMessagingFactoryCalls();
        }

        [TestMethod]
        public void Test_MessagingHelper_CreateSasConnectionString()
        {
            MessagingHelper helper = new MessagingHelper(MessagingContext.ContextFromSettings(ConfigurationManager.AppSettings));
            string connectionString = helper.CreateSasConnectionString();
            Assert.AreEqual("Endpoint=sb://testing-namespace.servicebus.windows.net/testing-path;SharedAccessKeyName=testing-issuer;SharedAccessKey=testing-key",
                connectionString);
        }

        [TestMethod]
        public void Test_MessagingHelper_ConvertToBrokeredMessage()
        {
            var original = new TestMessage();
            BrokeredMessage converted = helperMock.ConvertToBrokeredMessage(original);
            Assert.AreEqual("foo", converted.Properties["Foo"]);
            Assert.AreEqual("bar", converted.Properties["Bar"]);
        }

      
        [TestMethod]
        public void Test_MessagingHelper_SendMessage()
        {
            IMessagingContext context = MessagingContext.ContextFromSettings(ConfigurationManager.AppSettings);
            MessagingHelperMock mock = new MessagingHelperMock(context);
            mock.SendMessage(new TestMessage(), "");
            mock.AssertSendMessageCalls();
        }

        [TestMethod]
        public void Test_MessagingContext_ContextFromSettings()
        {
            IMessagingContext context = MessagingContext.ContextFromSettings(ConfigurationManager.AppSettings);
            Assert.AreEqual("testing-namespace", context.ServiceBusNamespace);
            Assert.AreEqual("testing-issuer", context.SharedSecretIssuer);
            Assert.AreEqual("testing-key", context.SharedSecretKey);
            Assert.AreEqual("testing-path", context.ServiceBusPath);
            Assert.AreEqual("testing-username", context.Username);
            Assert.AreEqual("testing-password", context.Password);
        }

    }
}
