// ---------------------------------------------------------
// AlfredModule.cs
// 
// Created on:      07/29/2015 at 3:01 PM
// Last Modified:   08/07/2015 at 11:17 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core
{

    /// <summary>
    ///     Represents a module within Alfred. Modules contain different bits of information to present to the user.
    /// </summary>
    public abstract class AlfredModule : AlfredComponent, IDisposable
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<AlfredWidget> _widgets;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredModule"/> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected AlfredModule([NotNull] IPlatformProvider platformProvider)
        {
            if (platformProvider == null)
            {
                throw new ArgumentNullException(nameof(platformProvider));
            }

            _widgets = platformProvider.CreateCollection<AlfredWidget>();
        }

        /// <summary>
        ///     Gets the user interface widgets for the module.
        /// </summary>
        /// <value>The user interface widgets.</value>
        [NotNull]
        public IEnumerable<AlfredWidget> Widgets
        {
            get { return _widgets; }
        }

        /// <summary>
        ///     Gets whether or not the module is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the module is visible.</value>
        public override bool IsVisible
        {
            get
            {
                return _widgets.Any(w => w.IsVisible);
            }
        }

        /// <summary>
        ///     Dispose of anything that needs to be done. By default nothing needs to be disposed of, but some modules will
        ///     need to support this and should override Dispose.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        ///     Initializes the module.
        /// </summary>
        /// <param name="alfred">
        ///     The provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Already online when told to initialize.
        /// </exception>
        public override void Initialize([NotNull] AlfredProvider alfred)
        {
            // Don't allow any residual widgets now that we can display widgets while shut down
            _widgets.Clear();

            base.Initialize(alfred);
        }

        /// <summary>
        ///     Shuts down the module and decouples it from Alfred.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Already offline when told to shut down.
        /// </exception>
        public override void Shutdown()
        {
            // We want to clear out the list before shutdown is called so that modules can re-register components for display during shutdown mode as needed
            _widgets.Clear();

            base.Shutdown();
        }

        /// <summary>
        ///     Registers a widget for the module.
        /// </summary>
        /// <param name="widget">
        ///     The widget.
        /// </param>
        protected void Register([NotNull] AlfredWidget widget)
        {
            _widgets.AddSafe(widget);

            OnPropertyChanged(nameof(IsVisible));
        }

        /// <summary>
        ///     Registers multiple widgets at once.
        /// </summary>
        /// <param name="widgets">
        ///     The widgets.
        /// </param>
        protected void Register([NotNull] IEnumerable<AlfredWidget> widgets)
        {
            if (widgets == null)
            {
                throw new ArgumentNullException(nameof(widgets));
            }

            foreach (var widget in widgets)
            {
                // ReSharper disable once AssignNullToNotNullAttribute - for testing purposes we'll allow this
                Register(widget);
            }
        }
    }
}