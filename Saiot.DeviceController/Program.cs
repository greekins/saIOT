using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Saiot.DeviceController.Application;
using Saiot.DeviceController.Model;

namespace Saiot.DeviceController
{
    class Program
    {


        static void Main(string[] args)
        {
            var facade = ApplicationFacade.Instance;
            facade.Startup();

            while (true)
            {
                string input = Console.ReadLine();
                facade.Notify(Constants.ON_INPUT_COMMAND_ENTERED, input);
            }
        }
    }
}
