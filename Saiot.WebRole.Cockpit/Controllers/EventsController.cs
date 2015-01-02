using Saiot.Bll.Dto;
using Saiot.Bll.Services;
using Saiot.Core.Security;
using Saiot.WebRole.Common.Controllers;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;

namespace Saiot.WebRole.Cockpit.Controllers
{
    [RoutePrefix("api/events")]
    public class EventsController : ProtectedServiceController
    {
        private EventService eventService;

        public EventsController() : base(ConfigurationManager.AppSettings["StorageAccount.ConnectionString"])
        {
            eventService = Assembler.GetEventService();
        }

        [Route("{tenantId}")]
        [ResponseType(typeof(IEnumerable<EventDto>))]
        public IHttpActionResult GetEvents(string tenantId)
        {
            return PerformClaimBasedOperation(() =>
            {
                return Ok(eventService.GetRecentEvents(tenantId));
            });
        }
    }
}
