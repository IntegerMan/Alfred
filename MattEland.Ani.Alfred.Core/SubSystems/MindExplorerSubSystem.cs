// ---------------------------------------------------------
// MindExplorerSubsystem.cs
// 
// Created on:      08/22/2015 at 10:48 PM
// Last Modified:   08/22/2015 at 11:15 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;

namespace MattEland.Ani.Alfred.Core.SubSystems
{
    /// <summary>
    ///     A SubSystem that pokes around at the internal state of each of Alfred's SubSystems and their
    ///     sub-components. This SubSystem has a particular focus towards anything AI-related.
    /// </summary>
    public class MindExplorerSubsystem : AlfredSubsystem
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="console">The console.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public MindExplorerSubsystem([NotNull] IPlatformProvider provider,
                                     [CanBeNull] IConsole console = null) : base(provider, console)
        {
            MindExplorerPage = new ExplorerPage(provider, "Mind Explorer", "MindMap");
        }

        /// <summary>
        /// Handles initialization events
        /// </summary>
        /// <param name="alfred">The alfred instance.</param>
        protected override void InitializeProtected(IAlfred alfred)
        {

            // Build out a new collection using the platform provider
            var nodes = alfred.PlatformProvider.CreateCollection<IPropertyProvider>();

            // Add alfred to the collection as the root level will only contain Alfred.
            nodes.Add(alfred);

            // Set the page's root nodes property so the UI can display things.
            MindExplorerPage.RootNodes = nodes;
        }

        /// <summary>
        ///     Gets the Mind Explorer page.
        /// </summary>
        /// <remarks>
        ///     This is intended primarily for testing
        /// </remarks>
        /// <value>The Mind Explorer page.</value>
        [NotNull]
        public ExplorerPage MindExplorerPage
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        public override string Name
        {
            get { return "Mind Explorer"; }
        }

        /// <summary>
        ///     Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        public override string Id
        {
            get { return "Mind"; }
        }

        /// <summary>
        ///     Allows components to define controls
        /// </summary>
        protected override void RegisterControls()
        {
            base.RegisterControls();

            Register(MindExplorerPage);
        }
    }
}