// ---------------------------------------------------------
// AlfredComponent.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:48 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     An abstract class containing most common shared functionality between Subsystems and Modules
    /// </summary>
    public abstract class AlfredComponent : INotifyPropertyChanged, IPropertyProvider
    {

        [CanBeNull]
        [ItemNotNull]
        private IEnumerable<IAlfredComponent> _childrenOnShutdown;

        /// <summary>
        ///     The logging console
        /// </summary>
        [CanBeNull]
        private IConsole _console;

        private AlfredStatus _status;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredComponent" /> class.
        /// </summary>
        /// <param name="console">The console.</param>
        protected AlfredComponent([CanBeNull] IConsole console = null)
        {
            _console = console;
        }

        /// <summary>
        ///     Gets the alfred instance associated with this component.
        /// </summary>
        /// <value>The alfred instance.</value>
        [CanBeNull]
        public IAlfred AlfredInstance
        {
            [DebuggerStepThrough]
            get;
            private set;
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
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the component is visible.</value>
        public abstract bool IsVisible { get; }

        /// <summary>
        ///     Gets the children of this component. Depending on the type of component this is, the children
        ///     will
        ///     vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        [NotNull]
        [ItemNotNull]
        public abstract IEnumerable<IAlfredComponent> Children { get; }

        /// <summary>
        ///     Gets the locale.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
        public CultureInfo Locale
        {
            get
            {
                if (AlfredInstance != null) { return AlfredInstance.Locale; }
                return CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        ///     Gets or sets the logging console.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console { get; protected set; } = null;

        /// <summary>
        ///     Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        public IEnumerable<IPropertyItem> Properties
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
        [NotNull, ItemNotNull]
        public virtual IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { return Children; }
        }

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
        ///     Allows components to define controls
        /// </summary>
        protected virtual void RegisterControls()
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
        ///     Clears all child collections
        /// </summary>
        protected virtual void ClearChildCollections()
        {
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
            foreach (var child in Children) { child.Update(); }
        }

        /// <summary>
        ///     Updates the component
        /// </summary>
        protected virtual void UpdateProtected()
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

        /// <summary>
        ///     Handles shutdown events
        /// </summary>
        protected virtual void ShutdownProtected()
        {
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred">The alfred instance.</param>
        protected virtual void InitializeProtected([NotNull] IAlfred alfred)
        {
        }

        /// <summary>
        ///     Logs an event to the console if a console was provided
        /// </summary>
        /// <param name="title">The title of the message.</param>
        /// <param name="message">The message body.</param>
        /// <param name="level">The logging level.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="title" /> is <see langword="null" />.</exception>
        protected void Log([NotNull] string title, [NotNull] string message, LogLevel level)
        {
            if (title == null) { throw new ArgumentNullException(nameof(title)); }
            if (message == null) { throw new ArgumentNullException(nameof(message)); }

            // Grab the console as Alfred may not have had the console object set during this module's construction
            if (_console == null) { _console = AlfredInstance?.Console; }

            // Send it on to the console, if we have one. It'll figure it out from there
            _console?.Log(title, message, level);
        }

        /// <summary>
        ///     Called when the component is registered.
        /// </summary>
        /// <param name="alfred">The alfred.</param>
        public virtual void OnRegistered([CanBeNull] IAlfred alfred)
        {
            // Hang on to the reference now so AlfredInstance doesn't lie and we can tell
            // our children who Alfred is before the whole update process happens
            AlfredInstance = alfred;

            RegisterControls();
        }

        /// <summary>
        ///     Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [NotifyPropertyChangedInvocator]
        [SuppressMessage("ReSharper", "CatchAllClause")]
        protected void OnPropertyChanged([CanBeNull] string propertyName)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                Log("Component.PropertyChanged",
                    "Encountered an exception raising a property changed event: "
                    + ex.BuildDetailsMessage(),
                    LogLevel.Error);
            }
        }

        /// <summary>
        ///     Registers the chat provider as the framework's chat provider.
        /// </summary>
        /// <param name="chatProvider">The chat provider.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="chatProvider" /> is <see langword="null" />
        ///     .
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     If Register is called when Alfred is null (prior to
        ///     initialization)
        /// </exception>
        protected void Register([NotNull] IChatProvider chatProvider)
        {
            if (chatProvider == null) { throw new ArgumentNullException(nameof(chatProvider)); }

            if (AlfredInstance == null)
            {
                throw new InvalidOperationException(Resources.NoAlfredInstance);
            }

            AlfredInstance.Register(chatProvider);
        }

        /// <summary>
        ///     Builds widget creation parameters for creating a widget.
        /// </summary>
        /// <param name="name">The name of the widget.</param>
        /// <returns>Usable WidgetCreationParameters.</returns>
        [NotNull]
        protected WidgetCreationParameters BuildWidgetParameters([NotNull] string name)
        {
            return new WidgetCreationParameters(name, Console, Locale);
        }
    }

}