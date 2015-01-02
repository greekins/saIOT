using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging
{

    public abstract class MessagingContext
    {
        public static KeyValueMessagingContext ContextFromSettings(NameValueCollection settings)
        {
            return new KeyValueMessagingContext(settings);
        }

        public static EmptyMessagingContext CreateEmptyContext()
        {
            return new EmptyMessagingContext();
        }

    }

    public class EmptyMessagingContext : IMessagingContext
    {
        public ServiceBusEntity ServiceBusEntity { get; set; }

        public string SharedSecretIssuer { get; set; }

        public string SharedSecretKey { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ServiceBusNamespace { get; set; }

        public string ServiceBusPath { get; set; }
    }

    public class KeyValueMessagingContext : IMessagingContext
    {

        private NameValueCollection dictionary;

        public KeyValueMessagingContext(NameValueCollection dictionary)
        {
            this.dictionary = dictionary;
        }

        public ServiceBusEntity ServiceBusEntity
        {
            get { return (ServiceBusEntity)Enum.Parse(typeof(ServiceBusEntity), dictionary["ServiceBusEntity"]); }
        }

        public string SharedSecretIssuer
        {
            get { return dictionary["SharedSecretIssuer"]; }
        }

        public string SharedSecretKey
        {
            get { return dictionary["SharedSecretKey"]; }
        }

        public string Username
        {
            get { return dictionary["Username"]; }
        }

        public string Password
        {
            get { return dictionary["Password"]; }
        }

        public string ServiceBusNamespace
        {
            get { return dictionary["ServiceBusNamespace"]; }
        }

        public string ServiceBusPath
        {
            get { return dictionary["ServiceBusPath"]; }
        }
    }

    
}
