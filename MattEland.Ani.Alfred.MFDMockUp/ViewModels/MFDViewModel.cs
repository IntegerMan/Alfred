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
namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     A ViewModel for a <see cref="MultifunctionDisplay"/>.
    /// </summary>
    [PublicAPI]
    public sealed class MFDViewModel
    {
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
                return _mfd;
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

        [NotNull]
        public MFDButtonStripViewModel LeftButtons
        {
            get { return new MFDButtonStripViewModel(Model.LeftButtons); }
        }

        [NotNull]
        public MFDButtonStripViewModel RightButtons
        {
            get { return new MFDButtonStripViewModel(Model.RightButtons); }
        }

        [NotNull]
        public MFDButtonStripViewModel TopButtons
        {
            get { return new MFDButtonStripViewModel(Model.TopButtons); }
        }

        [NotNull]
        public MFDButtonStripViewModel BottomButtons
        {
            get { return new MFDButtonStripViewModel(Model.BottomButtons); }
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

        public double ScreenHeight
        {
            get { return Model.ScreenHeight; }
            set { Model.ScreenHeight = value; }
        }

        /// <summary>
        ///     The MFD model backing store.
        /// </summary>
        [NotNull]
        private readonly MultifunctionDisplay _mfd;

        /// <summary>
        /// Initializes a new instance of the <see cref="MFDViewModel"/> class.
        /// </summary>
        /// <param name="mfd">The multifunction display.</param>
        public MFDViewModel([NotNull] MultifunctionDisplay mfd)
        {
            _mfd = mfd;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MFDViewModel() : this(new MultifunctionDisplay())
        {
        }
    }

}