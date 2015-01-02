using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Shared.Messaging.Adapters
{
    public class GeneralMessageReceiver : IMessageReceiver
    {

        public GeneralMessageReceiver(MessageReceiver receiver)
        {
            this.receiver = receiver;
        }
        public BrokeredMessage Receive()
        {
            return receiver.Receive();
        }

        private MessageReceiver receiver;
    }
}
