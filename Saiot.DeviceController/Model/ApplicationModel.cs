using Saiot.DeviceController.Application;
using Newtonsoft.Json;
using Saiot.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Saiot.DeviceController.Model
{
    public class ApplicationModel
    {
        public bool IsAadAuthenticated { get; private set; }
        public ApiLoader ApiLoader { get; private set; }
        public MessagingContext CmdQueueContext { get; private set; }
        public MessagingContext RplQueueContext { get; private set; }
        public EventHubMessagingContext HubContext { get; private set; }

        public string TenantId { get; set; }

        public string TableToken { get; private set; }
        public Uri TableEndpoint { get; private set; }

        public ApplicationModel()
        {
            IsAadAuthenticated = false;
            ApiLoader = new ApiLoader();
        }

        public async void ApiAuthenticate()
        {
            AuthenticationResult resultToken = null;
            bool success = false;
            try
            {
                resultToken = ApiLoader.AuthenticateToApi();
                IsAadAuthenticated = true;

                TenantId = JsonConvert.DeserializeObject<string>(await ApiLoader.GetTenantIdAsync());

                CmdQueueContext = await ApiLoader.LoadContextAsync<MessagingContext>(TenantId, "cmd");
                RplQueueContext = await ApiLoader.LoadContextAsync<MessagingContext>(TenantId, "rpl");
                Notification.Send(Constants.LOG, "Access for commands granted...");

                HubContext = await ApiLoader.LoadContextAsync<EventHubMessagingContext>(TenantId, "hub");
                Notification.Send(Constants.LOG, "Access for events granted...");
                success = true;
               
            }
            catch (Exception e)
            {
                IsAadAuthenticated = false;
                Notification.Send(Constants.ON_AUTH_ERROR, "Error during authentication process");
            }
            if (!success) return;
            Notification.Send(Constants.ON_AUTH_SUCCESS, string.Format("Welcome, {0}", resultToken.UserInfo.DisplayableId));
        }

        public async void ApiRegisterActor(IActor actor)
        {
            var dto = new
            {
                Name = actor.Name,
                Location = actor.Location,
                Type = actor.Type
            };
            var json = JsonConvert.SerializeObject(dto);
            try
            {
                var result = await ApiLoader.PutActorAsync(TenantId, json);
                Notification.Send(Constants.LOG, "Successfully registered actor...");
            }
            catch
            {
                Notification.Send(Constants.LOG_ERROR, "Failed registering actor");
            }
        }

        public async void ApiUpdateActorConfig(IActor actor)
        {
            var json = JsonConvert.SerializeObject(actor.Config);
            try
            {
                var result = await ApiLoader.PutActorConfigAsync(TenantId, actor.Name, json);
                Notification.Send(Constants.LOG, "Successfully updated actor config");
            }
            catch
            {
                Notification.Send(Constants.LOG, "Failed updating actor config");
            }
        }

        public async void ApiLoadActorConfig(IActor actor)
        {
            try
            {
                var result = await ApiLoader.GetActorConfigAsync(TenantId, actor.Name);
                var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                actor.SetScalarConfig(config);
                actor.Config = config;
                Notification.Send(Constants.LOG, "Successfully loaded actor config");
            }
            catch
            {
                Notification.Send(Constants.LOG, "Failed loading actor config");
            }
        }

        public async void ApiUpdateActorLocation(IActor actor)
        {
            try
            {
                var result = await ApiLoader.PutActorLocationAsync(TenantId, actor.Name, actor.Location);
                Notification.Send(Constants.LOG, "Successfully updated actor location");
            }
            catch
            {
                Notification.Send(Constants.LOG, "Failed updating actor location");
            }
        }
    }
}
