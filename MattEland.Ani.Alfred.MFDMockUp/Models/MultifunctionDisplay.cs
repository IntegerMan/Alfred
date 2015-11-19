using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assisticant.Fields;

using MattEland.Common.Annotations;
using MattEland.Common.Providers;
using System.Diagnostics.Contracts;
using System.Globalization;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens;
using MattEland.Common;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A multifunction display. This class cannot be inherited.
    /// </summary>
    public sealed class MultifunctionDisplay
    {

        /// <summary>
        ///     The default screen size.
        /// </summary>
        public const double DefaultScreenSize = 256;

        /// <summary>
        ///     The current view.
        /// </summary>
        [NotNull]
        private readonly Observable<ScreenModel> _currentScreen;

        /// <summary>
        ///     Whether or not this is the sensor of interest.
        /// </summary>
        [NotNull]
        private readonly Computed<bool> _isSensorOfInterest;

        /// <summary>
        ///     Whether or not to show the SOI / Not SOI control for this screen.
        /// </summary>
        [NotNull]
        private readonly Computed<bool> _isSensorOfInterestVisible;

        [NotNull]
        private readonly Observable<double> _screenHeight;

        [NotNull]
        private readonly Observable<double> _screenWidth;

        /// <summary>
        ///     Initializes a new instance of the MultifunctionDisplay class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="workspace"> The workspace. </param>
        /// <param name="name"> The name of the MFD. </param>
        public MultifunctionDisplay([NotNull] IAlfredContainer container,
            [NotNull] Workspace workspace,
            [NotNull] string name)
        {
            Contract.Requires(container != null);
            Contract.Requires(workspace != null);
            Contract.Requires(name != null);
            Contract.Requires(name.HasText());
            Contract.Ensures(Name == name);
            Contract.Ensures(Workspace == workspace);

            // Set Simple Properties
            Workspace = workspace;
            Name = name;
            Processor = new MFDProcessor(container, this);

            // Set up the master modes
            ScreenProvider = new ScreenProvider(this, workspace);
            _systemMasterMode = new SystemMasterMode(this);
            _bootupMasterMode = new BootupMasterMode(this, _systemMasterMode);
            _currentMasterMode = new Observable<MasterModeBase>(_bootupMasterMode);

            // Now that we have the master modes, set up the provider
            ButtonProvider = new ButtonProvider(this, MasterMode);

            // Set up the current screen as the default screen of the first mode
            _currentScreen = new Observable<ScreenModel>(MasterMode.DefaultScreen);

            //- Create Observable Properties
            _isSensorOfInterest = new Computed<bool>(() => Workspace.SelectedMFD == this);
            _isSensorOfInterestVisible = new Computed<bool>(() => CurrentScreen.ShowSensorOfInterestIndicator);
            _screenWidth = new Observable<double>(DefaultScreenSize);
            _screenHeight = new Observable<double>(DefaultScreenSize);
        }

        [NotNull]
        private readonly MasterModeBase _bootupMasterMode;

        [NotNull]
        private readonly SystemMasterMode _systemMasterMode;

        [NotNull]
        private readonly Observable<MasterModeBase> _currentMasterMode;

        /// <summary>
        ///     Gets or sets the current master mode.
        /// </summary>
        /// <value>
        ///     The master mode.
        /// </value>
        [NotNull]
        public MasterModeBase MasterMode
        {
            get { return _currentMasterMode; }
            set
            {
                _currentMasterMode.Value = value;
                ButtonProvider.MasterMode = value;
            }
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(Workspace != null);
            Contract.Invariant(Name != null);
            Contract.Invariant(Name.HasText());
            Contract.Invariant(Processor != null);
            Contract.Invariant(CurrentScreen != null);
        }

        /// <summary>
        ///     Gets the processor associated with this display.
        /// </summary>
        /// <value>
        ///     The processor.
        /// </value>
        [NotNull]
        public MFDProcessor Processor
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     The workspace.
        /// </summary>
        [NotNull]
        public Workspace Workspace
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets or sets the current screen.
        /// </summary>
        /// <value>
        ///     The current screen.
        /// </value>
        [NotNull]
        public ScreenModel CurrentScreen
        {
            get { return _currentScreen; }
            set { _currentScreen.Value = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is the sensor of interest.
        /// </summary>
        /// <value>
        ///     true if this instance is the sensor of interest, false if not.
        /// </value>
        public bool IsSensorOfInterest
        {
            get { return _isSensorOfInterest; }
            set { Workspace.SelectedMFD = this; }
        }

        /// <summary>
        ///     Gets the name of the multifunction display. This is different than the name of
        ///     its current view.
        /// </summary>
        /// <value>
        ///     The name of the MFD.
        /// </value>
        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     Gets or sets the height of the screen.
        /// </summary>
        /// <value>
        ///     The height of the screen.
        /// </value>
        public double ScreenHeight
        {
            get { return _screenHeight; }
            set { _screenHeight.Value = value; }
        }

        /// <summary>
        ///     Gets or sets the width of the screen.
        /// </summary>
        /// <value>
        ///     The width of the screen.
        /// </value>
        public double ScreenWidth
        {
            get { return _screenWidth; }
            set { _screenWidth.Value = value; }
        }

        /// <summary>
        ///     Gets the button provider associated with this display.
        /// </summary>
        /// <value>
        ///     The button provider.
        /// </value>
        [NotNull]
        internal ButtonProvider ButtonProvider { get; }

        /// <summary>
        ///     Gets the screen provider.
        /// </summary>
        /// <value>
        ///     The screen provider.
        /// </value>
        [NotNull]
        internal ScreenProvider ScreenProvider { get; }

        /// <summary>
        ///     Responds to the action of a button associated with this MFD being clicked.
        /// </summary>
        /// <param name="button"> The button. </param>
        internal void OnButtonClicked([NotNull] ButtonModel button)
        {
            Contract.Requires(button != null);

            Processor.EnqueueButtonPress(button);
        }

        /// <summary>
        ///     Updates this instance.
        /// </summary>
        internal void Update()
        {
            Processor.Update();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        [NotNull]
        public override string ToString()
        {
            return string.Format(Culture, "Display {0}", Name);
        }

        /// <summary>
        ///     Gets the user interface culture associated with this display.
        /// </summary>
        /// <value>
        ///     The culture.
        /// </value>
        [NotNull]
        public CultureInfo Culture
        {
            get { return CultureInfo.CurrentCulture; }
        }

        /// <summary>
        ///     Gets a value indicating whether the sensor of interest is visible.
        /// </summary>
        /// <value>
        ///     true if the sensor of interest is visible, false if not.
        /// </value>
        public bool IsSensorOfInterestVisible
        {
            get { return _isSensorOfInterestVisible.Value; }
        }
    }

    /// <summary>
    ///     The bootup master mode. This class cannot be inherited.
    /// </summary>
    public sealed class BootupMasterMode : MasterModeBase
    {
        [NotNull]
        private readonly BootupScreenModel _bootScreen;

        /// <summary>
        ///     Initializes a new instance of the BootupMasterMode class.
        /// </summary>
        /// <param name="display"> The display. </param>
        /// <param name="nextMasterMode"> The next master mode. </param>
        public BootupMasterMode([NotNull] MultifunctionDisplay display,
            [NotNull] MasterModeBase nextMasterMode) : base(display, nextMasterMode)
        {
            Contract.Requires(display != null);
            Contract.Requires(nextMasterMode != null);

            _bootScreen = new BootupScreenModel(nextMasterMode);
        }

        /// <summary>
        ///     Gets the default screen when this master mode is switched to.
        /// </summary>
        /// <value>
        ///     The default screen.
        /// </value>
        public override ScreenModel DefaultScreen { get { return _bootScreen; } }

        /// <summary>
        ///     Gets screen change buttons.
        /// </summary>
        /// <returns>
        ///     An enumerable of screen change buttons.
        /// </returns>
        public override IEnumerable<ButtonModel> GetScreenChangeButtons()
        {
            yield break;
        }

        /// <summary>
        ///     Gets the screen command buttons related to the current screen.
        /// </summary>
        /// <returns>
        /// An enumerable of screen command buttons.
        /// </returns>
        public override IEnumerable<ButtonModel> GetScreenCommandButtons()
        {
            yield break;
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(_bootScreen != null);
        }

    }

}
