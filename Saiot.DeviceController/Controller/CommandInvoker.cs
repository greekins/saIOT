using Saiot.DeviceController.Application;
using Saiot.DeviceController.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.DeviceController.Controller
{
    public class CommandInvoker
    {
        private Runspace runspace;
        private InitialSessionState iss;

        public CommandInvoker()
        {
            iss = InitialSessionState.Create();
            iss.LanguageMode = PSLanguageMode.FullLanguage;
        }


        public void RegisterCommand(string cmdletName, Type cmdlet)
        {
            iss.Commands.Add(new SessionStateCmdletEntry(cmdletName, cmdlet, ""));
        }

        

        public void InvokeRemoteCommand(RemoteCommand command)
        {
            Pipeline pipeline = createPipeline();
            pipeline.Commands.AddScript(string.Format("{0} -commandId {1}", command.CommandString, command.CommandId));
            invokePipelineRemote(pipeline, command);
        }

        public void InvokeInputCommand(String command)
        {
            Pipeline pipeline = createPipeline();
            pipeline.Commands.AddScript(command);
            invokePipelineLocal(pipeline, command);
        }

        private void invokePipelineLocal(Pipeline pipeline,  string command)
        {
            try
            {
                pipeline.Invoke();
            }
            catch (CommandNotFoundException)
            {
                Notification.Send(Constants.ON_COMMAND_NOT_FOUND, command);
            }
            catch (ParameterBindingException)
            {
                Notification.Send(Constants.ON_COMMAND_NOT_FOUND, command);
            }
        }

        private void invokePipelineRemote(Pipeline pipeline, RemoteCommand command)
        {
            try
            {
                pipeline.Invoke();
            }
            catch (CommandNotFoundException)
            {
                Notification.Send(Constants.ON_REMOTE_COMMAND_NOT_FOUND, command);
            }
            catch (ParameterBindingException)
            {
                Notification.Send(Constants.ON_REMOTE_COMMAND_NOT_FOUND, command);
            }
        }

      




        private Pipeline createPipeline()
        {
            if (runspace != null)
            {
                runspace.Close();
            }
            runspace = RunspaceFactory.CreateRunspace(iss);
            runspace.Open();
            return runspace.CreatePipeline();
        }
    }

    
}
