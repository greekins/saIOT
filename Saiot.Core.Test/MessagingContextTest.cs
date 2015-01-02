using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saiot.Core.Messaging;
using System.Diagnostics;

namespace Saiot.Core.Test
{
    [TestClass]
    public class MessagingContextTest
    {

        private MessagingContext context;
        private EventHubMessagingContext ehContext;

        [TestInitialize]
        public void Inititalize()
        {
            context = new MessagingContext()
            {
                ServiceBusNamespace = "namespace",
                ClientIdentifier = "xy",
                EntityName = "cmd"
            };
            ehContext = new EventHubMessagingContext()
            {
                ServiceBusNamespace = "namespace",
                EntityName = "hub",
                ClientIdentifier = "xy"
            };
        }

        [TestMethod]
        public void TestFactory()
        {
            Assert.IsNotNull(context.Factory);
        }

        [TestMethod]
        public void TestSasConnectionString()
        {
            Assert.AreEqual("Endpoint=sb://namespace.servicebus.windows.net/xy/;SharedAccessKeyName=[SharedAccessKeyName];SharedAccessKey=[SharedAccessKey]",
                context.SasConnectionString);
        }

        [TestMethod]
        public void TestSasConnectionString_NoClient()
        {
            MessagingContext context = new MessagingContext()
            {
                ServiceBusNamespace = "namespace"
            };
            Assert.AreEqual("Endpoint=sb://namespace.servicebus.windows.net/;SharedAccessKeyName=[SharedAccessKeyName];SharedAccessKey=[SharedAccessKey]",
                context.SasConnectionString);
        }

        [TestMethod]
        public void TestServiceUri()
        {
            Assert.AreEqual("sb://namespace.servicebus.windows.net/xy/", context.ServiceUri.ToString());
        }

        [TestMethod]
        public void TestServiceUri_NoClient()
        {
            MessagingContext context = new MessagingContext()
            {
                ServiceBusNamespace = "namespace"
            };
            Assert.AreEqual("sb://namespace.servicebus.windows.net/", context.ServiceUri.ToString());
        }

        [TestMethod]
        public void TestServiceUri_EventHub()
        {
            Assert.AreEqual("sb://namespace.servicebus.windows.net/", ehContext.ServiceUri.ToString());
        }

        [TestMethod]
        public void TestSharedAccessKeyName()
        {
            Assert.IsNotNull(context.SharedAccessKeyName);
            Assert.AreNotEqual(string.Empty, context.SharedAccessKeyName);
        }

        [TestMethod]
        public void TestSharedAccessKey()
        {
            Assert.IsNotNull(context.SharedAccessKey);
            Assert.AreNotEqual(string.Empty, context.SharedAccessKey);
        }

        [TestMethod]
        public void TestSharedAccessSignature()
        {
            Assert.IsNotNull(context.SharedAccessSignature);
        }

        [TestMethod]
        public void TestSharedAccessSignature_EventHub()
        {
            Assert.IsNotNull(ehContext.SharedAccessSignature);
        }

        [TestMethod]
        public void TestSasTokenProvider()
        {
            Assert.IsNotNull(context.SasTokenProvider);
        }

        [TestMethod]
        public void TestServiceBusPath()
        {
            Assert.AreEqual("xy/", context.ServiceBusPath);
        }

        [TestMethod]
        public void TestServiceBusPath_NoClient()
        {
            MessagingContext context = new MessagingContext();
            Assert.AreEqual(string.Empty, context.ServiceBusPath);
        }

        [TestMethod]
        public void TestServiceBusPath_EventHub()
        {
            Assert.AreEqual("hub/publishers/xy", ehContext.ServiceBusPath);
        }

        [TestMethod]
        public void TestFullEntityPath_EventHub()
        {
            Assert.AreEqual("sb://namespace.servicebus.windows.net/hub/publishers/xy", ehContext.FullEntityPath);
        }

        [TestMethod]
        public void TestFullEntityPath()
        {
            Assert.AreEqual("sb://namespace.servicebus.windows.net/xy/cmd", context.FullEntityPath);
        }

        [TestMethod]
        public void TestPartitionCount_EventHub()
        {
            ehContext.PartitionCount = 1;
            Assert.IsTrue(ehContext.PartitionCount >= 8);
            Assert.IsTrue(ehContext.PartitionCount <= 32);
        }

        [TestMethod]
        public void TestEntityName()
        {
            Assert.IsNotNull(context.EntityName);
        }

        [TestMethod]
        public void TestClientIdentifier()
        {
            Assert.IsNotNull(context.ClientIdentifier);
        }
    }
}
