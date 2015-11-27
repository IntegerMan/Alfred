using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes
{
    /// <summary>
    ///     A master mode. This represents the context for a multifunction display and governs the
    ///     available screens and button configurations.
    /// </summary>
    public abstract class MasterModeBase : IButtonClickListener
    {
        /// <summary>
        ///     The mode switch button.
        /// </summary>
        [NotNull]
        private readonly ButtonModel _modeSwitchButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterModeBase"/> class.
        /// </summary>
        protected MasterModeBase([NotNull] MultifunctionDisplay display)
        {
            Contract.Requires(display != null);
            Contract.Requires(display.ScreenProvider != null);

            IButtonClickListener listener = this;
            ScreenProvider = display.ScreenProvider;

            // TODO: This will need to move to the next available mode
            _modeSwitchButton = new ModeSwitchButtonModel("MODE", listener);

        }

        /// <summary>
        /// Builds a placeholder button suitable for rendering an empty space.
        /// </summary>
        /// <param name="buttonText">The text to use in the button. This defaults to empty.</param>
        /// <returns>The button.</returns>
        [NotNull]
        protected static ButtonModel BuildPlaceholderButton(string buttonText = "")
        {
            Contract.Ensures(Contract.Result<ButtonModel>() != null);

            return new ButtonModel(buttonText);
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(ModeSwitchButton != null);
            Contract.Invariant(ScreenProvider != null);
            Contract.Invariant(DefaultScreen != null);
            Contract.Invariant(GetScreenChangeButtons() != null);
            Contract.Invariant(GetScreenChangeButtons().All(b => b != null));
            Contract.Invariant(GetScreenCommandButtons() != null);
            Contract.Invariant(GetScreenCommandButtons().All(b => b != null));
        }

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
            get { return _modeSwitchButton; }
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
        ///     Gets the default screen when this master mode is switched to.
        /// </summary>
        /// <value>
        ///     The default screen.
        /// </value>
        [NotNull]
        public abstract ScreenModel DefaultScreen
        {
            get;
        }

        /// <summary>
        ///     Gets the text to display on the screen identifying the current mdoe.
        /// </summary>
        /// <value>
        ///     The screen identification text.
        /// </value>
        public abstract string ScreenIdentificationText { get; }

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
        /// <returns>
        ///     An enumerable of screen change buttons.
        /// </returns>
        [NotNull, ItemNotNull]
        public abstract IEnumerable<ButtonModel> GetScreenChangeButtons();

        /// <summary>
        ///     Gets the screen command buttons related to the current screen.
        /// </summary>
        /// <returns>
        /// An enumerable of screen command buttons.
        /// </returns>
        [NotNull, ItemNotNull]
        public abstract IEnumerable<ButtonModel> GetScreenCommandButtons();
    }

}