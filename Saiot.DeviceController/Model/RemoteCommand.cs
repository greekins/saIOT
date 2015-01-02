using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Saiot.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.DeviceController.Model
{
    public class RemoteCommand
    {
        private BrokeredMessage originalMessage;
        private DateTime enqueuedTimeUtc;
        private QueueHelper helper;
        public string CommandId { get; private set; }
        public string CommandString { get; private set; }
        public bool IsReplied { get; set; }
        public RemoteCommand(BrokeredMessage originalMessage, QueueHelper replyQueueHelper)
        {
            this.originalMessage = originalMessage;
            this.helper = replyQueueHelper;
            enqueuedTimeUtc = originalMessage.EnqueuedTimeUtc;
            CommandId = originalMessage.Label;
            CommandString = originalMessage.GetBody<String>();
        }

        public void Reply(Object body)
        {
            originalMessage.Complete();
            IsReplied = true;
            helper.SendMessage(new BrokeredMessage(JsonConvert.SerializeObject(body)) { Label = CommandId });
        }

        public void Dismiss()
        {
            originalMessage.Abandon();
        }

        public override string ToString()
        {
            return string.Format("[{0}, {2}] {1}", enqueuedTimeUtc, CommandString, CommandId);
        }

    }
}
