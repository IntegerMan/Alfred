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
        /// Initializes a new instance of the <see cref="ButtonStripModel"/> class.
        /// </summary>
        [UsedImplicitly]
        public ButtonStripModel() : this(ButtonStripDock.Top)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ButtonStripModel"/> class with the specified
        ///     docked state.
        /// </summary>
        /// <param name="dock"> The docked state. </param>
        public ButtonStripModel(ButtonStripDock dock)
        {
            _dock = new Observable<ButtonStripDock>(dock);
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
    }
}