using Microsoft.ServiceBus.Messaging;
using Saiot.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Test.Fakes
{
    public class EventHubHelperFake : EventHubHelper
    {

        public EventHubHelperFake() : base(new EventHubMessagingContext()) { }

        public ConnectionState ExpectedState { get; set; }
        protected override void InternalSendEvent(EventData data)
        {
            if (ExpectedState == ConnectionState.Failed)
            {
                throw new Exception("Fail");
            }
        }
    }
}
