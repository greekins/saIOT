using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.DeviceController.Application
{
    public class Constants
    {
        public const string ON_REMOTE_COMMAND_RECEIVE_SUCCESS = "onRemoteCommandReceiveSuccess";
        public const string ON_REMOTE_COMMAND_RECEIVE_ERROR = "onRemoteCommandReceiveError";
        public const string ON_REMOTE_COMMAND_NOT_FOUND = "onRemoteCommandProcessingError";
        public const string ON_INPUT_COMMAND_ENTERED = "onInputCommandEntered";
        public const string ON_COMMAND_MODEL_FEEDBACK = "onCommandModelFeedback";
        public const string ON_COMMAND_NOT_FOUND = "onCommandNotFound";
        public const string LOG = "log";
        public const string LOG_INFO = "logInfo";
        public const string LOG_ERROR = "logError";
        public const string LOG_SUCCESS = "logSuccess";
        public const string ON_AUTH_SUCCESS = "onAuthSuccess";
        public const string ON_AUTH_ERROR = "onAuthError";
        public const string ON_ACTOR_MEASURE_DATA = "onActorMeasureData";
        public const string ON_SEND_MEASURE_DATA_FAILED = "onSendMeasureDataFailed";
        public const string ON_ACTOR_REGISTERED = "onActorRegistered";
        public const string ON_ACTOR_UNREGISTERED = "onActorUnregistered";
        public const string ON_ACTOR_CONFIG_UPDATED = "onActorConfigUpdated";
        public const string ON_ACTOR_CONFIG_LOAD_SUCCESS = "onActorConfigLoadSuccess";
        public const string ON_ACTOR_LOCATION_CHANGED = "onActorLocationChanged";
    }
}
