using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Saiot.WebRole.Cockpit.Models;
using Saiot.Bll.Services;
using Saiot.Bll;
using Saiot.Core.Storage;
using Microsoft.WindowsAzure.Storage;

namespace Saiot.WebRole.Cockpit.Controllers
{
    public class WelcomeController : Controller, IStorageProvider
    {

        private TenantService tenantService;
        public WelcomeController()
        {
            var storageAccount = CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.AppSettings["StorageAccount.ConnectionString"]);
            var assembler = new ServiceAssembler(this, storageAccount);
            tenantService = assembler.GetTenantService();
        }
        public ActionResult Welcome()
        {
            if (Request.IsAuthenticated)
            {

                ViewBag.tenant = tenantService.GetTenantOf(User.Identity.Name);
                return View();
            }
            else
            {
               return RedirectToAction("Index", "Home");
            }
            
        }

        public Microsoft.WindowsAzure.Storage.Table.CloudTable AcquireReference(Core.Security.TableClaim claim)
        {
            throw new NotImplementedException();
        }
    }
}