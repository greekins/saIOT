using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Messaging;
using Microsoft.ServiceBus.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Messaging.Adapters;

namespace SharedTest.Fakes
{
    class MessagingHelperMock : MessagingHelper
    {

        private const string CREATE_FACTORY_CALL = "createFactory";
        private const string CREATE_CONNECTION_STRING_CALL = "createConnectionString";
        private const int NUM_CREATE_MESSAGING_FACTORY_CALLS = 2;
        private string[] createMessagingFactoryCalls;
        private int createFactoryCallsIndex = 0;

        private const string CONVERT_TO_BROKERED_MESSAGE_CALL = "convertToBrokeredMessage";
        private const string CREATE_SENDER_FOR_RESSOURCE_CALL = "createSenderForRessource";
        private const int NUM_SEND_MESSAGE_CALLS = 2;
        private string[] sendMessageCalls;
        private int sendMessageCallsIndex = 0;

        public MessagingHelperMock(IMessagingContext context) : base(context)
        {
            createMessagingFactoryCalls = new string[NUM_CREATE_MESSAGING_FACTORY_CALLS];
            sendMessageCalls = new string[NUM_SEND_MESSAGE_CALLS];
        }

        protected override MessagingFactory CreateFactory(string connectionString)
        {
            createMessagingFactoryCalls[createFactoryCallsIndex++] = CREATE_FACTORY_CALL;
            return null;
        }

        public override string CreateSasConnectionString()
        {
            createMessagingFactoryCalls[createFactoryCallsIndex++] = CREATE_CONNECTION_STRING_CALL;
            return base.CreateSasConnectionString();
        }

        public void AssertCreateMessagingFactoryCalls()
        {
            Assert.AreEqual(createMessagingFactoryCalls[0], CREATE_CONNECTION_STRING_CALL);
            Assert.AreEqual(createMessagingFactoryCalls[1], CREATE_FACTORY_CALL);
        }



        public override BrokeredMessage ConvertToBrokeredMessage(Object original)
        {
            sendMessageCalls[sendMessageCallsIndex++] = CONVERT_TO_BROKERED_MESSAGE_CALL;
            return base.ConvertToBrokeredMessage(original);
        }

        public override IMessageSender CreateSenderForRessource(string ressourceName)
        {
            sendMessageCalls[sendMessageCallsIndex++] = CREATE_SENDER_FOR_RESSOURCE_CALL;
            return base.CreateSenderForRessource(ressourceName);
        }

        protected override bool IsQueueEntity()
        {
            return false;
        }

        public override QueueClient CreateQueueClient(string ressourceName)
        {
            return null;
        }

        public void AssertSendMessageCalls()
        {
            Assert.AreEqual(sendMessageCalls[0], CREATE_SENDER_FOR_RESSOURCE_CALL);
            Assert.AreEqual(sendMessageCalls[1], CONVERT_TO_BROKERED_MESSAGE_CALL);
        }


    }
}
