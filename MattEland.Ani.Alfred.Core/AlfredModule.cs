// ---------------------------------------------------------
// AlfredModule.cs
// 
// Created on:      07/29/2015 at 3:01 PM
// Last Modified:   08/05/2015 at 2:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Represents a module within Alfred. Modules contain different bits of information to present to the user.
    /// </summary>
    public abstract class AlfredModule : NotifyPropertyChangedBase, IDisposable
    {
        [NotNull]
        private readonly IPlatformProvider _platformProvider;

        private AlfredStatus _status;

        [CanBeNull]
        [ItemNotNull]
        private ICollection<AlfredWidget> _widgets;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="AlfredModule" />
        ///     class.
        /// </summary>
        /// <param name="platformProvider">
        ///     The platform provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        protected AlfredModule([NotNull] IPlatformProvider platformProvider)
        {
            if (platformProvider == null)
            {
                throw new ArgumentNullException(nameof(platformProvider));
            }

            _platformProvider = platformProvider;
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
        public string NameAndVersion
        {
            get { return string.Format(CultureInfo.CurrentCulture, "{0} {1}", Name, Version); }
        }

        /// <summary>
        ///     Gets the version of the module.
        /// </summary>
        /// <value>The version.</value>
        [CanBeNull]
        public virtual string Version
        {
            get
            {
                // We'll base this off of the AssemblyVersion.
                return this.GetAssemblyVersion()?.ToString();
            }
        }

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
        ///     Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        [NotNull]
        public abstract string Name { get; }

        /// <summary>
        ///     Gets whether or not the module is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the module is visible.</value>
        public bool IsVisible
        {
            get
            {
                // TODO: This could use some tests
                return _widgets != null && _widgets.Any(w => w != null && w.IsVisible);
            }
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
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider",
            MessageId = "System.String.Format(System.String,System.Object[])")]
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

            Status = AlfredStatus.Initializing;

            // Don't allow any residual widgets now that we can display widgets while shut down
            Widgets?.Clear();

            EnsureWidgetsCollection();

            InitializeProtected();

            Status = AlfredStatus.Online;
        }

        /// <summary>
        ///     Ensures the widgets collection exists, creating a new collection as needed.
        /// </summary>
        private void EnsureWidgetsCollection()
        {
            if (Widgets == null)
            {
                Widgets = _platformProvider.CreateCollection<AlfredWidget>();
            }
        }

        /// <summary>
        ///     Shuts down the module and decouples it from Alfred.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Already offline when told to shut down.
        /// </exception>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider",
            MessageId = "System.String.Format(System.String,System.Object[])")]
        public void Shutdown()
        {
            if (Status == AlfredStatus.Offline)
            {
                throw new InvalidOperationException($"{NameAndVersion} was already offline when told to shut down.");
            }

            Status = AlfredStatus.Terminating;

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
            OnPropertyChanged(nameof(IsVisible));
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        protected virtual void InitializeProtected()
        {
            // Handled by modules as needed
            OnPropertyChanged(nameof(IsVisible));
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider",
            MessageId = "System.String.Format(System.String,System.Object[])")]
        public void Update()
        {
            if (Status == AlfredStatus.Offline)
            {
                var message = string.Format(
                                            CultureInfo.CurrentCulture,
                                            "{0} was offline when told to update.",
                                            NameAndVersion);
                throw new InvalidOperationException(message);
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
        /// <param name="widget">
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

            OnPropertyChanged(nameof(IsVisible));
        }

        /// <summary>
        ///     Registers multiple widgets at once.
        /// </summary>
        /// <param name="widgets">
        ///     The widgets.
        /// </param>
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

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public virtual void OnInitializationCompleted()
        {
        }

        /// <summary>
        ///     A notification method that is invoked when shutdown for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public virtual void OnShutdownCompleted()
        {
            OnPropertyChanged(nameof(IsVisible));
        }

        /// <summary>
        /// Dispose of anything that needs to be done. By default nothing needs to be disposed of, but some modules will
        /// need to support this and should override Dispose.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}