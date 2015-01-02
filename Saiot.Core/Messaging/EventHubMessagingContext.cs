using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Messaging
{
    public class EventHubMessagingContext : MessagingContext
    {

        public EventHubMessagingContext() { }
        public EventHubMessagingContext(string json) : base(json){}

        private int partitionCount;
        public int PartitionCount
        {
            get { return partitionCount == 0 ? 8 : partitionCount; }
            set 
            {
                int count = value;
                if (count < 8) count = 8;
                if (count > 32) count = 32;
                partitionCount = count; 
            }
        }

        public override string ServiceBusPath
        {
            get { return serviceBusPath ?? string.Format("{0}/publishers/{1}", EntityName, ClientIdentifier);}
            set { serviceBusPath = string.Format("{0}{1}{2}", value, value == string.Empty ? "" : "/", EntityName);}
        }

        public override string FullEntityPath
        {
            get
            {
                return string.Format("{0}{1}", ServiceUri.ToString(), ServiceBusPath);
            }
        }

        protected override MessagingFactory NewFactory()
        {
            var factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ServiceBusNamespace, string.Empty), new MessagingFactorySettings()
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(SharedAccessSignature),
                TransportType = TransportType.Amqp
            });
            return factory;
        }

        public override Uri ServiceUri
        {
            get { return ServiceBusEnvironment.CreateServiceUri("sb", ServiceBusNamespace, string.Empty);}
        }   

    }
}
