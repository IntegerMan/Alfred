// ---------------------------------------------------------
// ChatCommandTests.cs
// 
// Created on:      08/17/2015 at 9:56 PM
// Last Modified:   08/18/2015 at 3:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using MattEland.Ani.Alfred.Core.Modules.SysMonitor;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    ///     Tests for commands embedded in AIML that impact the functioning of the Alfred system.
    /// </summary>
    [TestFixture]
    public class ChatCommandTests : ChatTestsBase
    {
        /// <summary>
        ///     Sets up the testing environment prior to each test run.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            InitChatSystem();
        }

        /// <summary>
        ///     Builds a list of fake counter names with a total instance included as well.
        /// </summary>
        /// <param name="count">The count of fake items to build.</param>
        /// <returns>A list of fake counters</returns>
        private IEnumerable<string> BuildInstanceNamesWithTotal(int count)
        {
            for (var i = 1; i <= count; i++)
            {
                yield return $"Item {i}";
            }

            yield return SystemMonitorModule.TotalInstanceName;
        }

        /// <summary>
        ///     Tests that the CPU Status command includes the expected CPU information.
        /// </summary>
        /// <remarks>
        ///     See ALF-5, ALF-27, and ALF-28
        /// </remarks>
        [Test]
        public void CpuStatusIsAccurate()
        {
            Alfred.Shutdown();
            MetricProviderFactory.DefaultValue = 42.0f;
            MetricProviderFactory.CategoryInstanceNames.Add(CpuMonitorModule.CpuCategoryName,
                                                            BuildInstanceNamesWithTotal(72));
            Alfred.Initialize();

            var reply = GetReply("CPU STATUS");

            string expected = $"There are 72 CPU cores with an average of 42.0 % utilization.";
            Assert.That(reply.Contains(expected),
                        $"Reply '{reply}' did not match expected value of {expected}.");
        }

        /// <summary>
        ///     Tests that the CPU Status command includes the expected CPU information.
        /// </summary>
        /// <remarks>
        ///     See ALF-5, ALF-27, and ALF-28
        /// </remarks>
        [Test]
        public void MemoryStatusIsAccurate()
        {
            Alfred.Shutdown();
            MetricProviderFactory.DefaultValue = 42.0f;
            Alfred.Initialize();

            var reply = GetReply("MEMORY STATUS");

            string expected = $"The system is currently utilizing 42.0 % of all available memory.";
            Assert.That(reply.Contains(expected),
                        $"Reply '{reply}' did not match expected value of {expected}.");
        }

        /// <summary>
        ///     Ensure that the shutdown command causes alfred to be offline.
        /// </summary>
        /// <remarks>
        ///     See ALF-21
        /// </remarks>
        [Test]
        public void ShutdownCausesAlfredToBeOffline()
        {
            Say("Shutdown");

            Assert.IsFalse(Alfred.IsOnline, "Alfred was not offline after shutdown was handled.");
        }

        /// <summary>
        ///     Tests that the Status command includes the expected basic system information.
        /// </summary>
        /// <remarks>
        ///     See ALF-27
        /// </remarks>
        [Test]
        public void StatusYieldsRelevantInformation()
        {
            var reply = GetReply("Status");
            var count = Alfred.Subsystems.Count();

            Assert.That(
                        reply.Contains(
                                       $"The System is Online with a total of {count} Subsystems Present."),
                        $"{reply} did not contain the expected system status text.");
        }

        [Test]
        public void TagHandlersAreInvokedWhenATestingTemplateIsInvoked()
        {
            Say("TEST TAG INVOKE");

            Assert.IsTrue(AlfredTestTagHandler.WasInvoked);
        }

        [Test]
        public void TagHandlersAreNotInvokedOnOtherInput()
        {
            Say("I like turtles");

            Assert.IsFalse(AlfredTestTagHandler.WasInvoked);
        }
    }
}