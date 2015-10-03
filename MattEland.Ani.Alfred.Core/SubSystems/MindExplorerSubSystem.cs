// ---------------------------------------------------------
// MindExplorerSubsystem.cs
// 
// Created on:      09/03/2015 at 11:00 PM
// Last Modified:   09/09/2015 at 6:45 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Common.Providers;
using System.Diagnostics.Contracts;

namespace MattEland.Ani.Alfred.Core.Subsystems
{
    /// <summary>
    ///     A <see cref="AlfredSubsystem" /> that pokes around at the <see langword="internal"/>
    ///     state of each of Alfred's SubSystems and their sub-components. This Subsystem has a
    ///     particular focus towards anything AI-related.
    /// </summary>
    public sealed class MindExplorerSubsystem : AlfredSubsystem
    {
        /// <summary>
        ///     The search provider.
        /// </summary>
        [NotNull]
        private ExplorerSearchProvider _searchProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public MindExplorerSubsystem([NotNull] IAlfredContainer container)
            : this(container, true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="includeExplorerPage">
        ///      <see langword="true" /> to include the page in root pages, <see langword="false" /> otherwise.
        /// </param>
        public MindExplorerSubsystem([NotNull] IAlfredContainer container, bool includeExplorerPage)
            : base(container)
        {
            MindExplorerPage = new ExplorerPage(container, "Mind Explorer", "MindMap");

            if (!includeExplorerPage) { MindExplorerPage.IsRootLevel = false; }

            _searchProvider = new ExplorerSearchProvider(container, this);
        }

        /// <summary>
        ///     Gets the Mind Explorer page.
        /// </summary>
        /// <remarks>
        ///     This is intended primarily for testing
        /// </remarks>
        /// <value>
        /// The Mind Explorer page.
        /// </value>
        [NotNull]
        public ExplorerPage MindExplorerPage
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        public override string Name
        {
            get { return "Mind Explorer"; }
        }

        /// <summary>
        ///     Gets the search providers.
        /// </summary>
        /// <value>
        ///     The search providers.
        /// </value>
        public override IEnumerable<ISearchProvider> SearchProviders
        {
            get { yield return _searchProvider; }
        }

        /// <summary>
        ///     Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>
        /// The identifier for the subsystem.
        /// </value>
        public override string Id
        {
            get { return "Mind"; }
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="alfred" /> is <see langword="null" /> .
        /// </exception>
        protected override void InitializeProtected(IAlfred alfred)
        {
            Contract.Requires<ArgumentNullException>(alfred != null);

            // Add alfred to the collection as the root level will only contain Alfred.
            MindExplorerPage.ClearNodes();
            MindExplorerPage.AddRootNode(alfred);
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