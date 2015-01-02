using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Claims;
using System.IdentityModel.Services;
using System.Linq;
using System.Web.Helpers;
using Saiot.WebRole.Cockpit.Utils;

namespace Saiot.WebRole.Cockpit
{
    public static class IdentityConfig
    {
        public static string AudienceUri { get; private set; }
        public static string Realm { get; private set; }

        public static void ConfigureIdentity()
        {
            RefreshValidationSettings();
            Realm = ConfigurationManager.AppSettings["ida:realm"];
            AudienceUri = ConfigurationManager.AppSettings["ida:AudienceUri"];
            if (!String.IsNullOrEmpty(AudienceUri))
            {
                UpdateAudienceUri();
            }

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
        }

        public static void RefreshValidationSettings()
        {
            string metadataLocation = ConfigurationManager.AppSettings["ida:FederationMetadataLocation"];
            DatabaseIssuerNameRegistry.RefreshKeys(metadataLocation);
        }

        public static void UpdateAudienceUri()
        {
            int count = FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                .AudienceRestriction.AllowedAudienceUris.Count(
                    uri => String.Equals(uri.OriginalString, AudienceUri, StringComparison.OrdinalIgnoreCase));
            if (count == 0)
            {
                FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                    .AudienceRestriction.AllowedAudienceUris.Add(new Uri(IdentityConfig.AudienceUri));
            }
        }
    }
}
