using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Configuration;
using Saiot.Core.Messaging;
using Saiot.Dal.TableEntities;
using Microsoft.WindowsAzure;
using Saiot.Bll;
using Saiot.Core.Storage;
using Saiot.Bll.Dto;

namespace Saiot.WorkerRole.EventProcessor
{
    class Processor : IEventProcessor, IStorageProvider
    {
        PartitionContext partitionContext;
        Stopwatch checkpointStopWatch;

        CloudStorageAccount storageAccount;
        CloudTableClient tableClient;
        CloudTable table;

        public Processor()
        {
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        public Task OpenAsync(PartitionContext context)
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageAccount.ConnectionString"));
            tableClient = storageAccount.CreateCloudTableClient();
            this.partitionContext = context;
            this.checkpointStopWatch = new Stopwatch();
            this.checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            

            var assembler = new ServiceAssembler(this, storageAccount);
            var eventService = assembler.GetEventService();

            foreach (EventData eventData in messages)
            {
                try
                {
                    var json = System.Text.Encoding.UTF8.GetString(eventData.GetBytes());
                    var dto = JsonConvert.DeserializeObject<EventDto>(json);

                    dto.TenantId = eventData.PartitionKey;
                    dto.EventId = Guid.NewGuid().ToString();
                    dto.Timestamp = eventData.EnqueuedTimeUtc;

                    eventService.SaveEvent(dto);
                }
                catch (Exception exx)
                {
                    Console.WriteLine(exx.Message);
                }
            }

            if (this.checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(1))
            {
                await context.CheckpointAsync();
                lock (this)
                {
                    this.checkpointStopWatch.Reset();
                }
            }
        }

        public CloudTable AcquireReference(Core.Security.TableClaim claim)
        {
            if (table == null || table.Name != claim.TableName)
            {
                table = tableClient.GetTableReference(claim.TableName);
                table.CreateIfNotExists();
            }
            return table;
        }
    }
}
