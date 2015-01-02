using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging.Adapters
{
    public class NullObjectMessageReceiver : IMessageReceiver
    {

        public Microsoft.ServiceBus.Messaging.BrokeredMessage Receive()
        {
            return null;
        }
    }
}
