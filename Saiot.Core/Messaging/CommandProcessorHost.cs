using Saiot.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Saiot.Core.Messaging
{

    #region Enums
    public enum CommandProcessorHostState { Unstarted = 0, Started, Stopped, Restarted, Killed};
    public enum CommandProcessorHostFeedback { Started, NothingToStop, AlreadyRunning, Continue, Stopped, Killed, AlreadyKilled, NotAllowed};
    #endregion

    #region Events
    public class ProcessorHostMessageEventArgs : EventArgs 
    {
        public BrokeredMessage Message {get;set;}
    }

    public class ProcessorHostErrorEventArs : EventArgs
    {
        public string ErrorMessage { get; set; }
    }

    public class ProcesserHostFeedbackEventArgs : EventArgs 
    {
        public CommandProcessorHostFeedback Feedback {get;set;}
    }


    public delegate void OnMessageReceived(object sender, ProcessorHostMessageEventArgs args);
    public delegate void OnMessageError(object sender, ProcessorHostErrorEventArs args);
    public delegate void OnFeedback(object sender, ProcesserHostFeedbackEventArgs args);
    #endregion

    
    public class CommandProcessorHost
    {
        #region CommandProcessorHost
        public CommandProcessorHostState State { get; private set; }
        public event OnMessageReceived OnMessageReceivedHandler;
        public event OnMessageError OnMessageErrorHander;
        public event OnFeedback OnFeedbackHandler;
        public MessagingContext Context { get; private set; }

        private CancellationTokenSource cts;
        private QueueHelper helper;
        private StateHandler stateHandler;
        private IProcessorState currentState;
        private IProcessorState unstartedState;
        private IProcessorState startedState;
        private IProcessorState stoppedState;
        private IProcessorState restartedState;
        private IProcessorState killedState;

        public CommandProcessorHost(MessagingContext context)
        {
            Context = context;
            stateHandler = new StateHandler(this);
            unstartedState = new UnstartedState(stateHandler, this);
            startedState = new StartedState(stateHandler, this);
            stoppedState = new StoppedState(stateHandler, this);
            restartedState = new RestartedState(stateHandler, this);
            killedState = new KilledState(stateHandler, this);
            currentState = unstartedState;
            State = CommandProcessorHostState.Unstarted;
            SetupForStart();
        }

        public void Start()
        {
            currentState.Start();
        }

        public void Stop()
        {
            currentState.Stop();
        }

        public void Kill()
        {
            currentState.Kill();
        }

        private void dispatchFeedback(CommandProcessorHostFeedback feedback)
        {
            if(OnFeedbackHandler != null)
            {
                OnFeedbackHandler.Invoke(this, new ProcesserHostFeedbackEventArgs{ Feedback = feedback});
            }
        }

        private void dispatchError(string errorMessage)
        {
            if (OnMessageErrorHander != null)
            {
                OnMessageErrorHander.Invoke(this, new ProcessorHostErrorEventArs { ErrorMessage = errorMessage });
            }
        }

        protected virtual void SetupForStart()
        {
            cts = new CancellationTokenSource();
            helper = new QueueHelper(Context);
        }


        protected virtual void ReceiveMessagesAsync()
        {

            try
            {
                helper.Client.OnMessage((msg) =>
                {
                    try
                    {
                        if (OnMessageReceivedHandler != null)
                        {
                            OnMessageReceivedHandler.Invoke(this, new ProcessorHostMessageEventArgs { Message = msg });
                        }
                    }
                    catch
                    {

                    }
                });
            }
            catch
            {
                dispatchError("Cannot receive messages from queue");
            }
        }

        protected virtual void CancelReceiving()
        {
            cts.Cancel();
            helper.Client.Close();
        }

        protected virtual void CleanupRessources()
        {
            helper.Client.Close();
        }

        protected virtual void DoNothing()
        {

        }

        private void setUnstartedState()
        {
            currentState = unstartedState;
            State = CommandProcessorHostState.Unstarted;
        }

        private void setStartedState()
        {
            currentState = startedState;
            State = CommandProcessorHostState.Started;
        }

        private void setStoppedState()
        {
            currentState = stoppedState;
            State = CommandProcessorHostState.Stopped;
        }

        private void setRestartedState()
        {
            currentState = restartedState;
            State = CommandProcessorHostState.Restarted;
        }

        private void setKilledState()
        {
            currentState = killedState;
            State = CommandProcessorHostState.Killed;
        }
        #endregion


        #region StateProcessor
        private class StateHandler
        {

            public StateHandler(CommandProcessorHost host)
            {
                this.host = host;
            }

            private CommandProcessorHost host;
            public void started()
            {
                host.SetupForStart();
                host.ReceiveMessagesAsync();
                host.dispatchFeedback(CommandProcessorHostFeedback.Started);
            }

            public void nothingToStop()
            {
                host.DoNothing();
                host.dispatchFeedback(CommandProcessorHostFeedback.NothingToStop);
            }

            public void stopped()
            {
                host.CancelReceiving();
                host.dispatchFeedback(CommandProcessorHostFeedback.Stopped);
            }


            public void alreadyRunning()
            {
                host.DoNothing();
                host.dispatchFeedback(CommandProcessorHostFeedback.AlreadyRunning);
            }

            public void conitnue()
            {
                host.SetupForStart();
                host.ReceiveMessagesAsync();
                host.dispatchFeedback(CommandProcessorHostFeedback.Continue);
            }

            public void killed()
            {
                host.CancelReceiving();
                host.CleanupRessources();
                host.dispatchFeedback(CommandProcessorHostFeedback.Killed);
            }

            public void alreadyKilled()
            {
                host.DoNothing();
                host.dispatchFeedback(CommandProcessorHostFeedback.AlreadyKilled);
            }

            public void notAllowed()
            {
                host.DoNothing();
                host.dispatchFeedback(CommandProcessorHostFeedback.NotAllowed);
            }
        }
        #endregion

        #region States
        private interface IProcessorState
        {
            void Start();
            void Stop();
            void Kill();
        }

        private class UnstartedState : IProcessorState
        {
            private StateHandler handler;
            private CommandProcessorHost host;
            public UnstartedState(StateHandler handler, CommandProcessorHost host)
            {
                this.handler = handler;
                this.host = host;
            }

            public void Start()
            {
                handler.started();
                host.setStartedState();
            }

            public void Stop()
            {
                handler.nothingToStop();
                host.setUnstartedState();
            }


            public void Kill()
            {
                handler.killed();
                host.setKilledState();
            }
        }

        private class StartedState : IProcessorState
        {
            private StateHandler handler;
            private CommandProcessorHost host;
            public StartedState(StateHandler handler, CommandProcessorHost host)
            {
                this.handler = handler;
                this.host = host;
            }

            public void Start()
            {
                handler.alreadyRunning();
                host.setStartedState();
            }

            public void Stop()
            {
                handler.stopped();
                host.setStoppedState();
            }


            public void Kill()
            {
                handler.killed();
                host.setKilledState();
            }
        }

        private class StoppedState : IProcessorState
        {
            private StateHandler handler;
            private CommandProcessorHost host;
            public StoppedState(StateHandler handler, CommandProcessorHost host)
            {
                this.handler = handler;
                this.host = host;
            }

            public void Start()
            {
                handler.conitnue();
                host.setRestartedState();
            }

            public void Stop()
            {
                handler.nothingToStop();
                host.setStoppedState();
            }

            public void Kill()
            {
                handler.killed();
                host.setKilledState();
            }
        }

        private class RestartedState : IProcessorState
        {
            private StateHandler handler;
            private CommandProcessorHost host;
            public RestartedState(StateHandler handler, CommandProcessorHost host)
            {
                this.handler = handler;
                this.host = host;
            }

            public void Start()
            {
                handler.alreadyRunning();
                host.setRestartedState();
            }

            public void Stop()
            {
                handler.stopped();
                host.setStoppedState();
            }

            public void Kill()
            {
                handler.killed();
                host.setKilledState();
            }
        }

        private class KilledState : IProcessorState
        {
            private StateHandler handler;
            private CommandProcessorHost host;
            public KilledState(StateHandler handler, CommandProcessorHost host)
            {
                this.handler = handler;
                this.host = host;
            }

            public void Start()
            {
                handler.notAllowed();
                host.setKilledState();
            }

            public void Stop()
            {
                handler.notAllowed();
                host.setKilledState();
            }

            public void Kill()
            {
                handler.alreadyKilled();
                host.setKilledState();
            }
        }
        #endregion
    }
}
