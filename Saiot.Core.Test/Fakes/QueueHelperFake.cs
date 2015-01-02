using Saiot.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Test.Fakes
{
    public class QueueHelperFake : QueueHelper
    {
        public ConnectionState ExpectedState { get; set; }

        public QueueHelperFake() : base(new MessagingContext()) { }

        protected override Microsoft.ServiceBus.Messaging.BrokeredMessage InternalReceive()
        {
            if (ExpectedState == ConnectionState.Failed)
            {
                throw new Exception("Simulate Fail");
            }
            return null;
        }

        protected override void InternalSend(Microsoft.ServiceBus.Messaging.BrokeredMessage command)
        {
            if (ExpectedState == ConnectionState.Failed)
            {
                throw new Exception("Simulate Fail");
            }
        }

    }
}
