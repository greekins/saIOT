using Newtonsoft.Json;
using Saiot.Bll.Dto;
using Saiot.Bll.Services;
using Saiot.WebRole.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Saiot.WebRole.ClientApi
{
    [RoutePrefix("api/actors")]
    public class ActorsController : ProtectedServiceController
    {
        private ActorService actorService;

        public ActorsController() : base(ConfigurationManager.AppSettings["StorageAccount.ConnectionString"])
        {
            actorService = Assembler.GetActorService();
        }

        [Route("{tenantId}/register")]
        public IHttpActionResult PutActor(string tenantId, [FromBody] AbstractActorDto dto) 
        {

            return PerformClaimBasedOperation(() =>
            {
                dto.TenantId = tenantId;
                var result = actorService.RegisterActor(dto);
                return Ok(result);
            });
        }

        [Route("{tenantId}/config/{actorName}")]
        public IHttpActionResult GetConfig(string tenantId, string actorName)
        {
            return PerformClaimBasedOperation(() =>
            {
                return Ok(JsonConvert.DeserializeObject<Dictionary<string, string>>(actorService.GetConfig(tenantId, actorName)));
            });
        }

        [Route("{tenantId}/config/{actorName}")]
        public IHttpActionResult PutConfig(string tenantId, [FromBody] object config, string actorName)
        {
            return PerformClaimBasedOperation(() => 
            {
                actorService.UpdateConfig(tenantId, actorName, JsonConvert.SerializeObject(config));
                return Ok(config);
            });
        }

        [Route("{tenantId}/location/{actorName}")]
        public IHttpActionResult PutLocation(string tenantId, string actorName, [FromBody] string location)
        {
            return PerformClaimBasedOperation(() => 
            {
                actorService.UpdateLocation(tenantId, actorName, location);
                return Ok(location);
            });
        }
    }
}