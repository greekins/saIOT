using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging
{

    public enum ServiceBusEntity { queue_entity, topic_entity, subscription_entity, eventhub_entity, notificationhub_entity, any_entity};

    public interface IMessagingContext
    {
        ServiceBusEntity ServiceBusEntity { get; }

        string SharedSecretIssuer
        {
            get;
        }

        string SharedSecretKey
        {
            get;
        }

        string Username
        {
            get;
        }

        string Password
        {
            get;
        }

        string ServiceBusNamespace
        {
            get;
        }

        string ServiceBusPath
        {
            get;
        }


    }
}
