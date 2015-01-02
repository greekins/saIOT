using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.DeviceController.Application
{
    public class Notification
    {
        public static void Send(string notificationName, object body)
        {
            ApplicationFacade.Instance.Notify(notificationName, body);
        }
    }
}
