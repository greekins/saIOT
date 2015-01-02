using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging.Types
{
    public class CommandMessage : IIotMessage
    {
        public string Name { get; set; }
        public string PriorityLevel { get; set; }
        public string Body { get; set; }

        public string Type { get { return "CommandMessage"; } }
    }
}
