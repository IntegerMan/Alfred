// ---------------------------------------------------------
// TestSubSystem.cs
// 
// Created on:      08/07/2015 at 11:07 PM
// Last Modified:   08/08/2015 at 1:43 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

namespace MattEland.Ani.Alfred.Core.Tests.Mocks
{
    public class TestSubSystem : AlfredSubSystem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubSystem" /> class.
        /// </summary>
        public TestSubSystem() : base(new SimplePlatformProvider())
        {
        }

        /// <summary>
        ///     Gets the name of the subsystems.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return "Test SubSystem"; }
        }

        /// <summary>
        ///     Gets or sets the last time this module was updated.
        /// </summary>
        /// <value>The last time the module was updated.</value>
        public DateTime LastUpdated { get; set; }

        public DateTime LastInitializationCompleted { get; set; }

        /// <summary>
        ///     Gets or sets the last shutdown completed time.
        /// </summary>
        /// <value>The last shutdown completed time.</value>
        public DateTime LastShutdownCompleted { get; set; }

        /// <summary>
        ///     Gets or sets the last shutdown time.
        /// </summary>
        /// <value>The last shutdown time.</value>
        public DateTime LastShutdown { get; set; }

        /// <summary>
        ///     Gets or sets the last initialized time.
        /// </summary>
        /// <value>The last initialized time.</value>
        public DateTime LastInitialized { get; set; }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnInitializationCompleted()
        {
            LastInitializationCompleted = DateTime.Now;

            base.OnInitializationCompleted();
        }

        /// <summary>
        ///     A notification method that is invoked when shutdown for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnShutdownCompleted()
        {
            LastShutdownCompleted = DateTime.Now;

            base.OnShutdownCompleted();
        }

        /// <summary>
        ///     Handles shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            LastShutdown = DateTime.Now;

            base.ShutdownProtected();
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(AlfredProvider alfred)
        {
            LastInitialized = DateTime.Now;

            base.InitializeProtected(alfred);
        }

        /// <summary>
        ///     Handles updating the component as needed
        /// </summary>
        protected override void UpdateProtected()
        {
            LastUpdated = DateTime.Now;

            base.UpdateProtected();
        }
    }
}