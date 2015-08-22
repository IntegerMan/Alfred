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

using JetBrains.Annotations;

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
        [NotNull, ItemNotNull]
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
            MetricProviderFactory.CategoryInstanceNames.Add(CpuMonitorModule.ProcessorCategoryName,
                                                            BuildInstanceNamesWithTotal(72));
            Alfred.Initialize();

            var reply = GetReply("CPU STATUS");

            var expected = "There are 72 CPU cores with an average of 42.0 % utilization.";
            Assert.That(reply.Contains(expected),
                        $"Reply '{reply}' did not match expected value of {expected}.");
        }

        /// <summary>
        ///     Tests that the Memory Status command includes the expected Memory information.
        /// </summary>
        /// <remarks>
        ///     See ALF-5, ALF-27, and ALF-28
        /// </remarks>
        [Test]
        public void MemoryStatusIsAccurate()
        {
            MetricProviderFactory.DefaultValue = 42.0f;

            var reply = GetReply("MEMORY STATUS");

            var expected = "The system is currently utilizing 42.0 % of all available memory.";
            Assert.That(reply.Contains(expected),
                        $"Reply '{reply}' did not match expected value of {expected}.");
        }

        /// <summary>
        ///     Tests that the Disk Status command includes the expected Disk information.
        /// </summary>
        /// <remarks>
        ///     See ALF-5, ALF-27, and ALF-28
        /// </remarks>
        [Test]
        public void DiskStatusIsAccurate()
        {
            MetricProviderFactory.DefaultValue = 42.0f;

            var reply = GetReply("DISK STATUS");

            var expected = "Disk read speed is currently utilized at 42.0 % and disk write utilization is at 42.0 %.";
            Assert.That(reply.Contains(expected),
                        $"Reply '{reply}' did not match expected value of {expected}.");
        }

        /// <summary>
        ///     Tests that the Alfred Status command includes the expected information.
        /// </summary>
        /// <remarks>
        ///     See ALF-5
        /// </remarks>
        [Test]
        public void AlfredStatusIsAccurate()
        {
            var reply = GetReply("ALFRED STATUS");
            var count = Alfred.Subsystems.Count();

            Assert.That(reply.Contains($"The system is online with a total of {count} Subsystems Present."),
                        $"'{reply}' did not contain the expected system status text.");
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
        ///     Tests that the generic Status command includes the expected system information from various counters.
        /// </summary>
        /// <remarks>
        ///     See ALF-5, ALF-27, and ALF-28
        /// </remarks>
        [Test]
        public void StatusYieldsRelevantInformation()
        {
            var reply = GetReply("Status");

            Assert.That(reply.Contains("system is online"), $"System online status was missing from '{reply}'");
            Assert.That(reply.Contains("Disk read speed"), $"Disk status was missing from '{reply}'");
            Assert.That(reply.Contains("available memory"), $"Memory status was missing from '{reply}'");
            Assert.That(reply.Contains("CPU core"), $"CPU status was missing from '{reply}'");
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