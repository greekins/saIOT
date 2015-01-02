using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging.Adapters
{
    public class NullObjectMessageSender : IMessageSender
    {
        public void Send(Microsoft.ServiceBus.Messaging.BrokeredMessage message)
        {
            
        }
    }
}
