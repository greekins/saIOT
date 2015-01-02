using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Dal.TableEntities
{
    public class CommandCorrelationEntity : TableEntity
    {
        public string Cmd { get; set; }
        public string Rpl { get; set; }
    }
}
