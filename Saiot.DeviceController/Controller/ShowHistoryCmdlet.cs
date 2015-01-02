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
    [Cmdlet(VerbsCommon.Show, "History")]
    public class ShowHistoryCmdlet : Cmdlet
    {
      

        protected override void ProcessRecord()
        {
            var facade = ApplicationFacade.Instance;
            var history = facade.CommandModel.History;
            if (history.Count() == 0)
            {
                facade.Notify(Constants.LOG_INFO, "History is empty...");
            }
            else
            {
                foreach (var cmd in history)
                {
                    Notification.Send(Constants.LOG_INFO, cmd.ToString());
                }
            }
            
        }
    }
}
