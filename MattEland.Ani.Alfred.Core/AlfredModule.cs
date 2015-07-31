using System;
using System.Collections.Generic;
using System.ComponentModel;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Represents a module within Alfred. Modules contain different bits of information to present to the user.
    /// </summary>
    public abstract class AlfredModule : INotifyPropertyChanged
    {
        [NotNull]
        private readonly ICollectionProvider _collectionProvider;

        private AlfredStatus _status;
        private ICollection<AlfredWidget> _widgets;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredModule" />
        ///     class.
        /// </summary>
        /// <param
        ///     name="collectionProvider">
        ///     The collection provider.
        /// </param>
        /// <exception
        ///     cref="ArgumentNullException">
        /// </exception>
        protected AlfredModule([NotNull] ICollectionProvider collectionProvider)
        {
            if (collectionProvider == null)
            {
                throw new ArgumentNullException(nameof(collectionProvider));
            }

            _collectionProvider = collectionProvider;
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
        public abstract string NameAndVersion { get; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Initializes the module.
        /// </summary>
        /// <param
        ///     name="alfred">
        ///     The provider.
        /// </param>
        /// <exception
        ///     cref="System.ArgumentNullException">
        /// </exception>
        /// <exception
        ///     cref="System.InvalidOperationException">
        ///     Already online when told to initialize.
        /// </exception>
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

            EnsureWidgetsCollection();

            InitializeProtected();

            Status = AlfredStatus.Online;
        }

        /// <summary>
        /// Ensures the widgets collection exists, creating a new collection as needed.
        /// </summary>
        private void EnsureWidgetsCollection()
        {
            if (Widgets == null)
            {
                Widgets = _collectionProvider.CreateCollection<AlfredWidget>();
            }
        }

        /// <summary>
        ///     Called when a property changes.
        /// </summary>
        /// <param
        ///     name="propertyName">
        ///     Name of the property.
        /// </param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CanBeNull] string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Shuts down the module and decouples it from Alfred.
        /// </summary>
        /// <exception
        ///     cref="System.ArgumentNullException">
        /// </exception>
        /// <exception
        ///     cref="System.InvalidOperationException">
        ///     Already offline when told to shut down.
        /// </exception>
        public void Shutdown()
        {
            if (Status == AlfredStatus.Offline)
            {
                throw new InvalidOperationException($"{NameAndVersion} was already offline when told to shut down.");
            }

            ShutdownProtected();

            Widgets?.Clear();
            Widgets = null;

            Status = AlfredStatus.Offline;
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected virtual void ShutdownProtected()
        {
            // Handled by modules as needed
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        protected virtual void InitializeProtected()
        {
            // Handled by modules as needed
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        public void Update()
        {
            if (Status == AlfredStatus.Offline)
            {
                throw new InvalidOperationException($"{NameAndVersion} was offline when told to update.");
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
        /// <param
        ///     name="widget">
        ///     The widget.
        /// </param>
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