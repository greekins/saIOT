using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

namespace Saiot.WebRole.ClientApi
{
    [RoutePrefix("api/sas")]
    public class SignaturesController : Saiot.WebRole.Common.Controllers.ProtectedServiceController {

        public SignaturesController() : base(ConfigurationManager.AppSettings["StorageAccount.ConnectionString"]) { }
        
        [Route("{tenantId}/{entityName}")]
        public string GetEntity(string tenantId, string entityName)
        {
            
            try
            {
                return TenantService.GetServiceBusSignature(tenantId, entityName, "DeviceController");
            }
            catch (Saiot.Core.Security.ClaimAuthorizationException)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
        

    }
}