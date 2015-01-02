using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Saiot.Core.Storage;
using Saiot.Core.Security;
using Saiot.Bll;
using Microsoft.WindowsAzure.Storage;
using Saiot.Core.Messaging;
using Newtonsoft.Json;
using Saiot.Bll.Exceptions;

namespace Saiot.WorkerRole.Correlation
{
    public class WorkerRole : RoleEntryPoint, IStorageProvider
    {

        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);
        private ServiceAssembler assembler;
        private CloudStorageAccount storageAccount;

        public override void Run()
        {
            Client.OnMessage((receivedMessage) =>
            {
                try
                {
                    var body = receivedMessage.GetBody<string>();
                    assembler.GetCommandService().RegisterReply(CloudConfigurationManager.GetSetting("tenant"), receivedMessage.Label, body);
                    receivedMessage.Complete();
                }
                catch (EntryNotFoundException)
                {
                    receivedMessage.Complete();
                }
                catch (Exception)
                {
                    receivedMessage.Abandon();
                }
            }, new OnMessageOptions { AutoComplete = false });

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageAccount.ConnectionString"));
            assembler = new ServiceAssembler(this, storageAccount);

            var context = new MessagingContext
            {
                ServiceBusNamespace = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.Namespace"),
                EntityName = "rpl",
                ClientIdentifier = CloudConfigurationManager.GetSetting("tenant"),
                SharedAccessSignature = assembler.GetTenantService().GetServiceBusSignature(CloudConfigurationManager.GetSetting("tenant"), "rpl", "CorrelationRole")
            };
            Client = context.Factory.CreateQueueClient(context.EntityName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            Client.Close();
            CompletedEvent.Set();
            base.OnStop();
        }

        public Microsoft.WindowsAzure.Storage.Table.CloudTable AcquireReference(TableClaim claim)
        {
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(claim.TableName);
            table.CreateIfNotExists();
            return table;
        }
    }
}
