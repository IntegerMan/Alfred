using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    /// <summary>
    ///     A master mode. This represents the context for a multifunction display and governs the
    ///     available screens and button configurations.
    /// </summary>
    public abstract class MasterModeBase : IButtonClickListener
    {

        [NotNull]
        private readonly ButtonModel _modeButton;

        /// <summary>
        ///     Gets or sets the button click listener.
        /// </summary>
        /// <value>
        ///     The button click listener.
        /// </value>
        [CanBeNull]
        public IButtonClickListener ButtonClickListener { get; set; }

        /// <summary>
        ///     Gets the mode switch button used to cycle between available modes.
        /// </summary>
        /// <value>
        ///     The mode switch button.
        /// </value>
        [NotNull]
        protected ButtonModel ModeSwitchButton
        {
            get { return _modeButton; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected MasterModeBase([NotNull] MultifunctionDisplay display)
        {
            Contract.Requires(display != null);
            Contract.Requires(display.ScreenProvider != null);

            IButtonClickListener listener = this;
            ScreenProvider = display.ScreenProvider;

            // TODO: This will need to move to the next available mode
            _modeButton = new ButtonModel("MODE", listener);

        }

        /// <summary>
        ///     Gets the screen provider associated with this instance.
        /// </summary>
        /// <value>
        ///     The screen provider.
        /// </value>
        [NotNull]
        protected ScreenProvider ScreenProvider { get; }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(ModeSwitchButton != null);
            Contract.Invariant(ScreenProvider != null);
        }

        /// <summary>
        ///     Executes when a button is clicked.
        /// </summary>
        /// <remarks>
        ///     Overriding types, be sure to call the base implementation at the end of the method block.
        /// </remarks>
        /// <param name="button"> The button. </param>
        public virtual void OnButtonClicked(ButtonModel button)
        {
            // Pass the event on to any interested party
            ButtonClickListener?.OnButtonClicked(button);
        }

        /// <summary>
        ///     Gets screen change buttons.
        /// </summary>
        /// 
        /// <returns>
        ///     An enumerable of screen changebuttons.
        /// </returns>
        public abstract IEnumerable<ButtonModel> GetScreenChangeButtons();
    }

}