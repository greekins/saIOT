using System.Web;
using System.Web.Mvc;

namespace Saiot.WebRole.Cockpit
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
