// ---------------------------------------------------------
// AlfredApplication.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 5:58 PM
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
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Coordinates providing personal assistance to a user interface and receiving settings and
    ///     queries back from the user
    ///     interface.
    /// </summary>
    public sealed class AlfredApplication : INotifyPropertyChanged, IAlfred
    {

        /// <summary>
        ///     The root pages collection
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<IAlfredPage> _rootPages;

        /// <summary>
        ///     The status controller
        /// </summary>
        [NotNull]
        private readonly IStatusController _statusController;

        [NotNull]
        private readonly ICollection<IAlfredSubsystem> _subsystems;

        /// <summary>
        ///     The chat provider
        /// </summary>
        [CanBeNull]
        private IChatProvider _chatProvider;

        /// <summary>
        ///     The status
        /// </summary>
        private AlfredStatus _status;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredApplication" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        public AlfredApplication([NotNull] IObjectContainer container)
        {
            // Validate
            if (container == null) { throw new ArgumentNullException(nameof(container)); }
            Container = container;

            // Set the controller
            _statusController = new AlfredStatusController(this, container);

            // Build out sub-collections
            _subsystems = container.ProvideCollection<IAlfredSubsystem>();
            _rootPages = container.ProvideCollection<IAlfredPage>();
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IObjectContainer Container { get; }

        /// <summary>
        ///     Gets the name and version of Alfred.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public string NameAndVersion
        {
            get { return string.Format(CultureInfo.CurrentCulture, "{0} {1}", Name, Version); }
        }

        /// <summary>
        ///     Gets the name of the framework.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public string Version
        {
            get
            {
                // We'll base this off of the AssemblyVersion.
                var version = this.GetAssemblyVersion();
                return version?.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        ///     Gets the chat provider.
        /// </summary>
        /// <value>The chat provider.</value>
        [CanBeNull]
        public IChatProvider ChatProvider
        {
            [DebuggerStepThrough]
            get
            {
                return _chatProvider;
            }
            private set
            {
                if (Equals(value, _chatProvider)) { return; }

                _chatProvider = value;
                OnPropertyChanged(nameof(ChatProvider));
            }
        }

        /// <summary>
        ///     Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is already Online
        /// </exception>
        public void Initialize()
        {
            _rootPages.Clear();

            // This logic is a bit lengthy, so we'll have the status controller take care of it
            _statusController.Initialize();
        }

        /// <summary>
        ///     Gets Whether or not Alfred is online.
        /// </summary>
        /// <value>The is online.</value>
        public bool IsOnline
        {
            get { return Status == AlfredStatus.Online; }
        }

        /// <summary>
        ///     Registers the page as a root page.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="page"> The page. </param>
        public void Register(IAlfredPage page)
        {
            if (page == null) { throw new ArgumentNullException(nameof(page)); }

            if (page.IsRootLevel) { _rootPages.Add(page); }

            page.OnRegistered(this);
        }

        /// <summary>
        ///     Registers the shell command recipient that will allow the shell to get commands from the Alfred
        ///     layer.
        /// </summary>
        /// <param name="shell">The command recipient.</param>
        /// <exception cref="ArgumentNullException"><paramref name="shell" /> is <see langword="null" />.</exception>
        public void Register(IShellCommandRecipient shell)
        {
            if (shell == null) { throw new ArgumentNullException(nameof(shell)); }
            ShellCommandHandler = shell;
        }

        /// <summary>
        ///     Registers the chat provider as the framework's chat provider.
        /// </summary>
        /// <param name="chatProvider">The chat provider.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="chatProvider" /> is <see langword="null" />
        ///     .
        /// </exception>
        public void Register([NotNull] IChatProvider chatProvider)
        {
            if (chatProvider == null) { throw new ArgumentNullException(nameof(chatProvider)); }

            ChatProvider = chatProvider;
        }

        /// <summary>
        ///     Registers a sub system with Alfred.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <exception cref="InvalidOperationException">If a subsystem was registered when Alfred was offline.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="subsystem" /> is <see langword="null" />.</exception>
        public void Register([NotNull] IAlfredSubsystem subsystem)
        {
            if (subsystem == null) { throw new ArgumentNullException(nameof(subsystem)); }
            if (Status != AlfredStatus.Offline)
            {
                throw new InvalidOperationException(
                    Resources.AlfredProvider_AssertMustBeOffline_ErrorNotOffline);
            }

            _subsystems.AddSafe(subsystem);
            subsystem.OnRegistered(this);
        }

        /// <summary>
        ///     Gets the user interface pages registered to the Alfred Framework at root level.
        /// </summary>
        /// <value>The pages.</value>
        [NotNull]
        [ItemNotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public IEnumerable<IAlfredPage> RootPages
        {
            get
            {
                // Give me all pages in subsystems that are root level pages
                return _rootPages;
            }
        }

        /// <summary>
        ///     Gets the shell command handler that can pass shell commands on to the user interface.
        /// </summary>
        /// <value>The shell command handler.</value>
        [CanBeNull]
        public IShellCommandRecipient ShellCommandHandler { get; private set; }

        /// <summary>
        ///     Tells Alfred to go ahead and shut down.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is already Offline
        /// </exception>
        public void Shutdown()
        {
            // This process is a little lengthy so we'll have the status controller handle it
            _statusController.Shutdown();
        }

        /// <summary>
        ///     Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public AlfredStatus Status
        {
            get { return _status; }
            set
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
        ///     Gets the sub systems associated wih Alfred.
        /// </summary>
        /// <value>The sub systems.</value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<IAlfredSubsystem> Subsystems
        {
            get { return _subsystems; }
        }

        /// <summary>
        ///     Tells modules to take a look at their content and update as needed.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is not Online
        /// </exception>
        public void Update()
        {
            // Error check
            if (Status != AlfredStatus.Online)
            {
                throw new InvalidOperationException(
                    Resources.AlfredProvider_Update_ErrorMustBeOnline);
            }

            // Update every system
            foreach (var item in Subsystems) { item.Update(); }
        }

        /// <summary>
        ///     Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Gets the display name for use in the user interface.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        /// Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public string ItemTypeName
        {
            get { return "App Framework"; }
        }

        /// <summary>
        /// Gets the locale.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
        public CultureInfo Locale { get; set; } = CultureInfo.CurrentCulture;

        /// <summary>
        ///     Gets the name of the framework.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public string Name
        {
            get { return Resources.AlfredProvider_Name.NonNull(); }
        }

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
                yield return new AlfredProperty("Is Online", IsOnline);
                yield return new AlfredProperty("Subsystems", Subsystems.Count());
                yield return new AlfredProperty("Root Pages", RootPages.Count());
                yield return new AlfredProperty("Version", Version);
                yield return new AlfredProperty("Container", Container);
            }
        }

        /// <summary>
        ///     Gets the property providers.
        /// </summary>
        /// <value>The property providers.</value>
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get
            {
                return Subsystems;
            }
        }

        /// <summary>
        ///     Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CanBeNull] string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}