using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saiot.Core.Messaging;
using Saiot.Core.Test.Fakes;

namespace Saiot.Core.Test
{
    [TestClass]
    public class CommandProcessorHostTest
    {

        private CommandProcessorHostMock processorHost;
        private CommandProcessorHostFeedback feedback;


        public void OnFeedback(object sender, ProcesserHostFeedbackEventArgs args)
        {
            feedback = args.Feedback;
        } 

        [TestInitialize]
        public void Initialize()
        {
            processorHost = new CommandProcessorHostMock();
            processorHost.OnFeedbackHandler += OnFeedback;
        }

        [TestMethod]
        public void Test_Unstarted_Start()
        {
            Assert.AreEqual(CommandProcessorHostState.Unstarted, processorHost.State);
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Started, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.Started, feedback);
            processorHost.AssertStarted();
        }

        [TestMethod]
        public void Test_Unstarted_Stop()
        {
            Assert.AreEqual(CommandProcessorHostState.Unstarted, processorHost.State);
            processorHost.Stop();
            Assert.AreEqual(CommandProcessorHostState.Unstarted, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.NothingToStop, feedback);
            processorHost.AssertDidNothing();
        }

        [TestMethod]
        public void Test_Unstarted_Kill()
        {
            Assert.AreEqual(CommandProcessorHostState.Unstarted, processorHost.State);
            processorHost.Kill();
            Assert.AreEqual(CommandProcessorHostState.Killed, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.Killed, feedback);
            processorHost.AssertKilled();
        }

        [TestMethod]
        public void Test_Started_Start()
        {
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Started, processorHost.State);
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Started, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.AlreadyRunning, feedback);
            processorHost.AssertDidNothing();
        }

        [TestMethod]
        public void Test_Started_Stop()
        {
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Started, processorHost.State);
            processorHost.Stop();
            Assert.AreEqual(CommandProcessorHostState.Stopped, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.Stopped, feedback);
            processorHost.AssertStopped();
        }

        [TestMethod]
        public void Test_Started_Kill()
        {
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Started, processorHost.State);
            processorHost.Kill();
            Assert.AreEqual(CommandProcessorHostState.Killed, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.Killed, feedback);
            processorHost.AssertKilled();
        }

        [TestMethod]
        public void Test_Stopped_Start()
        {
            processorHost.Start();
            processorHost.Stop();
            Assert.AreEqual(CommandProcessorHostState.Stopped, processorHost.State);
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Restarted, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.Continue, feedback);
            processorHost.AssertStarted();
        }

        [TestMethod]
        public void Test_Stopped_Stop()
        {
            processorHost.Start();
            processorHost.Stop();
            Assert.AreEqual(CommandProcessorHostState.Stopped, processorHost.State);
            processorHost.Stop();
            Assert.AreEqual(CommandProcessorHostState.Stopped, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.NothingToStop, feedback);
            processorHost.AssertDidNothing();
        }

        [TestMethod]
        public void Test_Stopped_Kill()
        {
            processorHost.Start();
            processorHost.Stop();
            Assert.AreEqual(CommandProcessorHostState.Stopped, processorHost.State);
            processorHost.Kill();
            Assert.AreEqual(CommandProcessorHostState.Killed, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.Killed, feedback);
            processorHost.AssertKilled();
        }

        [TestMethod]
        public void Test_Restarted_Start()
        {
            processorHost.Start();
            processorHost.Stop();
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Restarted, processorHost.State);
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Restarted, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.AlreadyRunning, feedback);
            processorHost.AssertDidNothing();
        }

        [TestMethod]
        public void Test_Restarted_Stop()
        {
            processorHost.Start();
            processorHost.Stop();
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Restarted, processorHost.State);
            processorHost.Stop();
            Assert.AreEqual(CommandProcessorHostState.Stopped, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.Stopped, feedback);
            processorHost.AssertStopped();
        }

        [TestMethod]
        public void Test_Restarted_Kill()
        {
            processorHost.Start();
            processorHost.Stop();
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Restarted, processorHost.State);
            processorHost.Kill();
            Assert.AreEqual(CommandProcessorHostState.Killed, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.Killed, feedback);
            processorHost.AssertKilled();
        }

        [TestMethod]
        public void Test_Killed_Start()
        {
            processorHost.Kill();
            Assert.AreEqual(CommandProcessorHostState.Killed, processorHost.State);
            processorHost.Start();
            Assert.AreEqual(CommandProcessorHostState.Killed, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.NotAllowed, feedback);
            processorHost.AssertDidNothing();
        }

        [TestMethod]
        public void Test_Killed_Stop()
        {
            processorHost.Kill();
            Assert.AreEqual(CommandProcessorHostState.Killed, processorHost.State);
            processorHost.Stop();
            Assert.AreEqual(CommandProcessorHostState.Killed, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.NotAllowed, feedback);
            processorHost.AssertDidNothing();
        }

        [TestMethod]
        public void Test_Killed_Kill()
        {
            processorHost.Kill();
            Assert.AreEqual(CommandProcessorHostState.Killed, processorHost.State);
            processorHost.Kill();
            Assert.AreEqual(CommandProcessorHostState.Killed, processorHost.State);
            Assert.AreEqual(CommandProcessorHostFeedback.AlreadyKilled, feedback);
            processorHost.AssertDidNothing();
        }
    }
}
