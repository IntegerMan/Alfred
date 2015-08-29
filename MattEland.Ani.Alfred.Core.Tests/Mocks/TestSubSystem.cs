// ---------------------------------------------------------
// TestSubsystem.cs
// 
// Created on:      08/07/2015 at 11:07 PM
// Last Modified:   08/08/2015 at 1:43 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Tests.Mocks
{
    /// <summary>
    /// A test Subsystem. This class cannot be inherited.
    /// </summary>
    public sealed class TestSubsystem : AlfredSubsystem
    {
        [NotNull, ItemNotNull]
        private readonly ICollection<AlfredPage> _registerPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        internal TestSubsystem([NotNull] IObjectContainer container) : this(container, new SimplePlatformProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSubsystem" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="provider" /> is <see langword="null" />.</exception>
        internal TestSubsystem([NotNull] IObjectContainer container, [NotNull] IPlatformProvider provider) : base(container)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _registerPages = provider.CreateCollection<AlfredPage>();
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
        /// Registers the controls for this component.
        /// </summary>
        protected override void RegisterControls()
        {
            // Live up to our promise and auto-register some pages
            foreach (var page in _registerPages)
            {
                Register(page);
            }
        }

        /// <summary>
        ///     Gets or sets the last time this module was updated.
        /// </summary>
        /// <value>The last time the module was updated.</value>
        internal DateTime LastUpdated { get; set; }

        internal DateTime LastInitializationCompleted { get; set; }

        /// <summary>
        ///     Gets or sets the last shutdown completed time.
        /// </summary>
        /// <value>The last shutdown completed time.</value>
        internal DateTime LastShutdownCompleted { get; set; }

        /// <summary>
        ///     Gets or sets the last shutdown time.
        /// </summary>
        /// <value>The last shutdown time.</value>
        internal DateTime LastShutdown { get; set; }

        /// <summary>
        ///     Gets or sets the last initialized time.
        /// </summary>
        /// <value>The last initialized time.</value>
        internal DateTime LastInitialized { get; set; }

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
        protected override void InitializeProtected(IAlfred alfred)
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

        /// <summary>
        ///     Adds a <paramref name="page"/> to be automatically registered on initialization
        /// </summary>
        /// <param name="page">The page.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="page" /> is <see langword="null" /> .
        /// </exception>
        internal void AddAutoRegisterPage([NotNull] AlfredPage page)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            _registerPages.Add(page);
        }

        /// <summary>
        ///     Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        public override string Id
        {
            get { return "Test"; }
        }

        /// <summary>
        ///     Processes an Alfred Command. If the <paramref name="command"/> is handled,
        ///     <paramref name="result"/> should be modified accordingly and the method should
        ///     return true. Returning <see langword="false"/> will not stop the message from being
        ///     propagated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">
        /// The result. If the <paramref name="command"/> was handled, this should be updated.
        /// </param>
        /// <returns>
        ///     <c>True</c> if the <paramref name="command"/> was handled; otherwise false.
        /// </returns>
        public override bool ProcessAlfredCommand(ChatCommand command, AlfredCommandResult result)
        {
            var al = AlfredInstance as TestAlfred;
            if (al != null)
            {
                al.LastCommand = command;
            }

            return base.ProcessAlfredCommand(command, result);
        }
    }
}