using Microsoft.WindowsAzure.Storage.Table;
using Saiot.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Storage
{
    public interface IStorageProvider
    {
        CloudTable AcquireReference(TableClaim claim);
    }
}
