using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ServiceBus.Messaging;
using Saiot.Core.Messaging;
using System.Configuration;
using Saiot.Core.Test.Fakes;
using Saiot.Core.Test.Poco;

namespace Saiot.Core.Test
{
    [TestClass]
    public class MessagingHelperTest
    {

        private MessagingContext context;
        private EventHubMessagingContext ehContext;

        private EventHubHelper ehHelper;
        private EventHubHelperFake ehHelperFake;

        private QueueHelper qHelper;
        private QueueHelperFake qHelperFake;
        

        [TestInitialize]
        public void TestInitialize()
        {
            context = new MessagingContext()
            {
                ServiceBusNamespace = "saiot-sbns",
                EntityName = "cmd",
                ClientIdentifier = "xy",
                SharedAccessKeyName = "Sender",
            };
            ehContext = new EventHubMessagingContext()
            {
                ServiceBusNamespace = "namespace",
                EntityName = "hub",
                ClientIdentifier = "xy"
            };
            ehHelper = new EventHubHelper(ehContext);
            ehHelperFake = new EventHubHelperFake();
            qHelper = new QueueHelper(context);
            qHelperFake = new QueueHelperFake();
        }

        [TestMethod]
        public void TestEventHubHelper_Client()
        {
            Assert.IsNotNull(ehHelper.Client);
        }

        [TestMethod]
        public void TestEventHubHelper_Send_Success()
        {
            ehHelperFake.ExpectedState = ConnectionState.Success;
            Assert.AreEqual(ConnectionState.Unstarted, ehHelperFake.SenderState);
            ehHelperFake.SendEvent(new { Info = "Info" });
            Assert.AreEqual(ConnectionState.Success, ehHelperFake.SenderState);
        }

        [TestMethod]
        public void TestEventHubHelper_Send_Failed()
        {
            ehHelperFake.ExpectedState = ConnectionState.Failed;
            Assert.AreEqual(ConnectionState.Unstarted, ehHelperFake.SenderState);
            ehHelperFake.SendEvent(new { Info = "Info" });
            Assert.AreEqual(ConnectionState.Failed, ehHelperFake.SenderState);
        }

        [TestMethod]
        public void TestQueueHelper_Receive_Success()
        {
            qHelperFake.ExpectedState = ConnectionState.Success;
            queueReceiveAndExpect(qHelperFake, ConnectionState.Success);
        }

        [TestMethod]
        public void TestQueueHelper_Receive_Failed()
        {
            qHelperFake.ExpectedState = ConnectionState.Failed;
            queueReceiveAndExpect(qHelperFake, ConnectionState.Failed);
        }

        [TestMethod]
        public void TestQueueHelper_Send_Success()
        {
            qHelperFake.ExpectedState = ConnectionState.Success;
            queueSendAndExpect(qHelperFake, ConnectionState.Success);
        }

        [TestMethod]
        public void TestQueueHelper_Send_Failed()
        {
            qHelperFake.ExpectedState = ConnectionState.Failed;
            queueSendAndExpect(qHelperFake, ConnectionState.Failed);
        }

        private void queueSendAndExpect(QueueHelper helper, ConnectionState expectedState)
        {
            Assert.AreEqual(ConnectionState.Unstarted, helper.SenderState);
            helper.SendMessage(new BrokeredMessage());
            Assert.AreEqual(expectedState, helper.SenderState);
        }

        private void queueReceiveAndExpect(QueueHelper helper, ConnectionState expectedState)
        {
            Assert.AreEqual(ConnectionState.Unstarted, helper.ReceiverState);
            helper.ReceiveMessage();
            Assert.AreEqual(expectedState, helper.ReceiverState);
        }


      
    }
}
