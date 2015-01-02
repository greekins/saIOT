using Saiot.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Test.Fakes
{
    public class ClaimAuthorizerFake : IClaimAuthorizer
    {

        private bool approvePartition;
        private bool approveTable;
        private bool approvePermissions;

        public ClaimAuthorizerFake(bool approvePartition, bool approveTable, bool approvePermissions)
        {
            this.approvePartition = approvePartition;
            this.approvePermissions = approvePermissions;
            this.approveTable = approveTable;
        }

        public bool ApprovePartition(TableClaim claim)
        {
            return approvePartition;
        }

        public bool ApproveTable(TableClaim claim)
        {
            return approveTable;
        }

        public bool ApprovePermissions(TableClaim claim)
        {
            return approvePermissions;
        }
    }
}
