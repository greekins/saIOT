using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Shared.Messaging.Adapters
{
    public class QueueMessageReceiver : IMessageReceiver
    {

        private QueueClient client;

        public QueueMessageReceiver(QueueClient client)
        {
            this.client = client;
        }
        public BrokeredMessage Receive()
        {
            return client.Receive();
        }
    }
}
