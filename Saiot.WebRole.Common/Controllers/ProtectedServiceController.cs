using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Saiot.Bll;
using Saiot.Bll.Services;
using Saiot.Core.Security;
using Saiot.Core.Storage;
using System.Configuration;
using System.Web.Http;

namespace Saiot.WebRole.Common.Controllers
{

    public delegate IHttpActionResult ClaimBasedOperation();
    [Authorize]
    public class ProtectedServiceController : ApiController, IClaimAuthorizer, IStorageProvider
    {

        public CloudStorageAccount StorageAccount { get; protected set; }
        public ServiceAssembler Assembler { get; protected set; }
        public TenantService TenantService { get; protected set; }

        public ProtectedServiceController(string storageConnectionString)
        {
            StorageAccount = CloudStorageAccount.Parse(storageConnectionString);
            Assembler = new ServiceAssembler(this, StorageAccount);
            TenantService = Assembler.GetTenantService();
        }

        public CloudTable AcquireReference(TableClaim claim)
        {
            var identityName = User.Identity.Name;
            if (identityName.Contains("#"))
            {
                identityName = identityName.Split('#')[1];
            }
            claim.User = identityName;
            claim.AuthorizeWith(this);
            var token = TenantService.GetTableStorageToken(claim);
            StorageCredentials credentials = new StorageCredentials(token);
            CloudTableClient client = new CloudTableClient(StorageAccount.TableEndpoint, credentials);
            return client.GetTableReference(claim.TableName);
        }

        public bool ApprovePartition(TableClaim claim)
        {
            var userBelongsToTenant = TenantService.UserBelongsToTenant(claim.User, claim.PartitionKey);
            return userBelongsToTenant;
        }

        public bool ApproveTable(TableClaim claim)
        {
            return true;
        }

        public bool ApprovePermissions(TableClaim claim)
        {
            return true;
        }

        protected IHttpActionResult PerformClaimBasedOperation(ClaimBasedOperation operation)
        {
            try
            {
                return operation.Invoke();
            }
            catch (ClaimAuthorizationException)
            {
                return Unauthorized();
            }
            catch
            {
                return BadRequest();
            }
        }
    }

}
