using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using Shared.Messaging.Adapters;

namespace Shared.Messaging
{
    public class MessagingHelper
    {

        public IMessagingContext context { get; private set; }
        protected string connectionString;

        public MessagingHelper(IMessagingContext context)
        {
            this.context = context;
        }

        public virtual MessagingFactory CreateMessagingFactory()
        {
            string connectionString = CreateSasConnectionString();
            return CreateFactory(connectionString); 
        }

        protected virtual MessagingFactory CreateFactory(string connectionString)
        {
            return MessagingFactory.CreateFromConnectionString(connectionString);
        }

        public virtual string CreateSasConnectionString()
        {
            connectionString = String.Format("Endpoint=sb://{0}.servicebus.windows.net/{1};SharedAccessKeyName={2};SharedAccessKey={3}",
                context.ServiceBusNamespace,
                context.ServiceBusPath,
                context.SharedSecretIssuer,
                context.SharedSecretKey);
            return connectionString;
        }


        public virtual BrokeredMessage ConvertToBrokeredMessage(Object original)
        {
            // use original.GetType().GetProperties() and assign values to BrokeredMessage-Object
            // http://stackoverflow.com/a/737156/925493

            BrokeredMessage converted = new BrokeredMessage();
            converted.Properties["Foo"] = "foo";
            converted.Properties["Bar"] = "bar";
            return converted;
        }



        public virtual IMessageSender CreateSenderForRessource(string ressourceName)
        {
            if (IsQueueEntity())
            {
                return new QueueMessageSender(CreateQueueClient(ressourceName));
            }
            else if (IsAnyEnitiy())
            {
                return new GeneralMessageSender(CreateMessageSender(ressourceName));
            }
            return new NullObjectMessageSender();
        }

        public virtual IMessageReceiver CreateReceiverForRessource(string ressourceName)
        {
            if (IsQueueEntity())
            {
                return new QueueMessageReceiver(CreateQueueClient(ressourceName));
            }
            else if (IsAnyEnitiy())
            {
                return new GeneralMessageReceiver(CreateMessageReceiver(ressourceName));
            }
            return null;
        }

        public virtual MessageSender CreateMessageSender(string ressourceName)
        {
            var factory = CreateMessagingFactory();
            return factory.CreateMessageSender(ressourceName);
        }

        public virtual MessageReceiver CreateMessageReceiver(string ressourceName)
        {
            var factory = CreateMessagingFactory();
            return factory.CreateMessageReceiver(ressourceName);
        }

        public virtual QueueClient CreateQueueClient(string ressourceName)
        {
            return QueueClient.CreateFromConnectionString(CreateSasConnectionString(), ressourceName);
        }

        protected virtual bool IsQueueEntity()
        {
            return context.ServiceBusEntity == ServiceBusEntity.queue_entity;
        }
        protected virtual bool IsAnyEnitiy()
        {
            return context.ServiceBusEntity == ServiceBusEntity.any_entity;
        }

        public virtual void SendMessage(object message, string ressourceName)
        {
            IMessageSender sender = CreateSenderForRessource(ressourceName);
            sender.Send(ConvertToBrokeredMessage(message));
        }

        public virtual string ReceiveMessage(string ressourceName)
        {
            MessagingFactory factory = CreateMessagingFactory();
            var receiver = factory.CreateMessageReceiver(ressourceName);
            var message = receiver.Receive();
            return message.ToString();
        }


    }
}
