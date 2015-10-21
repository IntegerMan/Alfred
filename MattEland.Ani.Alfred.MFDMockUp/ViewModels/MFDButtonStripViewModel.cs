using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     A ViewModel for a multifunction display's button strip bordering one side of the screen.
    ///     This class cannot be inherited.
    /// </summary>
    public sealed class MFDButtonStripViewModel
    {
        [NotNull]
        private readonly ButtonStripModel _model;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MFDButtonStripViewModel"/> class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public MFDButtonStripViewModel([NotNull] ButtonStripModel model)
        {
            _model = model;
        }

        /// <summary>
        ///     Gets the docked state of the button strip.
        /// </summary>
        /// <value>
        ///     The docked state of the button strip.
        /// </value>
        [UsedImplicitly]
        public ButtonStripDock Dock
        {
            get { return _model.Dock; }
        }

        /// <summary>
        ///     Gets the button view models associated with each button in the button strip.
        /// </summary>
        /// <value>
        ///     The buttons.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<ButtonViewModel> Buttons
        {
            get { return _model.Buttons.Select(b => new ButtonViewModel(b, this)); }
        }

        /// <summary>
        ///     Gets a collection of buttons and the separators that should be rendered for those buttons.
        /// </summary>
        /// <value>
        ///     The buttons and separators.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable ButtonsAndSeparators
        {
            get
            {
                foreach (var button in Buttons)
                {
                    yield return button;

                    if (button.SeparatorVisibility == Visibility.Visible)
                    {
                        yield return new SeparatorViewModel(new SeparatorModel(true));
                    }
                }
            }
        }
    }
}