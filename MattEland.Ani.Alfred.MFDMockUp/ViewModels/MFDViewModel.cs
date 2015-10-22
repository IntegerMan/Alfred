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
        [NotNull]
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
                var currentScreen = Model.CurrentScreen;

                var vm = Locator.ViewModelFor(currentScreen);

                // TODO: When this is null, use another view model

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

    }

}