// ---------------------------------------------------------
// ComponentBase.cs
// 
// Created on:      09/02/2015 at 6:20 PM
// Last Modified:   09/03/2015 at 5:22 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     An <see langword="abstract"/> class containing most common shared functionality between
    ///     Subsystems and Modules
    /// </summary>
    public abstract class ComponentBase : NotifyChangedBase, IPropertyProvider
    {

        [CanBeNull]
        [ItemNotNull]
        private IEnumerable<IAlfredComponent> _childrenOnShutdown;

        private AlfredStatus _status;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentBase"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
        protected ComponentBase([NotNull] IObjectContainer container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            Container = container;
        }

        /// <summary>
        ///     Gets the <see cref="IAlfred" /> instance associated with this component.
        /// </summary>
        /// <value>The Alfred instance.</value>
        [CanBeNull]
        public IAlfred AlfredInstance
        {
            [DebuggerStepThrough]
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="IObjectContainer"/> used to provide types.
        /// </summary>
        /// <value>The container.</value>
        [NotNull]
        public IObjectContainer Container { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is online.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if this instance is online, <see langword="false"/> if not.
        /// </value>
        public bool IsOnline
        {
            get { return Status == AlfredStatus.Online; }
        }

        /// <summary>
        ///     Gets the name and version of the component.
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
        public string Version
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
                    OnPropertyChanged(nameof(IsOnline));
                }
            }
        }

        /// <summary>
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the component is visible.</value>
        public abstract bool IsVisible { get; }

        /// <summary>
        ///     Gets the children of the component. Depending on the type of component this is, the children
        ///     will vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        [NotNull]
        [ItemNotNull]
        public abstract IEnumerable<IAlfredComponent> Children { get; }

        /// <summary>
        ///     Gets the display name for use in the user interface.
        /// </summary>
        /// <value>The display name.</value>
        public virtual string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        ///     Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        [NotNull]
        public abstract string ItemTypeName { get; }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        [NotNull]
        public abstract string Name { get; }

        /// <summary>
        ///     Gets a list of properties provided by this item.
        /// </summary>
        /// <returns>The properties</returns>
        public virtual IEnumerable<IPropertyItem> Properties
        {
            get
            {
                yield return new AlfredProperty("Name", Name);
                yield return new AlfredProperty("Status", Status);
                yield return new AlfredProperty("Child Items", Children.Count());
                yield return new AlfredProperty("Visible", IsVisible);
                yield return new AlfredProperty("Version", Version);
            }
        }

        /// <summary>
        ///     Gets the property providers nested inside of this property provider.
        /// </summary>
        /// <value>The property providers.</value>
        [NotNull]
        [ItemNotNull]
        public virtual IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { return Children; }
        }

        #region Registration

        /// <summary>
        ///     Gets the registration provider.
        /// </summary>
        /// <value>
        ///     The registration provider.
        /// </value>
        public IRegistrationProvider RegistrationProvider
        {
            get { return AlfredInstance?.RegistrationProvider; }
        }

        /// <summary>
        ///     Allows components to define controls
        /// </summary>
        protected virtual void RegisterControls()
        {
        }

        /// <summary>
        ///     Registers the chat provider as the framework's chat provider.
        /// </summary>
        /// <param name="chatProvider">The chat provider.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="chatProvider" /> is <see langword="null" />
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     If Register is called when Alfred is null (prior to
        ///     initialization)
        /// </exception>
        protected void Register([NotNull] IChatProvider chatProvider)
        {
            if (chatProvider == null) { throw new ArgumentNullException(nameof(chatProvider)); }

            var registrator = RegistrationProvider;
            if (registrator == null)
            {
                throw new InvalidOperationException(Resources.NoAlfredInstance);
            }

            registrator.Register(chatProvider);
        }

        /// <summary>
        ///     Called when the component is registered.
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        public virtual void OnRegistered([CanBeNull] IAlfred alfred)
        {
            // Hang on to the reference now so AlfredInstance doesn't lie and we can tell
            // our children who Alfred is before the whole update process happens
            AlfredInstance = alfred;

            RegisterControls();
        }

        #endregion

        #region Lifecycle

        /// <summary>
        ///     Initializes the component.
        /// </summary>
        /// <param name="alfred">The alfred framework.</param>
        /// <exception cref="InvalidOperationException">Already online when told to initialize.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="alfred" /> is <see langword="null" />.</exception>
        public virtual void Initialize([NotNull] IAlfred alfred)
        {
            if (alfred == null) { throw new ArgumentNullException(nameof(alfred)); }

            if (Status == AlfredStatus.Online)
            {
                var format = Resources.AlfredModule_InitializeAlreadyOnline.NonNull();
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                                                                  format,
                                                                  NameAndVersion));
            }

            Status = AlfredStatus.Initializing;

            AlfredInstance = alfred;

            // Reset our children collections so that other collections can be registered during shutdown
            ClearChildCollections();

            RegisterControls();

            InitializeProtected(alfred);

            // Pass on the message to the children
            foreach (var child in Children) { child.Initialize(alfred); }

            Status = AlfredStatus.Online;

            OnPropertyChanged(nameof(IsVisible));
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        protected virtual void InitializeProtected([NotNull] IAlfred alfred)
        {
        }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can
        ///     be fully enabled or
        ///     adjusted
        /// </summary>
        public virtual void OnInitializationCompleted()
        {
            // Pass on the message to the children
            foreach (var child in Children) { child.OnInitializationCompleted(); }
        }

        /// <summary>
        ///     Handles updating the component as needed
        /// </summary>
        /// <exception cref="InvalidOperationException">If this was called when Alfred was offline.</exception>
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
        ///     Shuts down the component.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Already offline when told to shut down.
        /// </exception>
        public virtual void Shutdown()
        {
            if (Status == AlfredStatus.Offline)
            {
                var format = Resources.AlfredComponent_ShutdownAlreadyOffline.NonNull();
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                                                                  format,
                                                                  NameAndVersion));
            }

            Status = AlfredStatus.Terminating;

            // Pass on the message to the children
            _childrenOnShutdown = Children.ToList();
            foreach (var child in _childrenOnShutdown) { child?.Shutdown(); }

            // Reset our children collections so that other collections can be registered during shutdown
            ClearChildCollections();

            // Tell the derived module that it's now time to do any special logic (e.g. registering widgets, shutting down resources, stopping timers, etc.)
            ShutdownProtected();

            Status = AlfredStatus.Offline;

            OnPropertyChanged(nameof(IsVisible));
        }

        /// <summary>
        ///     Handles shutdown events
        /// </summary>
        protected virtual void ShutdownProtected()
        {
        }

        /// <summary>
        ///     A notification method that is invoked when shutdown for Alfred is complete so the UI can be
        ///     fully enabled or
        ///     adjusted
        /// </summary>
        public virtual void OnShutdownCompleted()
        {
            var notified = new HashSet<IAlfredComponent>();

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

        #endregion

        /// <summary>
        ///     Clears all child collections
        /// </summary>
        protected virtual void ClearChildCollections()
        {
        }

        /// <summary>
        ///     Builds widget creation parameters for creating a widget.
        /// </summary>
        /// <param name="name">The name of the widget.</param>
        /// <returns>Usable <see cref="WidgetCreationParameters"/>.</returns>
        [NotNull]
        protected WidgetCreationParameters BuildWidgetParameters([NotNull] string name)
        {
            return new WidgetCreationParameters(name, Container);
        }

        /// <summary>
        ///     Handles a callback <see cref="Exception" /> .
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="operationName">
        /// <see cref="Name"/> of the operation that was being performed.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if it the <paramref name="exception"/> was handled and should
        ///     not be thrown again, otherwise false.
        /// </returns>
        public override bool HandleCallbackException(Exception exception, string operationName)
        {
            // Log to the console
            var message = exception.BuildDetailsMessage();
            message = $"{GetType().Name}.{operationName} encountered an error: {message}";
            message.Log(operationName, LogLevel.Error, Container);

            // It's been logged. Don't throw it again
            return true;
        }

    }

}