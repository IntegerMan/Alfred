using System.Collections.Generic;
using System.Linq;

using Assisticant.Collections;
using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    /// <summary>
    ///     A data model for a button strip on the side of a multifunction display. This class cannot
    ///     be inherited.
    /// </summary>
    public sealed class ButtonStripModel
    {

        /// <summary>
        ///     The buttons.
        /// </summary>
        [NotNull]
        private readonly ObservableList<ButtonModel> _buttons;

        /// <summary>
        ///     The docked state of the button strip model.
        /// </summary>
        [NotNull]
        private readonly Observable<ButtonStripDock> _dock;

        [NotNull]
        private readonly Observable<int> _expectedButtons = new Observable<int>(5);
        /// <summary>
        ///     The button provider.
        /// </summary>
        [NotNull]
        private readonly ButtonProvider _provider;

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
        ///     Gets the buttons within this button strip.
        /// </summary>
        /// <value>
        ///     The buttons.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<ButtonModel> Buttons
        {
            get { return _buttons; }
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
        ///     Gets or sets the expected buttons.
        /// </summary>
        /// <value>
        ///     The expected buttons.
        /// </value>
        public int ExpectedButtons
        {
            get { return _expectedButtons; }
            set { _expectedButtons.Value = value; }
        }

        /// <summary>
        ///     Sets the collection to a group of empty buttons.
        /// </summary>
        /// <param name="numButtons"> The number of buttons to populate. </param>
        public void SetEmptyButtons(int numButtons)
        {
            var list = BuildEmptyButtons(numButtons);

            SetButtons(list.ToArray());
        }

        /// <summary>
        ///     Creates an empty buttons collection.
        /// </summary>
        /// <param name="numButtons"> The number of buttons to populate. </param>
        /// <returns>
        ///     The new collection of empty buttons.
        /// </returns>
        [NotNull, ItemNotNull]
        internal static IEnumerable<ButtonModel> BuildEmptyButtons(int numButtons)
        {
            var list = new List<ButtonModel>(numButtons);

            for (int i = 0; i < numButtons; i++)
            {
                list.Add(BuildEmptyButton(i));
            }
            return list;
        }

        /// <summary>
        ///     Sets the buttons to the specified set of buttons.
        /// </summary>
        /// <param name="buttons"> The buttons. </param>
        internal void SetButtons([CanBeNull, ItemCanBeNull] IEnumerable<ButtonModel> buttons)
        {
            SetButtons(buttons?.ToArray());
        }

        /// <summary>
        ///     Sets the buttons to the specified set of buttons.
        /// </summary>
        /// <param name="buttons"> The buttons. </param>
        internal void SetButtons([CanBeNull, ItemCanBeNull] params ButtonModel[] buttons)
        {
            var index = 0;

            // Check to see if the contents of buttons matches _buttons
            if (buttons != null && buttons.Length == _buttons.Count)
            {
                var isMatch = true;

                foreach (var button in buttons)
                {
                    if (button != _buttons[index++])
                    {
                        isMatch = false;
                    }
                }

                // Do nothing if this is the same list as we're using now
                if (isMatch) return;
            }

            // Ensure we don't get too many buttons
            _buttons.Clear();

            if (buttons == null)
            {
                buttons = BuildEmptyButtons(ExpectedButtons).ToArray();
            }

            index = 0;

            var toAdd = buttons.ToList();

            if (toAdd.Count < ExpectedButtons)
            {
                for (int i = toAdd.Count; i < ExpectedButtons; i++)
                {
                    toAdd.Add(BuildEmptyButton(i));
                }
            }

            foreach (var button in toAdd)
            {
                // Allow for null buttons to be passed in. These represent spacers
                var addButton = button ?? BuildEmptyButton();

                // Set it to the appropriate index
                addButton.Index = index++;
                addButton.ClickListener = _provider;

                _buttons.Add(addButton);
            }

        }

        /// <summary>
        ///     Builds an empty button.
        /// </summary>
        /// <param name="index"> Zero-based index of the. </param>
        /// <returns>
        ///     A ButtonModel.
        /// </returns>
        [NotNull]
        private static ButtonModel BuildEmptyButton(int index = 0)
        {
            // The ClickListener will be added later
            return new ButtonModel(string.Empty, null, index);
        }
    }
}