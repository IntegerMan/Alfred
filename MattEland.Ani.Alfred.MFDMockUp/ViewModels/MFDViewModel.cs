// ---------------------------------------------------------
// MFDViewModel.cs
// 
// Created on:      10/18/2015 at 12:18 AM
// Last Modified:   10/18/2015 at 12:18 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------
using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;
using System;
using System.Diagnostics;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.MFDMockUp.ViewModels.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     A ViewModel for a <see cref="MultifunctionDisplay"/>.
    /// </summary>
    [PublicAPI]
    [ViewModelFor(typeof(MultifunctionDisplay))]
    public sealed class MFDViewModel
    {
        /// <summary>
        ///     The MFD model backing store.
        /// </summary>
        [NotNull]
        private readonly MultifunctionDisplay _model;

        /// <summary>
        ///     The view model locator.
        /// </summary>
        [NotNull]
        private readonly ViewModelLocator _locator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MFDViewModel"/> class.
        /// </summary>
        /// <param name="locator"> The view model locator. </param>
        /// <param name="mfd"> The multifunction display. </param>
        public MFDViewModel([NotNull] ViewModelLocator locator, [NotNull] MultifunctionDisplay mfd)
        {
            _model = mfd;
            _locator = locator;
        }

        /// <summary>
        ///     Gets the bottom buttons.
        /// </summary>
        /// <value>
        ///     The bottom buttons.
        /// </value>
        [NotNull]
        public MFDButtonStripViewModel BottomButtons
        {
            get { return new MFDButtonStripViewModel(ButtonProvider.BottomButtons); }
        }

        /// <summary>
        ///     Gets the left buttons.
        /// </summary>
        /// <value>
        ///     The left buttons.
        /// </value>
        [NotNull]
        public MFDButtonStripViewModel LeftButtons
        {
            get { return new MFDButtonStripViewModel(ButtonProvider.LeftButtons); }
        }

        /// <summary>
        ///     Gets the <see cref="MultifunctionDisplay"/> model backing this view model.
        /// </summary>
        /// <value>
        ///     The model.
        /// </value>
        [NotNull]
        public MultifunctionDisplay Model
        {
            get
            {
                return _model;
            }
        }

        /// <summary>
        ///     Gets the name of the MFD.
        /// </summary>
        /// <remarks>
        ///     This is the name of the physical MFD, not its current View.
        /// </remarks>
        /// <value>
        ///     The name of the MFD.
        /// </value>
        [CanBeNull]
        public string Name
        {
            get { return Model.Name; }
        }
        /// <summary>
        ///     Gets the right buttons.
        /// </summary>
        /// <value>
        ///     The right buttons.
        /// </value>
        [NotNull]
        public MFDButtonStripViewModel RightButtons
        {
            get { return new MFDButtonStripViewModel(ButtonProvider.RightButtons); }
        }

        /// <summary>
        ///     Gets or sets the height of the screen.
        /// </summary>
        /// <value>
        ///     The height of the screen.
        /// </value>
        public double ScreenHeight
        {
            get { return Model.ScreenHeight; }
            set { Model.ScreenHeight = value; }
        }

        /// <summary>
        ///     Gets or sets the width of the screen.
        /// </summary>
        /// <value>
        ///     The width of the screen.
        /// </value>
        public double ScreenWidth
        {
            get { return Model.ScreenWidth; }
            set { Model.ScreenWidth = value; }
        }

        /// <summary>
        ///     Gets the top buttons.
        /// </summary>
        /// <value>
        ///     The top buttons.
        /// </value>
        [NotNull]
        public MFDButtonStripViewModel TopButtons
        {
            get { return new MFDButtonStripViewModel(ButtonProvider.TopButtons); }
        }

        /// <summary>
        ///     Gets the current view.
        /// </summary>
        /// <value>
        ///     The view.
        /// </value>
        public object View
        {
            get
            {
                // Grab the screen we need a view for
                var currentScreen = Model.CurrentScreen;

                //- Safely handle a null screen
                if (currentScreen == null) return null;

                // If we've already created a VM, don't recreate one
                object vm = currentScreen.GetViewModelFor(_model);
                if (vm != null) return vm;

                // Build a view model
                vm = Locator.ViewModelFor(currentScreen, _model);

                // Set the screen view model into the view
                var screenVM = vm as ScreenViewModel;
                if (screenVM != null) currentScreen.SetViewModelFor(this, screenVM);

                // Return the value provided
                return vm;
            }
        }

        /// <summary>
        ///     Gets the button provider.
        /// </summary>
        /// <value>
        ///     The button provider.
        /// </value>
        [NotNull]
        private ButtonProvider ButtonProvider
        {
            get { return Model.ButtonProvider; }
        }

        /// <summary>
        ///     The view model locator.
        /// </summary>
        [NotNull]
        public ViewModelLocator Locator
        {
            [DebuggerStepThrough]
            get
            { return _locator; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is the sensor of interest.
        /// </summary>
        /// <value>
        ///     true if this instance is the sensor of interest, false if not.
        /// </value>
        public bool IsSensorOfInterest
        {
            get { return _model.IsSensorOfInterest; }
        }

        /// <summary>
        ///     Gets a value indicating whether the sensor of interest control should be shown (this
        ///     includes visual states for when a MFD is SOI and when it is not SOI).
        /// </summary>
        /// <value>
        ///     true if the SOI / not SOI control should be shown, false if not.
        /// </value>
        public bool IsSensorOfInterestIndicatorVisible
        {
            get { return _model.IsSensorOfInterestVisible; }
        }

        /// <summary>
        ///     Gets the current master mode.
        /// </summary>
        /// <value>
        ///     The current master mode.
        /// </value>
        [NotNull]
        public MasterModeBase CurrentMode
        {
            get { return _model.MasterMode; }
        }

    }

}