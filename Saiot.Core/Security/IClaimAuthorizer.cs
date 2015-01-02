using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Security
{
    public interface IClaimAuthorizer
    {
        bool ApprovePartition(TableClaim claim);
        bool ApproveTable(TableClaim claim);
        bool ApprovePermissions(TableClaim claim);
    }
}
