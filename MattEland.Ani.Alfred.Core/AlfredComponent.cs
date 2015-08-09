// ---------------------------------------------------------
// AlfredComponent.cs
// 
// Created on:      08/07/2015 at 10:53 PM
// Last Modified:   08/07/2015 at 11:43 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     An abstract class containing most common shared functionality between SubSystems and Modules
    /// </summary>
    public abstract class AlfredComponent : NotifyPropertyChangedBase
    {
        [CanBeNull]
        private AlfredProvider _alfred;

        private AlfredStatus _status;

        [CanBeNull, ItemNotNull]
        private IEnumerable<AlfredComponent> _childrenOnShutdown;

        /// <summary>
        /// Gets the alfred instance associated with this component.
        /// </summary>
        /// <value>The alfred instance.</value>
        [CanBeNull]
        protected internal AlfredProvider AlfredInstance
        {
            [DebuggerStepThrough]
            get
            { return _alfred; }
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
            protected set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        public abstract string Name { get; }

        /// <summary>
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the component is visible.</value>
        public abstract bool IsVisible { get; }

        /// <summary>
        ///     Initializes the component.
        /// </summary>
        /// <param name="alfred">The alfred framework.</param>
        /// <exception cref="InvalidOperationException">Already online when told to initialize.</exception>
        public virtual void Initialize([NotNull] AlfredProvider alfred)
        {
            if (alfred == null)
            {
                throw new ArgumentNullException(nameof(alfred));
            }

            if (Status == AlfredStatus.Online)
            {
                var format = Resources.AlfredModule_InitializeAlreadyOnline.NonNull();
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                                                                  format,
                                                                  NameAndVersion));
            }

            Status = AlfredStatus.Initializing;

            _alfred = alfred;

            // Reset our children collections so that other collections can be registered during shutdown
            ClearChildCollections();

            InitializeProtected(alfred);

            // Pass on the message to the children
            foreach (var child in Children)
            {
                child.Initialize(alfred);
            }

            Status = AlfredStatus.Online;

            OnPropertyChanged(nameof(IsVisible));
        }

        /// <summary>
        ///     Shuts down the component.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Already offline when told to shut down.
        /// </exception>
        public virtual void Shutdown()
        {
            if (Status == AlfredStatus.Offline)
            {
                var format = Resources.AlfredComponent_ShutdownAlreadyOffline.NonNull();
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, format, NameAndVersion));
            }

            Status = AlfredStatus.Terminating;

            // Pass on the message to the children
            _childrenOnShutdown = Children.ToList();
            foreach (var child in _childrenOnShutdown)
            {
                child?.Shutdown();
            }

            // Reset our children collections so that other collections can be registered during shutdown
            ClearChildCollections();

            // Tell the derived module that it's now time to do any special logic (e.g. registering widgets, shutting down resources, stopping timers, etc.)
            ShutdownProtected();

            Status = AlfredStatus.Offline;

            OnPropertyChanged(nameof(IsVisible));
        }

        /// <summary>
        /// Clears all child collections
        /// </summary>
        protected virtual void ClearChildCollections()
        {
        }

        /// <summary>
        ///     Handles updating the component as needed
        /// </summary>
        public void Update()
        {
            if (Status == AlfredStatus.Offline)
            {
                var message = string.Format(CultureInfo.CurrentCulture,
                                            Resources.AlfredItemOfflineButToldToUpdate.NonNull(),
                                            NameAndVersion);
                throw new InvalidOperationException(message);
            }

            UpdateProtected();

            // Pass on the message to the children
            foreach (var child in Children)
            {
                child.Update();
            }

        }

        /// <summary>
        ///     Updates the component
        /// </summary>
        protected virtual void UpdateProtected()
        {
        }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public virtual void OnInitializationCompleted()
        {
            // Pass on the message to the children
            foreach (var child in Children)
            {
                child.OnInitializationCompleted();
            }
        }

        /// <summary>
        ///     A notification method that is invoked when shutdown for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public virtual void OnShutdownCompleted()
        {
            var notified = new HashSet<AlfredComponent>();

            // Pass on the message to the children
            foreach (var child in Children)
            {
                child.OnShutdownCompleted();
                notified.Add(child);
            }

            // Tell our old expatriated children that things ended
            if (_childrenOnShutdown != null)
            {
                foreach (var child in _childrenOnShutdown.Where(child => !notified.Contains(child)))
                {
                    child?.OnShutdownCompleted();
                    notified.Add(child);
                }
            }
        }

        /// <summary>
        ///     Handles shutdown events
        /// </summary>
        protected virtual void ShutdownProtected()
        {
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected virtual void InitializeProtected(AlfredProvider alfred)
        {
        }

        /// <summary>
        ///     Logs an event to the console if a console was provided
        /// </summary>
        /// <param name="title">The title of the message.</param>
        /// <param name="message">The message body.</param>
        /// <param name="level">The logging level.</param>
        protected void Log([NotNull] string title, [NotNull] string message, LogLevel level)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            // Grab the console as Alfred may not have had the console object set during this module's construction
            var console = _alfred?.Console;

            // Send it on to the console, if we have one. It'll figure it out from there
            console?.Log(title, message, level);

        }

        /// <summary>
        /// Gets the children of this component. Depending on the type of component this is, the children will
        /// vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        [NotNull, ItemNotNull]
        public abstract IEnumerable<AlfredComponent> Children { get; }

    }
}