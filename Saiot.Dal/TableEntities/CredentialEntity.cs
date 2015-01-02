using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Dal.TableEntities
{
    public class CredentialEntity : TableEntity
    {
        public string SharedAccessKeyName { get; set; }
        public string SharedAccessKey { get; set; }
        public string Entity { get; set; }
    }
}
