
using Saiot.DeviceController.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.DeviceController.Controller
{

    [Cmdlet(VerbsCommon.Set, "Enabled")]
    public class SetEnabledCmdlet : Cmdlet
    {

        [Parameter(Position = 0)]
        public SwitchParameter Off
        {
            get { return off; }
            set { off = value; }
        }
        private bool off;


        protected override void ProcessRecord()
        {
            var facade = ApplicationFacade.Instance;
            if (off)
            {
                facade.CommandModel.StopListening();
            }
            else
            {
                facade.CommandModel.StartListening();
            }
        }

       
    }
}
