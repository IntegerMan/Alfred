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
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.SubSystems;
using MattEland.Ani.Alfred.PresentationShared.Commands;
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

            _provider = new SimplePlatformProvider();
            _subsystem = new MindExplorerSubsystem(_provider);

            _page = _subsystem.MindExplorerPage;
        }

        [NotNull]
        private MindExplorerSubsystem _subsystem;

        [NotNull]
        private AlfredApplication _alfred;

        [NotNull]
        private ExplorerPage _page;

        [NotNull]
        private SimplePlatformProvider _provider;

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

        /// <summary>
        /// Tests that the Mind Explorer's Mind Explorer page must be of Mind Explorer Page type.
        /// </summary>
        /// <remarks>
        /// See ALF-15
        /// </remarks>
        [Test]
        public void MindExplorerPageIsMindExplorerPageType()
        {
            Assert.AreEqual(typeof(ExplorerPage), _page.GetType());
        }


        /// <summary>
        /// Tests that the Mind Explorer's Mind Explorer page must be visible.
        /// </summary>
        /// <remarks>
        /// See ALF-15
        /// </remarks>
        [Test]
        public void MindExplorerPageIsVisible()
        {
            Assert.That(_page.IsVisible);
        }

        /// <summary>
        /// Tests that standard applications contains the mind explorer subsystem.
        /// </summary>
        /// <remarks>
        /// See ALF-15
        /// </remarks>
        [Test]
        public void ApplicationContainsMindExplorer()
        {
            var app = new ApplicationManager(_provider);
            Assert.That(app.Alfred.Subsystems.Any(s => s is MindExplorerSubsystem), "The Mind Explorer subsystem is not part of a typical Alfred application");
        }

        /// <summary>
        /// Ensures that the explorer subsystem is in of itself an <see cref="IPropertyProvider"/>
        /// </summary>
        /// <remarks>
        /// See ALF-15
        /// </remarks>
        [Test]
        public void SubsystemIsPropertyProvider()
        {
            var propItem = _subsystem as IPropertyProvider;
            Assert.IsNotNull(propItem);
        }

        /// <summary>
        /// Ensures that the explorer subsystem provides properties when GetProperties is invoked.
        /// </summary>
        /// <remarks>
        /// See ALF-15
        /// </remarks>
        [Test]
        [SuppressMessage("ReSharper", "UseMethodAny.1")]
        public void SubsystemHasProperties()
        {
            var propItem = _subsystem as IPropertyProvider;
            var properties = propItem.Properties;

            Assert.IsNotNull(properties);
            Assert.That(properties.Count() >= 1);
        }

        /// <summary>
        /// Checks that the subsystem has items in its root nodes collection
        /// </summary>
        /// <remarks>
        /// See ALF-15
        /// </remarks>
        [Test]
        public void SubsystemPageHasNodes()
        {
            _alfred.Register(_subsystem);
            _alfred.Initialize();

            Assert.That(_page.RootNodes.Any());
        }
    }
}