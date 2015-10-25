using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Assisticant.Collections;
using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A data model for a button strip on the side of a multifunction display. This class cannot
    ///     be inherited.
    /// </summary>
    public sealed class ButtonStripModel
    {
        /// <summary>
        ///     The button provider.
        /// </summary>
        [NotNull]
        private readonly ButtonProvider _provider;

        /// <summary>
        ///     The docked state of the button strip model.
        /// </summary>
        [NotNull]
        private readonly Observable<ButtonStripDock> _dock;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ButtonStripModel"/> class with the specified
        ///     docked state.
        /// </summary>
        /// <param name="provider"> The button provider. </param>
        /// <param name="dock"> The docked state. </param>
        /// <param name="buttons"> The buttons. </param>
        public ButtonStripModel([NotNull] ButtonProvider provider,
            ButtonStripDock dock,
            [CanBeNull] params ButtonModel[] buttons)
        {
            _provider = provider;
            _dock = new Observable<ButtonStripDock>(dock);
            _buttons = new ObservableList<ButtonModel>();

            SetButtons(buttons?.ToArray());
        }

        /// <summary>
        ///     Sets the buttons to the specified set of buttons.
        /// </summary>
        /// <param name="buttons"> The buttons. </param>
        public void SetButtons([CanBeNull, ItemNotNull] params ButtonModel[] buttons)
        {
            // Ensure we don't get too many buttons
            _buttons.Clear();

            if (buttons == null) return;

            var index = 0;

            foreach (var button in buttons)
            {
                // Set it to the appropriate index
                button.Index = index++;

                _buttons.Add(button);
            }
        }

        /// <summary>
        ///     Gets or sets the docked state of the button strip.
        /// </summary>
        /// <value>
        ///     The docked state.
        /// </value>
        public ButtonStripDock Dock
        {
            get { return _dock; }
            set { _dock.Value = value; }
        }

        /// <summary>
        ///     The buttons.
        /// </summary>
        [NotNull]
        private readonly ObservableList<ButtonModel> _buttons;

        /// <summary>
        ///     Gets the buttons within this button strip.
        /// </summary>
        /// <value>
        ///     The buttons.
        /// </value>
        public IEnumerable<ButtonModel> Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        ///     Sets the collection to a group of empty buttons.
        /// </summary>
        /// <param name="numButtons"> The number of buttons to populate. </param>
        public void SetEmptyButtons(int numButtons)
        {
            var list = new List<ButtonModel>(numButtons);

            for (int i = 0; i < numButtons; i++)
            {
                list.Add(new ButtonModel(string.Empty, _provider, false, i));
            }

            SetButtons(list.ToArray());
        }
    }
}