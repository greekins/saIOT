using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Messaging
{
    public class QueueHelper
    {

        public MessagingContext MessagingContext;
        public ConnectionState ReceiverState;
        public ConnectionState SenderState;

        public QueueHelper(MessagingContext messagingContext)
        {
            MessagingContext = messagingContext;
            ReceiverState = ConnectionState.Unstarted;
            SenderState = ConnectionState.Unstarted;
        }

        private QueueClient client;
        public QueueClient Client
        {
            get { return client ?? NewQueueClient(); }
            set { client = value; }
        }

        protected QueueClient NewQueueClient()
        {
            return MessagingContext.Factory.CreateQueueClient(MessagingContext.EntityName);
        }

        public BrokeredMessage ReceiveMessage()
        {
            ReceiverState = ConnectionState.Receiving;
            BrokeredMessage message = null;
            try
            {
                message = InternalReceive();
                ReceiverState = ConnectionState.Success;
            }
            catch
            {
                ReceiverState = ConnectionState.Failed;
            }
            return message;
        }

        public async Task<BrokeredMessage> ReceiveCommandAsync()
        {
            ReceiverState = ConnectionState.Receiving;
            BrokeredMessage message = null;
            try
            {
                message = await InternalReceiveAsync();
                ReceiverState = ConnectionState.Success;
            }
            catch
            {
                ReceiverState = ConnectionState.Failed;
            }
            return message;
        }

        public void SendMessage(BrokeredMessage message)
        {
            SenderState = ConnectionState.Sending;
            try
            {
                InternalSend(message);
                SenderState = ConnectionState.Success;
            }
            catch
            {
                SenderState = ConnectionState.Failed;
            }
        }

        protected virtual BrokeredMessage InternalReceive() 
        {
            return Client.Receive();
        }

        protected virtual async Task<BrokeredMessage> InternalReceiveAsync()
        {
            return await Client.ReceiveAsync();
        }

        protected virtual void InternalSend(BrokeredMessage message)
        {
            Client.Send(message);
        }
    }
}
