using System;

using Assisticant.Collections;
using Assisticant.Fields;
using MattEland.Common.Annotations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Speech;
using MattEland.Ani.Alfred.PresentationAvalon.Commands;
using MattEland.Ani.Alfred.PresentationCommon.Commands;
using MattEland.Common;

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
            //- Contracts
            Contract.Requires(container != null);
            Contract.Ensures(AlfredApplication != null);
            Contract.Ensures(_name != null);
            Contract.Ensures(_selectedMFD != null);
            Contract.Ensures(_mfds != null);
            Contract.Ensures(Console != null);
            Contract.Ensures(_faultManager != null);
            Contract.Ensures(_updatePump != null);

            // Set the container before any other properties are set
            Container = container;

            //- Create Observables
            _name = new Observable<string>(DefaultWorkspaceName);
            _selectedMFD = new Observable<MultifunctionDisplay>();
            _mfds = new ObservableList<MultifunctionDisplay>();

            // Set up Alfred. This will not start Alfred
            var options = new ApplicationManagerOptions
            {
                IsSpeechEnabled = true,
                StackOverflowApiKey = "42", // TODO obviously
                BingApiKey = "42" // TODO obviously
            };


            // Create Alfred instance
            ApplicationManager = new ApplicationManager(container, options);

            Console = ApplicationManager.Console ?? new SimpleConsole(container);

            AlfredApplication = ApplicationManager.Alfred;

            // Add a faultIndicator indicator manager
            _faultManager = new FaultManager();
            _faultManager.Register(new FaultIndicatorModel("ALFRED",
                                                           () =>
                                                           AlfredApplication.IsOnline
                                                               ? FaultIndicatorStatus.Online
                                                               : FaultIndicatorStatus.Available));

            // Build the main update pump
            _updatePump = new DispatcherUpdatePump(TimeSpan.FromSeconds(0.1), Update);
        }

        /// <summary>
        ///     Gets or sets the console.
        /// </summary>
        /// <value>
        ///     The console.
        /// </value>
        [NotNull]
        public IConsole Console { get; }

        /// <summary>
        ///     Gets the manager for application.
        /// </summary>
        /// <value>
        ///     The application manager.
        /// </value>
        [NotNull]
        public ApplicationManager ApplicationManager { get; }

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

            // Update the indicators
            _faultManager.Update();
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
        public IAlfredContainer Container { get; }

        /// <summary>
        ///     The selected multifunction display.
        /// </summary>
        [NotNull]
        private readonly Observable<MultifunctionDisplay> _selectedMFD;

        /// <summary>
        ///     Manager for faultIndicator indicators.
        /// </summary>
        [NotNull]
        private readonly FaultManager _faultManager;

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
        ///     The manager for faultIndicator indicators.
        /// </summary>
        [NotNull]
        public FaultManager FaultManager
        {
            [DebuggerStepThrough]
            get
            { return _faultManager; }
        }

        /// <summary>
        ///     Gets the console.
        /// </summary>
        /// <value>
        ///     The console.
        /// </value>
        [NotNull]
        public IConsole LoggingConsole
        {
            get
            {
                Contract.Ensures(Contract.Result<IConsole>() != null);

                return Console;
            }
        }

        /// <summary>
        /// Creates and returns a new multifunction display (MFD). This display is added to the displays collection. 
        /// </summary>
        /// <param name="name"> The name of the display </param>
        /// <returns> A new display. </returns>
        [NotNull]
        public MultifunctionDisplay AddNewMultifunctionDisplay([NotNull] string name)
        {
            Contract.Requires(name != null);
            Contract.Requires(name.HasText());
            Contract.Ensures(Contract.Result<MultifunctionDisplay>() != null);
            Contract.Ensures(Contract.Result<MultifunctionDisplay>().Name == name);

            var item = new MultifunctionDisplay(Container, this, name);

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