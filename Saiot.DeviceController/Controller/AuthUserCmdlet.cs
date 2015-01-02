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
    [Cmdlet("Auth", "User")]
    public class AuthUserCmdlet : Cmdlet
    {
      

        protected override void ProcessRecord()
        {
            var facade = ApplicationFacade.Instance;
            facade.ApplicationModel.ApiAuthenticate();
        }
    }
}
