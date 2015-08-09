// ---------------------------------------------------------
// AlfredControlSubSystem.cs
// 
// Created on:      08/08/2015 at 6:12 PM
// Last Modified:   08/08/2015 at 6:58 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Pages;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     The control subsystem provides essential monitoring and control functionality for Alfred such as the Alfred control
    ///     page, an event log page, etc.
    /// </summary>
    public class AlfredControlSubSystem : AlfredSubSystem
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
        private readonly AlfredSubSystemListModule _systemsModule;

        [NotNull]
        private readonly AlfredPagesListModule _pagesModule;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubSystem" /> class.
        /// </summary>
        public AlfredControlSubSystem() : this(new SimplePlatformProvider())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubSystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredControlSubSystem([NotNull] IPlatformProvider provider) : base(provider)
        {

            _controlPage = new AlfredModuleListPage(provider, ControlPageName);

            // Instantiate the modules
            _powerModule = new AlfredPowerModule(provider);
            _timeModule = new AlfredTimeModule(provider);
            _systemsModule = new AlfredSubSystemListModule(provider);
            _pagesModule = new AlfredPagesListModule(provider);

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

        /// <summary>
        ///     Updates the component
        /// </summary>
        protected override void UpdateProtected()
        {
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(AlfredProvider alfred)
        {
            RegisterControls(AlfredInstance);
        }

        /// <summary>
        /// Called when the component is registered.
        /// </summary>
        /// <param name="alfred">The alfred.</param>
        public override void OnRegistered(AlfredProvider alfred)
        {
            base.OnRegistered(alfred);

            RegisterControls(alfred);

        }

        private void RegisterControls(AlfredProvider alfred)
        {

            Register(_controlPage);

            // Build out our control page
            _controlPage.ClearModules();
            _controlPage.Register(_powerModule);
            _controlPage.Register(_timeModule);
            _controlPage.Register(_systemsModule);
            _controlPage.Register(_pagesModule);

            // Don't include the event log page if there are no events
            if (alfred?.Console != null)
            {
                _eventLogPage = new AlfredEventLogPage(alfred.Console, EventLogPageName);
                Register(_eventLogPage);
            }
        }

        /// <summary>
        ///     Handles shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            _eventLogPage = null;
        }

        /// <summary>
        ///     A notification method that is invoked when shutdown for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnShutdownCompleted()
        {
            base.OnShutdownCompleted();
        }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnInitializationCompleted()
        {
            base.OnInitializationCompleted();
        }
    }
}