﻿// ---------------------------------------------------------
// AlfredCoreSubsystem.cs
// 
// Created on:      08/22/2015 at 10:47 PM
// Last Modified:   08/29/2015 at 12:28 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Subsystems
{
    /// <summary>
    ///     The core subsystem provides essential monitoring and control functionality for Alfred such as
    ///     the Alfred control
    ///     page, an event log page, etc. as well as monitoring of the current time and date.
    /// </summary>
    /// <remarks>
    ///     TODO: Once Alfred has a calendar subsystem, the time / date functionality may need to move
    ///     there
    /// </remarks>
    public sealed class AlfredCoreSubsystem : AlfredSubsystem
    {
        [NotNull]
        private readonly AlfredModuleListPage _controlPage;

        [NotNull]
        private readonly AlfredPagesListModule _pagesModule;

        [NotNull]
        private readonly AlfredPowerModule _powerModule;

        [NotNull]
        private readonly AlfredSubsystemListModule _systemsModule;

        [NotNull]
        private readonly AlfredTimeModule _timeModule;

        [CanBeNull]
        private AlfredEventLogPage _eventLogPage;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        public AlfredCoreSubsystem([NotNull] IObjectContainer container) : base(container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            _controlPage = new AlfredModuleListPage(container, ControlPageName, "Core");

            // Instantiate the modules
            _powerModule = new AlfredPowerModule(container);
            _timeModule = new AlfredTimeModule(container);
            _systemsModule = new AlfredSubsystemListModule(container);
            _pagesModule = new AlfredPagesListModule(container);
        }

        /// <summary>
        ///     Gets the name of the control page.
        /// </summary>
        /// <value>The name of the control page.</value>
        [NotNull]
        public static string ControlPageName
        {
            get { return "Alfred Core"; }
        }

        /// <summary>
        ///     Gets the name of the event log page.
        /// </summary>
        /// <value>The name of the event log page.</value>
        [NotNull]
        public static string EventLogPageName
        {
            get { return "Event Log"; }
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public override string Name
        {
            get { return Resources.AlfredControlSubSystem_Name.NonNull(); }
        }

        /// <summary>
        ///     Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        public override string Id
        {
            get { return "Core"; }
        }

        /// <summary>
        ///     Registers the controls for this component.
        /// </summary>
        protected override void RegisterControls()
        {
            Register(_controlPage);

            // Build out our control page
            _controlPage.ClearModules();
            _controlPage.Register(_powerModule);
            _controlPage.Register(_timeModule);
            _controlPage.Register(_systemsModule);
            _controlPage.Register(_pagesModule);

            // Don't include the event log page if there are no events
            if (AlfredInstance?.Console != null)
            {
                _eventLogPage = new AlfredEventLogPage(Container,
                                                       AlfredInstance.Console,
                                                       EventLogPageName);
                Register(_eventLogPage);
            }
        }
    }
}