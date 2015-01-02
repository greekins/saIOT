using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saiot.Core.Messaging;
using Saiot.DeviceController.Application;

namespace Saiot.DeviceController.Model
{

    public class RemoteCommandModel
    {
        private CommandProcessorHost commandProcessorHost;
        private QueueHelper replyQueueHelper;
        private Dictionary<string, RemoteCommand> messages;
        public LinkedList<RemoteCommand> History { get; private set; }

        public RemoteCommandModel(MessagingContext incomingContext, MessagingContext outgoingContext)
        {
            commandProcessorHost = new CommandProcessorHost(incomingContext);
            replyQueueHelper = new QueueHelper(outgoingContext);
            messages = new Dictionary<string, RemoteCommand>();
            History = new LinkedList<RemoteCommand>();
            
            commandProcessorHost.OnMessageReceivedHandler += onMessageReceived;
            commandProcessorHost.OnFeedbackHandler += onFeedback;
        }

        public void StartListening()
        {
            commandProcessorHost.Start();
        }

        public void StopListening()
        {
            commandProcessorHost.Stop();
        }

        public void ReplyCommand(string commandId, object body)
        {
            RemoteCommand command;
            var success = messages.TryGetValue(commandId, out command);
            if (success)
            {
                messages.Remove(commandId);
                command.Reply(body);
            }
            else
            {
                throw new Exception("Cannot reply to command");
            }
            
        }

        public void DismissCommand(string commandId)
        {
            RemoteCommand command;
            var success = messages.TryGetValue(commandId, out command);
            if (success)
            {
                command.Dismiss();
            }
        }

        private void onMessageReceived(object sender, ProcessorHostMessageEventArgs args)
        {
            var commandMessage = new RemoteCommand(args.Message, replyQueueHelper);
            if (!messages.ContainsKey(commandMessage.CommandId))
            {
                messages.Add(commandMessage.CommandId, commandMessage);
                History.AddLast(commandMessage);
                Notification.Send(Constants.ON_REMOTE_COMMAND_RECEIVE_SUCCESS, commandMessage);
            }
        }

        private void onMessageError(object sender, ProcessorHostErrorEventArs args)
        {
            Notification.Send(Constants.ON_REMOTE_COMMAND_RECEIVE_ERROR, args.ErrorMessage);
        }

        private void onFeedback(object sender, ProcesserHostFeedbackEventArgs args)
        {
            Notification.Send(Constants.ON_COMMAND_MODEL_FEEDBACK, args.Feedback);
        }

    }
}
