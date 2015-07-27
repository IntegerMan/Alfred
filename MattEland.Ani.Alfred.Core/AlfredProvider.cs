using System;
using System.Collections.Generic;
using System.ComponentModel;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Modules;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// Coordinates providing personal assistance to a user interface and receiving settings and queries back from the user
    /// interface.
    /// </summary>
    public class AlfredProvider : INotifyPropertyChanged
    {
        private AlfredStatus _status;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredProvider"/> class.
        /// </summary>
        /// <param name="provider">The collection provider.</param>
        public AlfredProvider([NotNull] ICollectionProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            CollectionProvider = provider;
            Modules = provider.CreateCollection<AlfredModule>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredProvider"/> class.
        /// </summary>
        public AlfredProvider() : this(new SimpleCollectionProvider())
        {
        }

        /// <summary>
        /// Gets or sets the console provider. This can be null.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console { get; set; }

        /// <summary>
        /// Gets the name and version of Alfred.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public static string NameAndVersion => "Alfred 0.1 Alpha";

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <value>The modules.</value>
        [NotNull]
        public ICollection<AlfredModule> Modules { get; private set; }

        /// <summary>
        /// Gets the status.
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
        /// Gets the collection provider used for cross platform portability.
        /// </summary>
        /// <value>The collection provider.</value>
        public ICollectionProvider CollectionProvider { get; }

        /// <summary>
        /// Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if Alfred is already Online</exception>
        public void Initialize()
        {
            const string LogHeader = "Alfred.Initialize";

            // Handle case on initialize but already initializing or online
            if (Status == AlfredStatus.Online)
            {
                const string Message = "Instructed to initialize but system is already online";
                Console?.Log(LogHeader, Message);

                throw new InvalidOperationException(Message);
            }

            Console?.Log(LogHeader, "Initializing...");

            // Boot up Modules and give them a provider
            foreach (var module in Modules)
            {
                Console?.Log(LogHeader, $"Initializing {module.NameAndVersion}");
                module.Initialize(this);
                Console?.Log(LogHeader, $"{module.NameAndVersion} is now initialized.");
            }

            // We're done. Let the world know.
            Status = AlfredStatus.Online;
            Console?.Log(LogHeader, "Initilization Completed.");
        }

        /// <summary>
        /// Tells Alfred to go ahead and shut down.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if Alfred is already Offline</exception>
        public void Shutdown()
        {
            const string LogHeader = "Alfred.Shutdown";

            // Handle case on shutdown but already offline
            if (Status == AlfredStatus.Offline)
            {
                const string Message = "Instructed to shut down but system is already offline";
                Console?.Log(LogHeader, Message);

                throw new InvalidOperationException(Message);
            }

            Console?.Log(LogHeader, "Shutting down...");

            // Shut down modules and decouple them from Alfred
            foreach (var module in Modules)
            {
                Console?.Log(LogHeader, $"Shutting down {module.NameAndVersion}");
                module.Shutdown();
                Console?.Log(LogHeader, $"{module.NameAndVersion} is now offline.");
            }

            // We're done here. Tell the world.
            Status = AlfredStatus.Offline;
            Console?.Log(LogHeader, "Shut down completed.");
        }

        /// <summary>
        /// Tells modules to take a look at their content and update as needed.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if Alfred is not Online</exception>
        public void Update()
        {

            // Error check
            if (Status != AlfredStatus.Online)
            {
                throw new InvalidOperationException("Alfred must be online in order to update modules.");
            }

            foreach (var module in Modules)
            {
                module.Update();
            }
        }

        /// <summary>
        /// Adds the standard internal modules to Alfred.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if Alfred is not Offline</exception>
        public void AddStandardModules()
        {
            if (Status != AlfredStatus.Offline)
            {
                throw new InvalidOperationException("Alfred must be offline in order to add modules.");
            }

            Modules.Add(new AlfredTimeModule());
        }

        #region Notify Property Changed

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property changes to support the property changed notifications.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CanBeNull] string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}