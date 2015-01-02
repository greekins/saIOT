
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Saiot.DeviceController.Model
{

    public class OnActorMeasureDataEventArgs : EventArgs
    {
        public IActor Actor { get; set; }
        public object Data { get; set; }
    }

    public class OnActorInfoEventArgs : EventArgs
    {
        public IActor Actor { get; set; }
        public object Data { get; set; }
    }


    public delegate void OnActorMeasureData(object sender, OnActorMeasureDataEventArgs args);
    public delegate void OnActorInfo(object sender, OnActorInfoEventArgs args);

    public interface IActor
    {
        event OnActorMeasureData OnActorMeasureDataHandler;
        event OnActorInfo OnActorInfoHandler;
        string Name { get; }
        string Type { get;}
        string Location { get; set;}
        Dictionary<string,string> Config { get; set; }
        void SetScalarConfig(Dictionary<string, string> config);
    }
}
