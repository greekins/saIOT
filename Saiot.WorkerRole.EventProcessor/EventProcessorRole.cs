using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using Microsoft.WindowsAzure.Storage.Table;

namespace Saiot.WorkerRole.EventProcessor
{
    public class EventProcessorRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private string storageConnectionString = CloudConfigurationManager.GetSetting("StorageAccount.ConnectionString");
        private string serviceBusConnectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
        private string eventHubName = "hub";


        public override void Run()
        {

            var tokenSource = new CancellationTokenSource();
            EventProcessorHost host = null;
            EventHubClient client = null;
            try
            {
                client = EventHubClient.CreateFromConnectionString(serviceBusConnectionString, eventHubName);
                host = new EventProcessorHost("singleworker", eventHubName, client.GetDefaultConsumerGroup().GroupName, serviceBusConnectionString, storageConnectionString);
                host.RegisterEventProcessorAsync<Processor>().Wait();
            }
            catch
            {
                Console.WriteLine("Error while starting event processing...");
                tokenSource.Cancel();
                host = null;
            }

            while (!tokenSource.IsCancellationRequested)
            {
                Thread.Sleep(10000);
            }

            if (host != null)
            {
                host.UnregisterEventProcessorAsync().Wait();
            }
            if (client != null)
            {
                client.Close();
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            return base.OnStart();
        }



        public override void OnStop()
        {
            base.OnStop();
        }
    }
}
