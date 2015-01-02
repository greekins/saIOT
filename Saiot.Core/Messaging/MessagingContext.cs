using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Saiot.Core.Messaging
{
    public class MessagingContext
    {

        public MessagingContext()
        {

        }
        public MessagingContext(string json)
        {
            var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            ServiceBusNamespace = obj["serviceBusNamespace"];
            EntityName = obj["entityName"];
            ClientIdentifier = obj["clientIdentifier"];
            SharedAccessSignature = obj["sharedAccessSignature"];
        }

        public string ServiceBusNamespace { get; set; }

        protected string serviceBusPath;
        public virtual string ServiceBusPath
        {
            get { return serviceBusPath ?? string.Format("{0}{1}", ClientIdentifier, ClientIdentifier == string.Empty ? "" : "/"); }
            set { serviceBusPath = value; }
        }

        private string entityName;
        public string EntityName
        {
            get { return entityName ?? string.Empty; }
            set { entityName = value; }
        }

        private string clientIdentiefier;
        public string ClientIdentifier
        {
            get { return clientIdentiefier ?? string.Empty; }
            set { clientIdentiefier = value; }
        }

        private MessagingFactory factory;

        public MessagingFactory Factory
        {
            get
            {
                return factory ?? NewFactory();
            }
            set
            {
                factory = value;
            }
        }


        protected virtual MessagingFactory NewFactory()
        {
            return MessagingFactory.Create( ServiceUri, SasTokenProvider);
        }

        public string SasConnectionString
        {
            get
            {
                return ServiceBusConnectionStringBuilder.CreateUsingSharedAccessKey(ServiceUri, SharedAccessKeyName, SharedAccessKey);
            }
        }

        public virtual Uri ServiceUri
        {
            get
            {
                return ServiceBusEnvironment.CreateServiceUri("sb", ServiceBusNamespace, ServiceBusPath);
            }
        }

        public virtual string FullEntityPath
        {
            get { return string.Format("{0}{1}", ServiceUri.ToString(), EntityName); }
        }

        private TokenProvider sasTokenProvider;
        public TokenProvider SasTokenProvider
        {
            get
            {
                return sasTokenProvider ?? (sasTokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(SharedAccessSignature));
            }
            set { sasTokenProvider = value; }
        }


        private string sharedAccessKeyName;
        public string SharedAccessKeyName
        {
            get { return sharedAccessKeyName ?? "[SharedAccessKeyName]"; }
            set { sharedAccessKeyName = value; }
        }

        private string sharedAccessKey;
        public string SharedAccessKey
        {
            get { return sharedAccessKey ?? "[SharedAccessKey]"; }
            set { sharedAccessKey = value; }
        }

        private string sharedAccessSignature;
        public string SharedAccessSignature
        {
            get { return sharedAccessSignature ?? SharedAccessSignatureTokenProvider.GetSharedAccessSignature(SharedAccessKeyName, SharedAccessKey, FullEntityPath.Trim('/'), new TimeSpan(1,0,0)); }
            set { sharedAccessSignature = value; }
        }

        public object ToObject()
        {
            Object obj = new
            {
                SharedAccessSignature = SharedAccessSignature,
                ServiceBusNamespace = ServiceBusNamespace,
                ClientIdentifier = ClientIdentifier,
                EntityName = EntityName
            };
            return obj;
        }



    }
}
