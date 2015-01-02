using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Saiot.WebRole.Cockpit.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult SignIn()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Welcome", "Welcome");
            }

            WsFederationConfiguration config = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration;
            string callbackUrl = Url.Action("Welcome", "Welcome", routeValues: null, protocol: Request.Url.Scheme);
            SignInRequestMessage signInRequest = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest(
                uniqueId: String.Empty,
                returnUrl: callbackUrl,
                rememberMeSet: false);
            signInRequest.SetParameter("wtrealm", IdentityConfig.Realm ?? config.Realm);
            return new RedirectResult(signInRequest.RequestUrl.ToString());
        }

        public ActionResult SignOut()
        {
            WsFederationConfiguration config = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration;

            string callbackUrl = Url.Action("Index", "Home", routeValues: null, protocol: Request.Url.Scheme);
            SignOutRequestMessage signoutMessage = new SignOutRequestMessage(new Uri(config.Issuer), callbackUrl);
            signoutMessage.SetParameter("wtrealm", IdentityConfig.Realm ?? config.Realm);
            FederatedAuthentication.SessionAuthenticationModule.SignOut();

            return new RedirectResult(signoutMessage.WriteQueryString());
        }
    }
}
