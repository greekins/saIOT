using Newtonsoft.Json;
using Saiot.DeviceController.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.DeviceController.Controller
{

    [Cmdlet(VerbsCommon.Set, "Config")]
    public class SetConfigCmdlet : Cmdlet
    {

        [Parameter(Position = 0, Mandatory = true)]
        public string Name { get { return name; } set { name = value; } }
        private string name;

        [Parameter(Position = 0, Mandatory = false)]
        public string Location { get { return location; } set { location = value; } }
        private string location;


        [Parameter(Position = 2, Mandatory=true)]
        public string Config
        {
            get { return config; }
            set { config = value; }
        }
        private string config;

       

        [Parameter(Position = 3, Mandatory = true)]
        public string CommandId
        {
            get { return commandId; }
            set { commandId = value; }
        }
        private string commandId;


        protected override void ProcessRecord()
        {
            var facade = ApplicationFacade.Instance;
            var model = facade.ActorModel;
            var actor = model.GetActorWithName(Name);

            if (actor == null) return;

            if (Location != null && Location != string.Empty && actor.Location != Location)
            {
                actor.Location = Location;
                Notification.Send(Constants.ON_ACTOR_LOCATION_CHANGED, actor);
            }
            
            object info = null;
            var status = string.Empty;

            try
            {
                var conf = Encoding.UTF8.GetString(Convert.FromBase64String(Config));
                actor.Config = JsonConvert.DeserializeObject<Dictionary<string,string>>(conf);
                status = "ok";
                info = actor.Config;
                Notification.Send(Constants.ON_ACTOR_CONFIG_UPDATED, actor);
            }
            catch (Exception e)
            {
                status = "error";
                info = e.Message;
            }
            


            facade.CommandModel.ReplyCommand(CommandId, new { 
                status = status,
                info = info,
                actor = actor.Name
            });
        }
    }
}
