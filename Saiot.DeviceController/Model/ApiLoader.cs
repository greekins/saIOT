using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Saiot.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Saiot.DeviceController.Model 
{
    public class ApiLoader
    {
        public string Instance { get; private set; }
        public string Tenant { get; private set; }
        public string ClientId { get; private set; }
        public Uri RedirectUri { get; private set; }
        public string Authority { get; private set; }
        public string ApiResourceId { get; private set; }
        public string ApiBaseAddress { get; private set; }
        private AuthenticationContext authContext;
        private AuthenticationResult authResult;

        public ApiLoader()
        {
            Instance = ConfigurationManager.AppSettings["ida:AADInstance"];
            Tenant = ConfigurationManager.AppSettings["ida:Tenant"];
            ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
            RedirectUri = new Uri(ConfigurationManager.AppSettings["ida:RedirectUri"]);
            Authority = String.Format(Instance, Tenant);
            ApiResourceId = ConfigurationManager.AppSettings["ApiResourceId"];
            ApiBaseAddress = ConfigurationManager.AppSettings["ApiBaseAddress"];

            authContext = new AuthenticationContext(Authority);
        }

        public AuthenticationResult AuthenticateToApi()
        {
            authResult = authContext.AcquireToken(ApiResourceId, ClientId, RedirectUri);
            return authResult;
        }

        public async Task<T> LoadContextAsync<T>(string tenantId, string entityName) where T : MessagingContext, new()
        {
            var path = string.Format("api/sas/{0}/{1}", tenantId, entityName);
            T context = new T()
            {
                ServiceBusNamespace = ConfigurationManager.AppSettings["Microsoft.ServiceBus.Namespace"],
                EntityName = entityName,
                ClientIdentifier = tenantId,
                SharedAccessSignature = JsonConvert.DeserializeObject<string>(await getStringAsync(path))
            };
            return context;
        }


        public async Task<string> GetTenantIdAsync()
        {
            var address = string.Format("api/tenants/{0}/", HttpUtility.UrlEncode(authResult.UserInfo.DisplayableId));
            return await getStringAsync(address);
        }

        public async Task<string> GetActorConfigAsync(string tenantId, string actorName)
        {
            var path = string.Format("api/actors/{0}/config/{1}", tenantId, actorName);
            return await getStringAsync(path);
        }

        public async Task<string> PutActorAsync(string tenantId, string json)
        {
            var path = string.Format("api/actors/{0}/register", tenantId);
            return await putJsonAsync(path, json);
        }

        public async Task<string> PutActorConfigAsync(string tenantId, string actorName, string json)
        {
            var path = string.Format("api/actors/{0}/config/{1}", tenantId, actorName);
            return await putJsonAsync(path, json);
        }

        public async Task<object> PutActorLocationAsync(string tenantId, string actorName, string location)
        {
            var path = string.Format("api/actors/{0}/location/{1}", tenantId, actorName);
            return await putJsonAsync(path, JsonConvert.SerializeObject(location));
        }

        private async Task<string> putJsonAsync(string path, string json)
        {
            var client = getNewClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.PutAsync(ApiBaseAddress + path, new StringContent(json, Encoding.UTF8, "application/json"));
            if(!response.IsSuccessStatusCode) throw new Exception("Failed putting json");
            return await response.Content.ReadAsStringAsync();
        }


        private async Task<string> getStringAsync(string address)
        {
            var client = getNewClient();
            var response = await client.GetAsync(ApiBaseAddress + address);
            if (!response.IsSuccessStatusCode) throw new Exception();
            return await response.Content.ReadAsStringAsync();
        }

        private HttpClient getNewClient()
        {
            if (authResult != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                return client;
            }
            throw new Exception("Not authenticated");
        }

     
    }
}
