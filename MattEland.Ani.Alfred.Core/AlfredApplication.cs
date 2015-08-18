// ---------------------------------------------------------
// AlfredApplication.cs
// 
// Created on:      07/25/2015 at 11:30 PM
// Last Modified:   08/11/2015 at 7:02 PM
// Original author: Matt Eland
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

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Coordinates providing personal assistance to a user interface and receiving settings and queries back from the user
    ///     interface.
    /// </summary>
    public sealed class AlfredApplication : INotifyPropertyChanged, IAlfred
    {
        /// <summary>
        ///     The platform provider
        /// </summary>
        [NotNull]
        private readonly IPlatformProvider _platformProvider;

        /// <summary>
        ///     The status controller
        /// </summary>
        [NotNull]
        private readonly IStatusController _statusController;

        [NotNull]
        private readonly ICollection<IAlfredSubsystem> _subsystems;

        [CanBeNull]
        private IChatProvider _chatProvider;

        /// <summary>
        ///     The status
        /// </summary>
        private AlfredStatus _status;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredApplication" /> class.
        /// </summary>
        /// <remarks>
        ///     Initialization should come from AlfredBootstrapper
        /// </remarks>
        /// <param name="provider">The provider.</param>
        /// <param name="controller">The controller.</param>
        /// <exception cref="System.ArgumentNullException">provider</exception>
        internal AlfredApplication([NotNull] IPlatformProvider provider, [CanBeNull] IStatusController controller)
        {
            // Set the controller
            if (controller == null)
            {
                controller = new AlfredStatusController(this);
            }
            _statusController = controller;
            _statusController.Alfred = this;

            // Set the provider
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            _platformProvider = provider;

            // Build out sub-collections
            _subsystems = provider.CreateCollection<IAlfredSubsystem>();
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
            { return _chatProvider; }
            private set
            {
                if (Equals(value, _chatProvider))
                {
                    return;
                }

                _chatProvider = value;
                OnPropertyChanged(nameof(ChatProvider));
            }
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
        ///     Gets the collection provider used for cross platform portability.
        /// </summary>
        /// <value>The collection provider.</value>
        [NotNull]
        public IPlatformProvider PlatformProvider
        {
            get { return _platformProvider; }
        }

        /// <summary>
        ///     Gets or sets the console provider. This can be null.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console { get; set; }

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
                return Subsystems.SelectMany(subSystem => subSystem.RootPages);
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
        ///     Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is already Online
        /// </exception>
        public void Initialize()
        {
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
        ///     Registers the chat provider as the framework's chat provider.
        /// </summary>
        /// <param name="chatProvider">The chat provider.</param>
        /// <exception cref="ArgumentNullException"><paramref name="chatProvider"/> is <see langword="null" />.</exception>
        public void Register([NotNull] IChatProvider chatProvider)
        {
            if (chatProvider == null)
            {
                throw new ArgumentNullException(nameof(chatProvider));
            }

            /* The ChatProvider we're getting is nice and all, but we want to be able
               to route ChatCommands to the various Subsystems while still allowing the
               user to interact directly with an IChatProvider. To do this we decorate
               the one that's coming in with our own provider that intercepts ChatCommands
               and routes them accordingly while still returning the same result to the user */

            ChatProvider = new DelegatingChatProvider(chatProvider, this);
        }

        /// <summary>
        ///     Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
                throw new InvalidOperationException(Resources.AlfredProvider_Update_ErrorMustBeOnline);
            }

            // Update every system
            foreach (var item in Subsystems)
            {
                item.Update();
            }
        }

        /// <summary>
        ///     Registers a sub system with Alfred.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <exception cref="InvalidOperationException">If a subsystem was registered when Alfred was offline.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="subsystem"/> is <see langword="null" />.</exception>
        public void Register([NotNull] AlfredSubsystem subsystem)
        {
            if (subsystem == null)
            {
                throw new ArgumentNullException(nameof(subsystem));
            }
            if (Status != AlfredStatus.Offline)
            {
                throw new InvalidOperationException(Resources.AlfredProvider_AssertMustBeOffline_ErrorNotOffline);
            }

            _subsystems.AddSafe(subsystem);
            subsystem.OnRegistered(this);
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