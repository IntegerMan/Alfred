using System.Collections;
using System.Collections.Generic;

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
        ///     The docked state of the button strip model.
        /// </summary>
        [NotNull]
        private readonly Observable<ButtonStripDock> _dock;

        /// <summary>
        ///     Initializes a new instance of the ButtonStripModel class.
        /// </summary>
        public ButtonStripModel() : this(ButtonStripDock.Top) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonStripModel"/> class.
        /// </summary>
        [UsedImplicitly]
        public ButtonStripModel(ButtonStripDock dock)
            : this(
                dock,
                new List<ButtonModel>
                {
                    new ButtonModel("BTN1", true),
                    new ButtonModel("BTN2"),
                    new ButtonModel("BTN3"),
                    new ButtonModel("BTN4"),
                    new ButtonModel("BTN5")
                })
        {

        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ButtonStripModel"/> class with the specified
        ///     docked state.
        /// </summary>
        /// <param name="dock"> The docked state. </param>
        public ButtonStripModel(ButtonStripDock dock, [CanBeNull] IEnumerable<ButtonModel> buttons = null)
        {
            _dock = new Observable<ButtonStripDock>(dock);
            _buttons = new ObservableList<ButtonModel>();

            if (buttons != null)
            {
                var index = 0;

                foreach (var button in buttons)
                {
                    // Set it to the appropriate index
                    button.Index = index++;

                    _buttons.Add(button);
                }
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
    }
}