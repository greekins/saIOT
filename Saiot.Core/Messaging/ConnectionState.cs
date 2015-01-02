using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Messaging
{
    public enum ConnectionState { Unstarted = 0, Sending, Receiving, Success, Failed }
}
