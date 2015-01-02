using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.Messaging;

namespace SharedTest.Fakes
{
    class MessagingContextFake : IMessagingContext
    {

        public ServiceBusEntity ServiceBusEntity { get { return ServiceBusEntity.queue_entity; } }
        public string SharedSecretIssuer
        {
            get { return string.Empty; }
        }

        public string SharedSecretKey
        {
            get { return string.Empty; }
        }

        public string Username
        {
            get { return string.Empty; }
        }

        public string Password
        {
            get { return string.Empty; }
        }

        public string ServiceBusNamespace
        {
            get { return string.Empty; }
        }

        public string ServiceBusPath
        {
            get { return string.Empty; }
        }
    }
}
