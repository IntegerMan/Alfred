﻿// ---------------------------------------------------------
// AlfredCoreSubsystem.cs
// 
// Created on:      09/01/2015 at 3:46 PM
// Last Modified:   09/02/2015 at 5:41 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Common;
using System.Diagnostics.Contracts;
namespace MattEland.Ani.Alfred.Core.Subsystems
{
    /// <summary>
    ///     The core subsystem provides essential monitoring and control functionality for Alfred such
    ///     as the Alfred control page, an event log page, etc. as well as monitoring of the current time
    ///     and date.
    /// </summary>
    public sealed class AlfredCoreSubsystem : AlfredSubsystem
    {
        /// <summary>
        ///     The control page.
        /// </summary>
        [NotNull]
        private readonly ModuleListPage _controlPage;

        /// <summary>
        ///     The pages list module.
        /// </summary>
        [NotNull]
        private readonly AlfredPagesListModule _pagesModule;

        /// <summary>
        ///     The power module.
        /// </summary>
        [NotNull]
        private readonly AlfredPowerModule _powerModule;

        /// <summary>
        ///     The systems list module.
        /// </summary>
        [NotNull]
        private readonly AlfredSubsystemListModule _systemsModule;

        /// <summary>
        ///     The time module.
        /// </summary>
        [NotNull]
        private readonly AlfredTimeModule _timeModule;

        /// <summary>
        ///     The event log page.
        /// </summary>
        [CanBeNull]
        private EventLogPage _eventLogPage;

        /// <summary>
        ///     The search results page.
        /// </summary>
        [NotNull]
        private SearchPage _searchPage;

        /// <summary>
        ///     The search history page.
        /// </summary>
        [NotNull]
        private SearchHistoryPage _searchHistoryPage;

        /// <summary>
        ///     The web browser page.
        /// </summary>
        [NotNull]
        private WebBrowserPage _browserPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredCoreSubsystem" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public AlfredCoreSubsystem([NotNull] IAlfredContainer container) : this(container, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredCoreSubsystem" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="includeSearchModuleOnSearchPage">Whether or not to include the search module on the search page.</param>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        public AlfredCoreSubsystem([NotNull] IAlfredContainer container, bool includeSearchModuleOnSearchPage) : base(container)
        {
            //- Validate
            Contract.Requires(container != null, "container is null.");

            // Set up pages
            _controlPage = new ModuleListPage(container, ControlPageName, "Core");
            _searchPage = new SearchPage(container, includeSearchModuleOnSearchPage);
            _searchHistoryPage = new SearchHistoryPage(container);
            _browserPage = new WebBrowserPage(container);

            // Instantiate the modules
            _powerModule = new AlfredPowerModule(container);
            _timeModule = new AlfredTimeModule(container);
            _systemsModule = new AlfredSubsystemListModule(container);
            _pagesModule = new AlfredPagesListModule(container);
        }

        /// <summary>
        ///     Gets the name of the control page.
        /// </summary>
        /// <value>
        ///     The name of the control page.
        /// </value>
        [NotNull]
        public static string ControlPageName
        {
            get { return "Alfred Core"; }
        }

        /// <summary>
        ///     Gets the name of the event log page.
        /// </summary>
        /// <value>
        ///     The name of the event log page.
        /// </value>
        [NotNull]
        public static string EventLogPageName
        {
            get { return "Event Log"; }
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>
        ///     The name of the module.
        /// </value>
        public override string Name
        {
            get { return Resources.AlfredControlSubSystem_Name.NonNull(); }
        }

        /// <summary>
        ///     Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>
        ///     The identifier for the subsystem.
        /// </value>
        public override string Id
        {
            get { return "Core"; }
        }

        /// <summary>
        ///     Registers the controls for this component.
        /// </summary>
        protected override void RegisterControls()
        {
            // Register Pages
            Register(_controlPage);
            Register(_searchPage);
            Register(_searchHistoryPage);
            Register(_browserPage);

            // Build out our control page
            _controlPage.ClearModules();
            _controlPage.Register(_powerModule);
            _controlPage.Register(_timeModule);
            _controlPage.Register(_systemsModule);
            _controlPage.Register(_pagesModule);

            // Don't include the event log page if there are no events
            if (Container.HasMapping(typeof(IConsole)))
            {
                _eventLogPage = new EventLogPage(Container, EventLogPageName);
                Register(_eventLogPage);
            }
        }

        /// <summary>
        ///     Processes an Alfred Command. If the <paramref name="command"/> is handled,
        ///     <paramref name="result"/> should be modified accordingly and the method should return
        ///     true. Returning false will not stop the message from being propagated.
        /// </summary>
        /// <param name="command"> The command. </param>
        /// <param name="result"> The result. If the command was handled, this should be updated. </param>
        /// <returns>
        ///     <c>True</c> if the command was handled; otherwise false.
        /// </returns>
        public override bool ProcessAlfredCommand(ChatCommand command, [CanBeNull] ICommandResult result)
        {
            // Allow the default implementation to take a swing at things
            if (base.ProcessAlfredCommand(command, result))
            {
                return true;
            }

            // Ensure the command is for this module
            if (command.IsFor(this))
            {
                // Route search commands to the search controller
                if (command.Name.Matches("Search") && command.Data.HasText())
                {
                    AlfredInstance.SearchController.PerformSearch(command.Data);
                }
            }

            // If we got here, the command wasn't handled
            return false;
        }
    }
}