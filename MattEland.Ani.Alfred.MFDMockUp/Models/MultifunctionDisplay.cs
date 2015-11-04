using System.Collections.Generic;
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

        [NotNull]
        private readonly Observable<string> _name;

        [NotNull]
        private readonly MFDProcessor _processor;

        [NotNull]
        private readonly Observable<double> _screenHeight;

        [NotNull]
        private readonly Observable<double> _screenWidth;
        /// <summary>
        ///     The workspace.
        /// </summary>
        [NotNull]
        private readonly Workspace _workspace;

        /// <summary>
        ///     Initializes a new instance of the MultifunctionDisplay class.
        /// </summary>
        public MultifunctionDisplay([NotNull] IAlfredContainer container, [NotNull] Workspace workspace)
        {
            Contract.Requires(container != null);
            Contract.Requires(workspace != null);

            _workspace = workspace;

            _processor = new MFDProcessor(container, this);

            // Create the provider objects
            ScreenProvider = new ScreenProvider(this, workspace);
            ButtonProvider = new ButtonProvider(this, workspace);

            //- Create Observable Properties
            _name = new Observable<string>("<New MFD>");
            _isSensorOfInterest = new Computed<bool>(() => _workspace.SelectedMFD == this);
            _screenWidth = new Observable<double>(DefaultScreenSize);
            _screenHeight = new Observable<double>(DefaultScreenSize);

            // Set up the current screen as the boot screen
            _currentScreen = new Observable<ScreenModel>(ScreenProvider.BootScreen);
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
            set { _workspace.SelectedMFD = this; }
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

            _processor.EnqueueButtonPress(button);
        }

        /// <summary>
        ///     Updates this instance.
        /// </summary>
        internal void Update()
        {
            _processor.Update();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
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
        public CultureInfo Culture
        {
            get { return CultureInfo.CurrentCulture; }
        }
    }

}
