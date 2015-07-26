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
        /// Gets or sets the console provider. This can be null.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console { get; set; }

        /// <summary>
        /// Gets the name and version string for display purposes.
        /// </summary>
        /// <value>The name and version string.</value>
        public static string NameAndVersionString => "Alfred 0.1 Alpha";

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <value>The modules.</value>
        [NotNull]
        public ICollection<AlfredModule> Modules { get; } = new HashSet<AlfredModule>();

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
        /// Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        public void Initialize()
        {
            // Tell folks we're initializing
            Status = AlfredStatus.Initializing;

            const string LogHeader = "Alfred.Initialize";

            Console?.Log(LogHeader, "Initializing...");

            // TODO: Set things up here

            // We're done. Let the world know.
            Status = AlfredStatus.Online;
            Console?.Log(LogHeader, "Initilization Completed.");
        }

        /// <summary>
        /// Tells Alfred to go ahead and shut down.
        /// </summary>
        public void Shutdown()
        {
            const string LogHeader = "Alfred.Shutdown";

            Console?.Log(LogHeader, "Shutting down...");

            // TODO: Tear things down

            Console?.Log(LogHeader, "Shut down completed.");
        }

        /// <summary>
        /// Adds the standard internal modules to Alfred.
        /// </summary>
        public void AddStandardModules()
        {
            Modules.Add(new AlfredTimeModule());
        }

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property changes to support the property changed notifications.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}