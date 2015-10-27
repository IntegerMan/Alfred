using System;

using Assisticant.Collections;
using Assisticant.Fields;
using MattEland.Common.Annotations;
using System.Collections.Generic;
using MattEland.Common.Providers;
using System.Diagnostics.Contracts;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A workspace containing multiple MFDs. This class cannot be inherited.
    /// </summary>
    [PublicAPI]
    public sealed class Workspace
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public Workspace([NotNull] IAlfredContainer container)
        {
            Contract.Requires(container != null);

            // Set the container before any other properties are set
            Container = container;

            //- Create Observables
            _name = new Observable<string>(DefaultWorkspaceName);
            _mfds = new ObservableList<MultifunctionDisplay>();
            _selectedMFD = new Observable<MultifunctionDisplay>();

            // Set up Alfred. This will not start Alfred
            AlfredApplication = new AlfredApplication(container);

            // Build the main update pump
            _updatePump = new DispatcherUpdatePump(TimeSpan.FromSeconds(0.1), Update);
        }

        /// <summary>
        ///     Updates the workspace's contents.
        /// </summary>
        private void Update()
        {
            // Update each MFD
            foreach (var mfd in _mfds)
            {
                mfd.Update();
            }
        }

        /// <summary>
        /// The default workspace name. 
        /// </summary>
        [NotNull]
        private const string DefaultWorkspaceName = "Alfred MFD Prototype";

        [NotNull, ItemNotNull]
        private readonly ObservableList<MultifunctionDisplay> _mfds;

        /// <summary>
        /// The name's observable backing store. 
        /// </summary>
        [NotNull]
        private readonly Observable<string> _name;

        [NotNull]
        private readonly DispatcherUpdatePump _updatePump;

        /// <summary>
        /// Gets the multifunction displays (MFDs). 
        /// </summary>
        /// <value> The displays. </value>
        [NotNull, ItemNotNull]
        public IEnumerable<MultifunctionDisplay> MFDs
        {
            get { return _mfds; }
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="Workspace"/>. 
        /// </summary>
        /// <value> The name of the workspace. </value>
        [NotNull]
        public string Name
        {
            get { return _name; }
            set { _name.Value = value; }
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IObjectContainer Container { get; }

        [NotNull]
        private readonly Observable<MultifunctionDisplay> _selectedMFD;

        /// <summary>
        ///     Gets or sets the selected multifunction display.
        /// </summary>
        /// <value>
        ///     The selected multifunction display.
        /// </value>
        [CanBeNull]
        public MultifunctionDisplay SelectedMFD
        {
            get { return _selectedMFD; }
            set { _selectedMFD.Value = value; }
        }

        /// <summary>
        ///     Gets the alfred application.
        /// </summary>
        /// <value>
        ///     The alfred application.
        /// </value>
        [NotNull]
        public AlfredApplication AlfredApplication { get; }

        /// <summary>
        /// Creates a new multifunction display (MFD). 
        /// </summary>
        /// <returns> A new display. </returns>
        [NotNull]
        public MultifunctionDisplay NewMFD()
        {
            var item = new MultifunctionDisplay(Container, this);
            _mfds.Add(item);
            return item;
        }

        /// <summary>
        ///     Starts the main update pump that runs the Multifunction Displays.
        /// </summary>
        public void Start()
        {
            _updatePump.Start();
        }
    }
}