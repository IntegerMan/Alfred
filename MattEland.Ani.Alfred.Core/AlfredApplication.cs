// ---------------------------------------------------------
// AlfredApplication.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/03/2015 at 1:24 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;
using System.Diagnostics.Contracts;

namespace MattEland.Ani.Alfred.Core
{

    /// <summary>
    ///     Coordinates providing personal assistance to a user interface and receiving settings and
    ///     queries back from the user interface.
    /// </summary>
    public sealed class AlfredApplication : NotifyChangedBase, IAlfred, IDisposable
    {

        /// <summary>
        ///     The root pages collection
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<IPage> _rootPages;

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
        ///     Thrown when the container is null.
        /// </exception>
        /// <param name="container"> The container. </param>
        public AlfredApplication([NotNull] IAlfredContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            // The specialized container needs a reference to this object as we construct ourselves
            container.Alfred = this;

            // Set the container
            Container = container;

            // Build out sub-collections
            _subsystems = container.ProvideCollection<IAlfredSubsystem>();
            _rootPages = container.ProvideCollection<IPage>();

            // Set the Search Controller from the provider, falling back to a new default type
            SearchController = container.TryProvide<ISearchController>()
                               ?? new AlfredSearchController(Container);

            // Set composite objects - TODO: Get these from the container!
            _statusController = new AlfredStatusController(container);
            RegistrationProvider = new ComponentRegistrationProvider(container, this, _subsystems, _rootPages);

            // Set Command Router
            var router = new AlfredCommandRouter(container);
            container.CommandRouter = router;
            router.RegisterAsProvidedInstance(typeof(IAlfredCommandRecipient), container);
        }

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
            internal set
            {
                if (Equals(value, _chatProvider)) { return; }

                _chatProvider = value;
                OnPropertyChanged(nameof(ChatProvider));
            }
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
        ///     Gets the user interface pages registered to the Alfred Framework at root level.
        /// </summary>
        /// <value>The pages.</value>
        [NotNull]
        [ItemNotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public IEnumerable<IPage> RootPages
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
        public IShellCommandRecipient ShellCommandHandler { get; internal set; }

        /// <summary>
        ///     Gets the registration provider that manages registering other components for Alfred.
        /// </summary>
        /// <value>
        ///     The registration provider.
        /// </value>
        public IRegistrationProvider RegistrationProvider { get; }

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
        ///     Gets the sub systems associated with Alfred.
        /// </summary>
        /// <value>The sub systems.</value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<IAlfredSubsystem> Subsystems
        {
            get { return _subsystems; }
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IAlfredContainer Container { get; }

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
            get { return Subsystems; }
        }

        /// <summary>
        ///     Gets the search controller. This <see langword="object"/> provides search functionality
        ///     and manages the search and results processing.
        /// </summary>
        /// <value>
        ///     The search controller.
        /// </value>
        [NotNull]
        public ISearchController SearchController { get; private set; }

        /// <summary>
        ///     Gets the components registered to Alfred. This will include subsystems as well as various
        ///     helper components.
        /// </summary>
        /// <value>
        ///     The components.
        /// </value>
        public IEnumerable<IAlfredComponent> Components
        {
            get
            {
                foreach (var subsystem in this.Subsystems) { yield return subsystem; }

                yield return SearchController;
            }
        }

        /// <summary>
        /// Gets the command router.
        /// </summary>
        /// <value>
        /// The command router.
        /// </value>
        [NotNull]
        public IAlfredCommandRecipient CommandRouter
        {
            get { return Container.CommandRouter; }
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
                throw new InvalidOperationException(@"Alfred must be online in order to update components.");
            }

            // Update every registered component
            foreach (var item in Components) { item.Update(); }
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
            var message = exception.BuildDetailsMessage(culture: Container.Locale);
            message = $"{operationName} encountered an error: {message}";
            message.Log(operationName, LogLevel.Error, Container);

            // It's been logged. Don't throw it again
            return true;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting
        ///     unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Tear down registered components
            foreach (var component in Components)
            {
                component.TryDispose();
            }

            // Tear down composite objects
            ChatProvider.TryDispose();
            ShellCommandHandler.TryDispose();
        }
    }

}