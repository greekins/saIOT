using Saiot.Bll.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Bll.Exceptions
{
    public class CorrelationNotFoundException : EntryNotFoundException
    {

        public CommandCorrelationDto CommandCorrelation { get; private set; }
        public CorrelationNotFoundException() { }
        public CorrelationNotFoundException(string message) : this(null, message) { }
        public CorrelationNotFoundException(CommandCorrelationDto commandCorrelation)
            : this(commandCorrelation, getMessage(commandCorrelation)) { }

        public CorrelationNotFoundException(CommandCorrelationDto commandCorrelation, string message)
            : base(message)
        {
            CommandCorrelation = commandCorrelation;
        }

        private static string getMessage(CommandCorrelationDto commandCorrelation)
        {
            return string.Format("Correlation not found: [{0}]", commandCorrelation.Cmd);
        } 
    }
}
