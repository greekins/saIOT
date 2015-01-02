using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Messaging
{
    public class EventHubHelper
    {

        public ConnectionState SenderState { get; protected set; }

        public EventHubMessagingContext MessagingContext { get; private set; }
        public EventHubHelper(EventHubMessagingContext messagingContext)
        {
            MessagingContext = messagingContext;
            SenderState = ConnectionState.Unstarted;
        }

        private EventHubClient client;
        public EventHubClient Client
        {
            get { return client ?? NewEventHubClient(); }
            set { client = value; }
        }


        protected EventHubClient NewEventHubClient()
        {
            return MessagingContext.Factory.CreateEventHubClient(MessagingContext.ServiceBusPath);
        }

        public void SendEvent(object data)
        {
            var messageData = JsonConvert.SerializeObject(data);
            var eventData = new EventData(Encoding.UTF8.GetBytes(messageData));
            eventData.PartitionKey = MessagingContext.ClientIdentifier;
            try
            {
                SenderState = ConnectionState.Sending;
                InternalSendEvent(eventData);
                SenderState = ConnectionState.Success;
            }
            catch
            {
                SenderState = ConnectionState.Failed;
            }
        }

        protected virtual void InternalSendEvent(EventData data)
        {
            Client.Send(data);
        }


    }
}
