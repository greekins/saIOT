using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Shared.Messaging.Adapters
{
    public class QueueMessageSender : IMessageSender
    {

        private QueueClient client;

        public QueueMessageSender(QueueClient client)
        {
            this.client = client;
        }

        public void Send(BrokeredMessage message)
        {
            client.Send(message);
        }
    }
}
