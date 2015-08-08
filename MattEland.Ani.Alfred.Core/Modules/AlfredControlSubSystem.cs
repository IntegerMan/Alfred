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
            _controlPage = new AlfredModuleListPage(provider) { Name = ControlPageName };

            var power = new AlfredPowerModule(provider);
            _controlPage.Register(power);

            var time = new AlfredTimeModule(provider);
            _controlPage.Register(time);

            var systems = new AlfredSubSystemListModule(provider);
            _controlPage.Register(systems);

            var pages = new AlfredPagesListModule(provider);
            _controlPage.Register(pages);

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
            Register(_controlPage);

            if (alfred?.Console != null)
            {
                _eventLogPage = new AlfredEventLogPage(alfred.Console) { Name = EventLogPageName };
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
        }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnInitializationCompleted()
        {
        }
    }
}