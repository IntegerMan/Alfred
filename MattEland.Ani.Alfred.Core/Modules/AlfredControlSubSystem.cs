// ---------------------------------------------------------
// AlfredControlSubsystem.cs
// 
// Created on:      08/08/2015 at 6:12 PM
// Last Modified:   08/08/2015 at 6:58 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Interfaces;
using MattEland.Ani.Alfred.Core.Pages;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     The control subsystem provides essential monitoring and control functionality for Alfred such as the Alfred control
    ///     page, an event log page, etc.
    /// </summary>
    public class AlfredControlSubsystem : AlfredSubsystem
    {
        [NotNull]
        private readonly AlfredModuleListPage _controlPage;

        [CanBeNull]
        private AlfredEventLogPage _eventLogPage;

        [NotNull]
        private readonly AlfredPowerModule _powerModule;

        [NotNull]
        private readonly AlfredTimeModule _timeModule;

        [NotNull]
        private readonly AlfredSubsystemListModule _systemsModule;

        [NotNull]
        private readonly AlfredPagesListModule _pagesModule;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        public AlfredControlSubsystem() : this(new SimplePlatformProvider())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredControlSubsystem([NotNull] IPlatformProvider provider) : base(provider)
        {

            _controlPage = new AlfredModuleListPage(provider, ControlPageName);

            // Instantiate the modules
            _powerModule = new AlfredPowerModule(provider);
            _timeModule = new AlfredTimeModule(provider);
            _systemsModule = new AlfredSubsystemListModule(provider);
            _pagesModule = new AlfredPagesListModule(provider);

        }

        /// <summary>
        /// Registers the controls for this component.
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
                _eventLogPage = new AlfredEventLogPage(AlfredInstance.Console, EventLogPageName);
                Register(_eventLogPage);
            }
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
            get { return Resources.AlfredControlSubSystem_Name; }
        }


    }
}