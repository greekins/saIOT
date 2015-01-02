using Saiot.Core.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Core.Test.Fakes
{
    public class CommandProcessorHostMock : CommandProcessorHost
    {
        private static int SETUP_FOR_START = 0;
        private static int RECEIVE_MESSAGE_ASYNC = 1;
        private static int CANCEL_RECEIVING = 2;
        private static int CLEANUP_RESSOURCES = 3;
        private static int DO_NOTHING = 4;
        private Stack<int> calls = new Stack<int>();

        public CommandProcessorHostMock(): base(null) {}

        protected override void SetupForStart()
        {
            calls.Push(SETUP_FOR_START);
        }

        protected override async void ReceiveMessagesAsync()
        {
            calls.Push(RECEIVE_MESSAGE_ASYNC);
            await Task.Run(() => { });
        }

        protected override void CancelReceiving()
        {
            calls.Push(CANCEL_RECEIVING);
        }

        protected override void CleanupRessources()
        {
            calls.Push(CLEANUP_RESSOURCES);
        }

        protected override void DoNothing()
        {
            calls.Push(DO_NOTHING);
        }

        public void AssertStarted()
        {
            Assert.AreEqual(RECEIVE_MESSAGE_ASYNC, calls.Pop());
            Assert.AreEqual(SETUP_FOR_START, calls.Pop());
        }

        public void AssertDidNothing()
        {
            Assert.AreEqual(DO_NOTHING, calls.Pop());
        }

        public void AssertStopped()
        {
            Assert.AreEqual(CANCEL_RECEIVING, calls.Pop());
        }

        public void AssertKilled()
        {
            Assert.AreEqual(CLEANUP_RESSOURCES, calls.Pop());
            Assert.AreEqual(CANCEL_RECEIVING, calls.Pop());
        }
    }
}
