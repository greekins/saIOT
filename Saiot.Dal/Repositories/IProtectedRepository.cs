using Saiot.Core.Security;
using Saiot.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Dal.Repositories
{
    public interface IProtectedRepository
    {
        IStorageProvider StorageProvider { get; set; }
    }
}
