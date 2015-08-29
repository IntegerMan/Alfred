// ---------------------------------------------------------
// TestPage.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/29/2015 at 12:53 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Tests.Mocks
{
    /// <summary>A test page for testing the <see cref="AlfredPage" /> update pumps</summary>
    public class TestPage : AlfredPage
    {
        /// <summary>Initializes a new instance of the <see cref="AlfredSubsystem" /> class.</summary>
        /// <param name="container">The container.</param>
        public TestPage([NotNull] IObjectContainer container) : base(container, "Test Page", "Test")
        {
        }

        /// <summary>
        ///     Gets or sets the last time the page was updated.
        /// </summary>
        /// <value>
        /// The last time the module was updated.
        /// </value>
        public DateTime LastUpdated { get; set; }

        /// <summary>Gets or sets the last time the page was notified initialization completed.</summary>
        /// <value>The last initialization completed.</value>
        public DateTime LastInitializationCompleted { get; set; }

        /// <summary>Gets or sets the last shutdown completed time.</summary>
        /// <value>The last shutdown completed time.</value>
        public DateTime LastShutdownCompleted { get; set; }

        /// <summary>Gets or sets the last shutdown time.</summary>
        /// <value>The last shutdown time.</value>
        public DateTime LastShutdown { get; set; }

        /// <summary>Gets or sets the last initialized time.</summary>
        /// <value>The last initialized time.</value>
        public DateTime LastInitialized { get; set; }

        /// <summary>
        ///     Gets the children of the component.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { yield break; }
        }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI
        ///     can be fully enabled or adjusted
        /// </summary>
        public override void OnInitializationCompleted()
        {
            LastInitializationCompleted = DateTime.Now;

            base.OnInitializationCompleted();
        }

        /// <summary>
        ///     A notification method that is invoked when shutdown for Alfred is complete so the UI can
        ///     be fully enabled or adjusted
        /// </summary>
        public override void OnShutdownCompleted()
        {
            LastShutdownCompleted = DateTime.Now;

            base.OnShutdownCompleted();
        }

        /// <summary>Handles shutdown events</summary>
        protected override void ShutdownProtected()
        {
            LastShutdown = DateTime.Now;

            base.ShutdownProtected();
        }

        /// <summary>Handles initialization events</summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            LastInitialized = DateTime.Now;

            base.InitializeProtected(alfred);
        }

        /// <summary>Handles updating the component as needed</summary>
        protected override void UpdateProtected()
        {
            LastUpdated = DateTime.Now;

            base.UpdateProtected();
        }
    }
}