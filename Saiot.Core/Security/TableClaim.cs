using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Security
{
    public class TableClaim
    {
        public string PartitionKey { get; set; }
        public SharedAccessTablePermissions Permissions { get; set; }
        public string TableName { get; set; }
        public string User { get; set; }
        public bool IsAuthorized { get; private set; }

        public void AuthorizeWith(IClaimAuthorizer authorizer)
        {
            if (!authorizer.ApproveTable(this)) throw new ClaimAuthorizationException(string.Format("Access to table '{0}' not allowed.", TableName));
            if (!authorizer.ApprovePartition(this)) throw new ClaimAuthorizationException(string.Format("Access to partition '{0}' not allowed.", PartitionKey));
            if (!authorizer.ApprovePermissions(this)) throw new ClaimAuthorizationException("Claimed permissions do not match users permissions.");
            IsAuthorized = true;
       } 

    }
}
