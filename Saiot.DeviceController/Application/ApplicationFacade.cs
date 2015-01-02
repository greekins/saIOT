
using Saiot.DeviceController.Controller;
using Saiot.DeviceController.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.DeviceController.Application
{
    public class ApplicationFacade
    {
        private static ApplicationFacade defaultFacade;
        private CommandInvoker invoker;

        public RemoteCommandModel CommandModel { get; private set; }
        public ActorModel ActorModel { get; private set; }
        public ApplicationModel ApplicationModel { get; private set; }

        private ApplicationFacade() 
        {
            
        }

        public static ApplicationFacade Instance 
        {
            get
            {
                defaultFacade = defaultFacade ?? new ApplicationFacade();
                return defaultFacade;
            }
        }

        public void Startup()
        {
            invoker = new CommandInvoker();
            invoker.RegisterCommand("Auth-User", typeof(AuthUserCmdlet));

            ApplicationModel = new ApplicationModel();
            Console.WriteLine("Authenticate User...");
            ApplicationModel.ApiAuthenticate();
        }


        private void continueAuthenticated()
        {
            invoker.RegisterCommand("Set-Enabled", typeof(SetEnabledCmdlet));
            invoker.RegisterCommand("New-TestCommand", typeof(NewTestCommandCmdlet));
            invoker.RegisterCommand("Show-History", typeof(ShowHistoryCmdlet));
            invoker.RegisterCommand("Set-Config", typeof(SetConfigCmdlet));

            CommandModel = new RemoteCommandModel(ApplicationModel.CmdQueueContext, ApplicationModel.RplQueueContext);
            ActorModel = new ActorModel(ApplicationModel.HubContext);
            var actor = new SprenklerActor("Sprinkler 1");
            ActorModel.RegisterActor(actor);
            ApplicationModel.ApiLoadActorConfig(actor);
            CommandModel.StartListening();
        }


        public void Notify(string notificationName, object body)
        {
            if (notificationName == Constants.ON_REMOTE_COMMAND_RECEIVE_SUCCESS)
            {
                invoker.InvokeRemoteCommand(body as RemoteCommand);
            }
            else if (notificationName == Constants.ON_INPUT_COMMAND_ENTERED)
            {
                invoker.InvokeInputCommand(body as string);
            }
            else if (notificationName == Constants.ON_AUTH_SUCCESS)
            {
                logSuccess(body.ToString());
                continueAuthenticated();
            }
            else if (notificationName == Constants.ON_AUTH_ERROR)
            {
                logError(body.ToString());
            }
            else if (notificationName == Constants.ON_COMMAND_MODEL_FEEDBACK)
            {
                logInfo(body.ToString());
            }
            else if (notificationName == Constants.ON_COMMAND_NOT_FOUND)
            {
                logError(string.Format("Command not found: [{0}]", body));
            }
            else if (notificationName == Constants.LOG)
            {
                Console.WriteLine(body.ToString());
            }
            else if (notificationName == Constants.LOG_INFO)
            {
                logInfo(body.ToString());
            }
            else if (notificationName == Constants.LOG_SUCCESS)
            {
                logSuccess(body.ToString());
            }
            else if (notificationName == Constants.LOG_ERROR)
            {
                logError(body.ToString());
            }
            else if (notificationName == Constants.ON_REMOTE_COMMAND_NOT_FOUND)
            {
                var cmd = body as RemoteCommand;
                var replyBody = new
                {
                    status = "error",
                    info = string.Format("command not found: {0}", cmd.CommandString)
                };
                CommandModel.ReplyCommand(cmd.CommandId, replyBody);
                logError(string.Format("Command not found: [{0}]", cmd.CommandString));
            }
            else if (notificationName == Constants.ON_ACTOR_MEASURE_DATA)
            {
                ActorModel.SendMeasureData(body as OnActorMeasureDataEventArgs);
            }
            else if (notificationName == Constants.ON_SEND_MEASURE_DATA_FAILED)
            {
                logError(string.Format("Failed sending measure data from sensor '{0}'", body.ToString()));
            }
            else if (notificationName == Constants.ON_ACTOR_REGISTERED)
            {
                ApplicationModel.ApiRegisterActor(body as IActor);
            }
            else if (notificationName == Constants.ON_ACTOR_CONFIG_UPDATED)
            {
                ApplicationModel.ApiUpdateActorConfig(body as IActor);
            }
            else if (notificationName == Constants.ON_ACTOR_LOCATION_CHANGED)
            {
                ApplicationModel.ApiUpdateActorLocation(body as IActor);
            }
            else if (notificationName == Constants.ON_ACTOR_CONFIG_LOAD_SUCCESS)
            {

            }
           
        }




        private void logInfo(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private void logError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private void logSuccess(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
