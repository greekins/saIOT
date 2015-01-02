using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Security
{
    public class ClaimAuthorizationException : System.Exception
    {
        public ClaimAuthorizationException()
        {

        }

        public ClaimAuthorizationException(string message) : base(message) { }
    }
}
