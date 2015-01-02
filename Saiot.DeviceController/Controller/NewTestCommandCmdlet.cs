using Saiot.DeviceController.Application;
using Saiot.DeviceController.Model;
using Saiot.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.DeviceController.Controller
{
    [Cmdlet(VerbsCommon.New, "TestCommand")]
    public class NewTestCommandCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory=true)]
        public string Command 
        {
            get { return command;}
            set { command = value;}
        }
        private string command;

        protected override void ProcessRecord()
        {
            var facade = ApplicationFacade.Instance;

            var helper = new QueueHelper(facade.ApplicationModel.CmdQueueContext);
            helper.SendMessage(new Microsoft.ServiceBus.Messaging.BrokeredMessage(Command){Label = Guid.NewGuid().ToString()});
        }
    }
}
