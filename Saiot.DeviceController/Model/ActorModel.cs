using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Linq;
using Saiot.DeviceController.Application;
using Saiot.Core.Messaging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace Saiot.DeviceController.Model
{
    public class ActorModel
    {
        private LinkedList<IActor> actors;
        private EventHubHelper helper;
        private EventHubMessagingContext context;

        private string tableToken;
        private Uri tableEndpoint;

        public ActorModel(EventHubMessagingContext context)
        {
            actors = new LinkedList<IActor>();
            this.context = context;
            helper = new EventHubHelper(context);
        }

        public void RegisterActor(IActor actor)
        {
            actors.AddLast(actor);
            actor.OnActorMeasureDataHandler += onActorMeasureData;
            actor.OnActorInfoHandler += onActorInfo;
            Notification.Send(Constants.ON_ACTOR_REGISTERED, actor);
        }

        public void UnregisterActor(IActor actor)
        {
            actors.Remove(actor);
            actor.OnActorMeasureDataHandler -= onActorMeasureData;
            Notification.Send(Constants.ON_ACTOR_UNREGISTERED, actor);
        }

        public IEnumerable<IActor> GetSensorList()
        {
            return actors.ToImmutableList<IActor>();
        }

        public IActor GetActorWithName(string name)
        {
            return actors.FirstOrDefault(s => s.Name == name);
        }

        private void onActorMeasureData(object sender, OnActorMeasureDataEventArgs args)
        {
            Notification.Send(Constants.ON_ACTOR_MEASURE_DATA, args);
        }

        private void onActorInfo(object sender, OnActorInfoEventArgs args)
        {
            helper.SendEvent(new
            {
                Sender = context.ClientIdentifier,
                Name = args.Actor.Name,
                Type = "Info",
                PriorityLevel = "Medium",
                Body = JsonConvert.SerializeObject(args.Data)
            });
        }

        public void SendMeasureData(OnActorMeasureDataEventArgs data)
        {
            helper.SendEvent(new 
            {
                Sender = context.ClientIdentifier,
                Name = data.Actor.Name,
                Type = "MeasureData",
                PriorityLevel = "Medium",
                Body = JsonConvert.SerializeObject(data.Data)
            });
            if (helper.SenderState == ConnectionState.Failed)
            {
                Notification.Send(Constants.ON_SEND_MEASURE_DATA_FAILED, data.Actor.Name);
            }
        }

     }
}
