using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Shared.Messaging.Adapters
{
    public class GeneralMessageSender : IMessageSender
    {

        private MessageSender sender;

        public GeneralMessageSender(MessageSender sender)
        {
            this.sender = sender;
        }

        public void Send(BrokeredMessage message)
        {
            sender.Send(message);
        }
    }
}
