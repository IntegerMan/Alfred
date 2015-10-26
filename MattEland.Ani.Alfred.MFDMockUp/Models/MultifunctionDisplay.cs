using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Assisticant.Fields;

using MattEland.Ani.Alfred.MFDMockUp.Views;
using MattEland.Common.Annotations;
using MattEland.Common.Providers;
using System.Diagnostics.Contracts;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A multifunction display. This class cannot be inherited.
    /// </summary>
    public sealed class MultifunctionDisplay
    {
        /// <summary>
        ///     The workspace.
        /// </summary>
        [NotNull]
        private readonly Workspace _workspace;

        /// <summary>
        ///     The default screen size.
        /// </summary>
        public const double DefaultScreenSize = 256;

        /// <summary>
        ///     The button provider.
        /// </summary>
        [NotNull]
        private readonly Observable<ButtonProvider> _buttonProvider;

        /// <summary>
        ///     The current view.
        /// </summary>
        [NotNull]
        private readonly Observable<ScreenModel> _currentScreen;

        [NotNull]
        private readonly Observable<string> _name;

        [NotNull]
        private readonly Observable<double> _screenHeight;

        [NotNull]
        private readonly Observable<double> _screenWidth;

        [NotNull]
        private readonly MFDProcessor _processor;

        [NotNull, ItemNotNull]
        private readonly ObservableCollection<ButtonModel> _buttonPresses;

        /// <summary>
        ///     Initializes a new instance of the MultifunctionDisplay class.
        /// </summary>
        public MultifunctionDisplay([NotNull] IObjectContainer container, [NotNull] Workspace workspace)
        {
            Contract.Requires(container != null);
            Contract.Requires(workspace != null);

            _workspace = workspace;

            _name = new Observable<string>("<New MFD>");

            _processor = new MFDProcessor(container, this);

            // We're going to start on a bootup mode and move on to the home screen after that loads
            var homeScreen = new HomeScreenModel();
            var bootScreen = new BootupScreenModel(homeScreen);

            //- Create Observable Properties
            _currentScreen = new Observable<ScreenModel>(bootScreen);
            _buttonProvider = new Observable<ButtonProvider>(new ButtonProvider(this));
            _isSensorOfInterest = new Computed<bool>(() => _workspace.SelectedMFD == this);
            _screenWidth = new Observable<double>(DefaultScreenSize);
            _screenHeight = new Observable<double>(DefaultScreenSize);
            _buttonPresses = new ObservableCollection<ButtonModel>();
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
        ///     Gets or sets the name of the multifunction display. This is different than the name of
        ///     its current view.
        /// </summary>
        /// <value>
        ///     The name of the MFD.
        /// </value>
        [CanBeNull]
        public string Name
        {
            get { return _name; }
            set { _name.Value = value; }
        }

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
        internal ButtonProvider ButtonProvider { get { return _buttonProvider; } }

        /// <summary>
        ///     Whether or not this is the sensor of interest.
        /// </summary>
        [NotNull]
        private readonly Computed<bool> _isSensorOfInterest;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is the sensor of interest.
        /// </summary>
        /// <value>
        ///     true if this instance is the sensor of interest, false if not.
        /// </value>
        public bool IsSensorOfInterest
        {
            get { return _isSensorOfInterest; }
            set { _workspace.SelectedMFD = this; }
        }

        /// <summary>
        ///     Updates this instance.
        /// </summary>
        public void Update()
        {
            _processor.Update();
        }

        /// <summary>
        ///     Responds to the action of a button associated with this MFD being clicked.
        /// </summary>
        /// <param name="button"> The button. </param>
        internal void OnButtonClicked([NotNull] ButtonModel button)
        {
            Contract.Requires(button != null);

            _buttonPresses.Add(button);
        }

        /// <summary>
        ///     Gets the button presses pending processing.
        /// </summary>
        /// <value>
        ///     The button presses pending processing.
        /// </value>
        [NotNull, ItemNotNull]
        internal ObservableCollection<ButtonModel> ButtonPresses
        {
            [DebuggerStepThrough]
            get
            { return _buttonPresses; }
        }
    }

}
