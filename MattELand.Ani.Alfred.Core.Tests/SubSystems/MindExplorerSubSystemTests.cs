// ---------------------------------------------------------
// MindExplorerSubsystemTests.cs
// 
// Created on:      08/22/2015 at 10:42 PM
// Last Modified:   08/22/2015 at 11:06 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.SubSystems;
using MattEland.Common;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Subsystems
{
    /// <summary>
    ///     Contains tests related to the <see cref="MindExplorerSubsystem" />.
    /// </summary>
    /// <remarks>
    ///     See ALF-15
    /// </remarks>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class MindExplorerSubsystemTests
    {

        /// <summary>
        ///     Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var bootstrapper = new AlfredBootstrapper();
            _alfred = bootstrapper.Create();

            _subsystem = new MindExplorerSubsystem(new SimplePlatformProvider());
        }

        [NotNull]
        private MindExplorerSubsystem _subsystem;

        [NotNull]
        private AlfredApplication _alfred;

        /// <summary>
        ///     Ensures that the mind explorer subsystem can be instantiated
        /// </summary>
        /// <remarks>
        ///     See ALF-15
        /// </remarks>
        [Test]
        public void MindExplorerCanBeInitialized()
        {
            Assert.That(_subsystem.Name.HasText());
        }

        /// <summary>
        ///     Ensures that the Mind Explorer can be registered
        /// </summary>
        /// <remarks>
        ///     See ALF-15
        /// </remarks>
        [Test]
        public void MindExplorerCanBeRegistered()
        {
            _alfred.Register(_subsystem);

            Assert.Contains(_subsystem, (ICollection)_alfred.Subsystems);
        }

        /// <summary>
        ///     Asserts that the Mind Explorer must register a single Mind Map page.
        /// </summary>
        /// <remarks>
        ///     See ALF-15
        /// </remarks>
        [Test]
        public void MindExplorerHasMindMapPage()
        {
            var alfredPages = _alfred.RootPages.Count();

            _alfred.Register(_subsystem);

            Assert.AreEqual(1,
                            _subsystem.RootPages.Count(),
                            "Subsystem did not have the expected page count.");

            Assert.AreEqual(alfredPages + 1,
                            _alfred.RootPages.Count(),
                            "Alfred did not have the expected page count.");
        }
    }
}