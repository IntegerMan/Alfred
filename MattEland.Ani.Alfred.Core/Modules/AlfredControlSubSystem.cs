// ---------------------------------------------------------
// AlfredControlSubSystem.cs
// 
// Created on:      08/08/2015 at 6:12 PM
// Last Modified:   08/08/2015 at 6:24 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     The control subsystem provides essential monitoring and control functionality for Alfred such as the Alfred control
    ///     page, an event log page, etc.
    /// </summary>
    public class AlfredControlSubSystem : AlfredSubSystem
    {
        [NotNull]
        private readonly UserInterfacePage _page;

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
            var power = new AlfredPowerModule(provider);
            Register(power);

            var time = new AlfredTimeModule(provider);
            Register(time);

            var systems = new AlfredSubSystemListModule(provider);
            Register(systems);

            _page = new UserInterfacePage(provider);

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
            Register(_page);
        }

        /// <summary>
        ///     Handles shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
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