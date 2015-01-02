using Saiot.WebRole.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Saiot.WebRole.ClientApi
{
    [RoutePrefix("api/tenants")]
    public class TenantsController : ProtectedServiceController
    {
       
        public TenantsController() : base(ConfigurationManager.AppSettings["StorageAccount.ConnectionString"]) { }

        [Route("{userIdentity}")]
        public IHttpActionResult GetByUser(string userIdentity)
        {
            if (!User.Identity.Name.Contains(userIdentity)) throw new UnauthorizedAccessException();
            return PerformClaimBasedOperation(() =>
            {
                var tenantId = TenantService.GetTenantOf(userIdentity);
                return Ok(tenantId);
            });
        }
    }
}