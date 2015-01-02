using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging.Types
{
    public class StatusMessage : IIotMessage
    {
        public string Type
        {
            get { return "StatusMessage"; }
        }

        public string CommandRef { get; set; }
        public string Info { get; set; }
    }
}
