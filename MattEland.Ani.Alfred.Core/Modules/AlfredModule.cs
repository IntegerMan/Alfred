using System;
using System.Collections.Generic;
using System.ComponentModel;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    /// Represents a module within Alfred. Modules contain different bits of information to present to the user.
    /// </summary>
    public abstract class AlfredModule : INotifyPropertyChanged
    {
        private AlfredStatus _status;

        /// <summary>
        /// Gets the user interface widgets for the module.
        /// </summary>
        /// <value>The user interface widgets.</value>
        [CanBeNull]
        public ICollection<AlfredWidget> Widgets { get; private set; }

        /// <summary>
        /// Gets the alfred provider reference
        /// </summary>
        /// <value>The alfred provider reference.</value>
        [CanBeNull]
        protected AlfredProvider Alfred { get; private set; }

        /// <summary>
        /// Gets the name and version of the Module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public abstract string NameAndVersion { get; }

        /// <summary>
        /// Gets the status of the Module.
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
        /// Initializes the module.
        /// </summary>
        /// <param name="alfred">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException">Already online when told to initialize.</exception>
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

            Alfred = alfred;

            EnsureWidgetsCollection();

            InitializeProtected();

            Status = AlfredStatus.Online;
        }

        private void EnsureWidgetsCollection()
        {
            if (Alfred == null)
            {
                throw new InvalidOperationException("Alfred is not provided. Cannot create widgets collection.");
            }

            Widgets = Alfred.CollectionProvider.CreateCollection<AlfredWidget>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CanBeNull] string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Shuts down the module and decouples it from Alfred.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException">Already offline when told to shut down.</exception>
        public void Shutdown()
        {
            if (Status == AlfredStatus.Offline)
            {
                throw new InvalidOperationException($"{NameAndVersion} was already offline when told to shut down.");
            }

            ShutdownProtected();

            Alfred = null;

            Widgets?.Clear();
            Widgets = null;

            Status = AlfredStatus.Offline;
        }

        /// <summary>
        /// Handles module shutdown events
        /// </summary>
        protected virtual void ShutdownProtected()
        {
            // Handled by modules as needed
        }

        /// <summary>
        /// Handles module initialization events
        /// </summary>
        protected virtual void InitializeProtected()
        {
            // Handled by modules as needed
        }

        /// <summary>
        /// Handles updating the module as needed
        /// </summary>
        public void Update()
        {
            if (Status == AlfredStatus.Offline)
            {
                throw new InvalidOperationException($"{NameAndVersion} was offline when told to update.");
            }

            this.UpdateProtected();
        }

        /// <summary>
        /// Handles updating the module as needed
        /// </summary>
        protected virtual void UpdateProtected()
        {
            // Handled by modules as needed
        }

        /// <summary>
        /// Gets the user interface text displayed as a widget.
        /// </summary>
        /// <value>The user interface text.</value>
        public virtual string UserInterfaceText => null;

        /// <summary>
        /// Registers a widget for the module.
        /// </summary>
        /// <param name="widget">The widget.</param>
        protected void RegisterWidget([NotNull] AlfredWidget widget)
        {
            if (widget == null)
            {
                throw new ArgumentNullException(nameof(widget));
            }

            EnsureWidgetsCollection();

            // ReSharper disable once PossibleNullReferenceException
            Widgets.Add(widget);
        }
    }
}