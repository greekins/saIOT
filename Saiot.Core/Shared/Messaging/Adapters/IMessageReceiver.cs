using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Shared.Messaging.Adapters
{
    public interface IMessageReceiver
    {
        BrokeredMessage Receive();
    }
}
