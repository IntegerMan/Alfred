using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Represents a module within Alfred. Modules contain different bits of information to present to the user.
    /// </summary>
    public abstract class AlfredModule : NotifyPropertyChangedBase
    {
        [NotNull]
        private readonly ICollectionProvider _collectionProvider;

        private AlfredStatus _status;
        private ICollection<AlfredWidget> _widgets;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredModule" />
        ///     class.
        /// </summary>
        /// <param
        ///     name="collectionProvider">
        ///     The collection provider.
        /// </param>
        /// <exception
        ///     cref="ArgumentNullException">
        /// </exception>
        protected AlfredModule([NotNull] ICollectionProvider collectionProvider)
        {
            if (collectionProvider == null)
            {
                throw new ArgumentNullException(nameof(collectionProvider));
            }

            _collectionProvider = collectionProvider;
        }

        /// <summary>
        ///     Gets the user interface widgets for the module.
        /// </summary>
        /// <value>The user interface widgets.</value>
        [CanBeNull]
        public ICollection<AlfredWidget> Widgets
        {
            get { return _widgets; }
            private set
            {
                if (Equals(value, _widgets))
                {
                    return;
                }
                _widgets = value;
                OnPropertyChanged(nameof(Widgets));
            }
        }

        /// <summary>
        ///     Gets the name and version of the Module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public abstract string NameAndVersion { get; }

        /// <summary>
        ///     Gets the status of the Module.
        /// </summary>
        /// <value>The status.</value>
        public AlfredStatus Status
        {
            get { return _status; }
            private set
            {
                if (value == _status)
                {
                    return;
                }
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        /// <summary>
        ///     Initializes the module.
        /// </summary>
        /// <param
        ///     name="alfred">
        ///     The provider.
        /// </param>
        /// <exception
        ///     cref="ArgumentNullException">
        /// </exception>
        /// <exception
        ///     cref="InvalidOperationException">
        ///     Already online when told to initialize.
        /// </exception>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])")]
        public void Initialize([NotNull] AlfredProvider alfred)
        {
            if (alfred == null)
            {
                throw new ArgumentNullException(nameof(alfred));
            }

            if (Status == AlfredStatus.Online)
            {
                throw new InvalidOperationException($"{NameAndVersion} was already online when told to initialize.");
            }

            // Don't allow any residual widgets now that we can display widgets while shut down
            Widgets?.Clear();

            EnsureWidgetsCollection();

            InitializeProtected();

            Status = AlfredStatus.Online;
        }

        /// <summary>
        /// Ensures the widgets collection exists, creating a new collection as needed.
        /// </summary>
        private void EnsureWidgetsCollection()
        {
            if (Widgets == null)
            {
                Widgets = _collectionProvider.CreateCollection<AlfredWidget>();
            }
        }

        /// <summary>
        ///     Shuts down the module and decouples it from Alfred.
        /// </summary>
        /// <exception
        ///     cref="ArgumentNullException">
        /// </exception>
        /// <exception
        ///     cref="InvalidOperationException">
        ///     Already offline when told to shut down.
        /// </exception>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])")]
        public void Shutdown()
        {
            if (Status == AlfredStatus.Offline)
            {
                throw new InvalidOperationException($"{NameAndVersion} was already offline when told to shut down.");
            }

            // We want to clear out the list before shutdown is called so that modules can re-register components for display during shutdown mode as needed
            Widgets?.Clear();

            // Tell the derived module that it's now time to do any special logic (e.g. registering widgets, shutting down resources, stopping timers, etc.)
            ShutdownProtected();

            Status = AlfredStatus.Offline;
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected virtual void ShutdownProtected()
        {
            // Handled by modules as needed
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        protected virtual void InitializeProtected()
        {
            // Handled by modules as needed
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])")]
        public void Update()
        {
            if (Status == AlfredStatus.Offline)
            {
                throw new InvalidOperationException($"{NameAndVersion} was offline when told to update.");
            }

            UpdateProtected();
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        protected virtual void UpdateProtected()
        {
            // Handled by modules as needed
        }

        /// <summary>
        ///     Registers a widget for the module.
        /// </summary>
        /// <param
        ///     name="widget">
        ///     The widget.
        /// </param>
        protected void RegisterWidget([NotNull] AlfredWidget widget)
        {
            if (widget == null)
            {
                throw new ArgumentNullException(nameof(widget));
            }

            // This shouldn't happen, but I want to check to make sure
            if (Widgets != null && Widgets.Contains(widget))
            {
                throw new InvalidOperationException("The specified widget was already part of the collection");
            }

            EnsureWidgetsCollection();

            // ReSharper disable once PossibleNullReferenceException
            Widgets.Add(widget);
        }

        /// <summary>
        /// Registers multiple widgets at once.
        /// </summary>
        /// <param name="widgets">The widgets.</param>
        protected void RegisterWidgets([NotNull] IEnumerable<AlfredWidget> widgets)
        {
            if (widgets == null)
            {
                throw new ArgumentNullException(nameof(widgets));
            }

            foreach (var widget in widgets)
            {
                // ReSharper disable once AssignNullToNotNullAttribute - for testing purposes we'll allow this
                RegisterWidget(widget);
            }
        }
    }
}