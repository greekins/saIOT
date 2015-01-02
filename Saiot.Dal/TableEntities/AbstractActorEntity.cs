using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Dal.TableEntities
{
    public class AbstractActorEntity : TableEntity
    {

        public string Location { get; set; }
        public string Name { get; set; }
        public string Config { get; set; }
        public string Type { get; set; }
    }
}
