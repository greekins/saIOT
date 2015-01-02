using Saiot.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Test.Fakes
{
    public class StorageSasProducerFake : StorageSasProducer
    {

        public StorageSasProducerFake() : base("") { }

        protected override string requestSasTokenImpl(string partitionKey, string tableName, Microsoft.WindowsAzure.Storage.Table.SharedAccessTablePermissions permissions)
        {
            return "tokenstringfake";
        }
    }
}
