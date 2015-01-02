using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Saiot.DeviceController.Controller;
using Newtonsoft.Json;

namespace Saiot.DeviceController.Model
{
    public class SprenklerActor : IActor
    {
        public event OnActorMeasureData OnActorMeasureDataHandler;
        public event OnActorInfo OnActorInfoHandler;

        public string Name { get; private set; }
        public string Location { get; set; }
        public string Type { get { return "sprenkler"; } }

        private Dictionary<string, string> config;
        private CancellationTokenSource cts;
        private Dictionary<string, int> scalarValues;
       
        public SprenklerActor(string name)
        {
            Name = name;
            Location = "Hecke Nord";
            scalarValues =  new Dictionary<string,int>();
            scalarValues["interval"] = 5000;
            scalarValues["radius"] = 10;
            scalarValues["threshold"] = 10;
        }

     
        public Dictionary<string, string> Config
        {
            get { return config; }
            set
            {
                config = value;

                var errors = string.Empty;
                
                checkConfigCompleteness(errors);
                updateMode(errors);
                if (errors != string.Empty) throw new Exception(errors);

                updateScalarValue(errors, "interval");
                updateScalarValue(errors, "radius");
                updateScalarValue(errors, "threshold");

                if (errors != string.Empty) throw new Exception(errors);
            }
        }


        private void checkConfigCompleteness(string errors)
        {
            checkConfigParameterExists(errors, "mode");
            checkConfigParameterExists(errors, "radius");
            checkConfigParameterExists(errors, "interval");
            checkConfigParameterExists(errors, "threshold");
        }

        private void checkConfigParameterExists(string errors, string parameterName)
        {
            if (!config.ContainsKey(parameterName)) appendArgumentError(errors, parameterName, "null");
        }

        private void updateMode(string errors)
        {
            if (config["mode"] == "on") switchOn();
            else if (config["mode"] == "off") switchOff();
            else appendArgumentError(errors, "mode", config["mode"]);
        }

        private void updateScalarValue(string errors, string property)
        {
            try
            {
                int oldValue = scalarValues[property];
                int newValue = int.Parse(config[property]);
                if (oldValue != newValue)
                {
                    notifyInfo(string.Format("Changed {0} from {1} to {2}", property, oldValue, newValue));
                }
                scalarValues[property] = newValue;
            }
            catch
            {
                appendArgumentError(errors, property, config[property]);
            }
        }

        private void appendArgumentError(string errors, string argumentName, string value)
        {
            errors += string.Format("Invalid argument value: [{0} = {1}]", argumentName, value);
        }

        private void switchOff()
        {
            stopGeneratingEvents();
            notifyInfo("Switched off");
        }

        private void switchOn()
        {
            startGeneratingEvents();
            notifyInfo("Switched on");
        }

        private void startGeneratingEvents()
        {
            cts = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    var value = new Random().Next(20, 80);
                    if (OnActorMeasureDataHandler != null)
                    {
                        
                        OnActorMeasureDataHandler.Invoke(this, new OnActorMeasureDataEventArgs { Actor = this, Data = new {
                            Moisture = value
                        } });
                    }
                    if (value > scalarValues["threshold"])
                    {
                        notifyInfo(string.Format("Humiditiy is over {0}%, turn sprinkler off.", scalarValues["threshold"]));
                    }
                    else
                    {
                        notifyInfo(string.Format("Humiditiy is below {0}%, turn sprinkler on.", scalarValues["threshold"]));
                    }
                    Task.Delay(scalarValues["interval"]).Wait();
                }
            });
        }


        private void notifyInfo(object info)
        {
            if (OnActorInfoHandler != null)
            {
                OnActorInfoHandler.Invoke(this, new OnActorInfoEventArgs { Actor = this, Data = info });
            }
        }


        private void stopGeneratingEvents()
        {
            if (cts != null) cts.Cancel();
        }





        public void SetScalarConfig(Dictionary<string, string> config)
        {
            foreach (var key in config.Keys)
            {
                try
                {
                    scalarValues[key] = int.Parse(config[key]); 
                }
                catch { }
            }
        }
    }
}
