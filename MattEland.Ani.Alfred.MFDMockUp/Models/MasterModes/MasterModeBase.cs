using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Assisticant.Collections;

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

        [CanBeNull]
        private ICollection<ButtonModel> _commandButtons;

        [CanBeNull]
        private ICollection<ButtonModel> _navButtons;

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

            NavButtons = new ObservableList<ButtonModel>();
            CommandButtons = BuildDefaultCommandButtons();

        }

        /// <summary>
        ///     Builds a placeholder button suitable for rendering an empty space of feature not yet
        ///     implemented.
        /// </summary>
        /// <param name="buttonText"> The text to use in the button. </param>
        /// <returns>
        ///     The button.
        /// </returns>
        [NotNull]
        protected static ButtonModel BuildPlaceholderButton(string buttonText)
        {
            Contract.Ensures(Contract.Result<ButtonModel>() != null);

            return new ButtonModel(buttonText ?? string.Empty);
        }

        /// <summary>
        ///     Builds an empty button.
        /// </summary>
        /// <returns>
        ///     A ButtonModel.
        /// </returns>
        [NotNull]
        protected static ButtonModel BuildEmptyButton()
        {
            Contract.Ensures(Contract.Result<ButtonModel>() != null);

            return BuildPlaceholderButton(string.Empty);
        }

        /// <summary>
        ///     Creates an empty button list.
        /// </summary>
        /// <returns>
        ///     The new empty button list.
        /// </returns>
        [NotNull]
        protected static ICollection<ButtonModel> BuildEmptyButtonList()
        {
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>() != null);

            return new ObservableList<ButtonModel>
                   {
                       BuildEmptyButton(),
                       BuildEmptyButton(),
                       BuildEmptyButton(),
                       BuildEmptyButton(),
                       BuildEmptyButton()
                   };
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(ModeSwitchButton != null);
            Contract.Invariant(CommandButtons != null);
            Contract.Invariant(NavButtons != null);
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
        ///     Gets or sets the command buttons.
        /// </summary>
        /// <value>
        ///     The command buttons.
        /// </value>
        protected ICollection<ButtonModel> CommandButtons
        {
            get { return _commandButtons; }
            set { _commandButtons = value ?? BuildEmptyButtonList(); }
        }

        protected ICollection<ButtonModel> NavButtons
        {
            get { return _navButtons; }
            set { _navButtons = value ?? BuildEmptyButtonList(); }
        }

        /// <summary>
        ///     Gets or sets the master mode cycler.
        /// </summary>
        /// <value>
        ///     The master mode cycler.
        /// </value>
        [CanBeNull]
        public MasterModeCycler MasterModeCycler { get; set; }

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
        ///     Gets the screen command buttons related to the current screen.
        /// </summary>
        /// <returns>
        /// An enumerable of screen command buttons.
        /// </returns>
        [NotNull, ItemNotNull]
        public virtual IEnumerable<ButtonModel> GetScreenCommandButtons()
        {
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>() != null);
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>().All(b => b != null));
            Contract.Ensures(CommandButtons != null);

            if (CommandButtons == null)
            {
                CommandButtons = BuildEmptyButtonList();
            }

            /* Using yield here will build different collections each go
               resulting in binding not functioning. */

            return CommandButtons;
        }

        /// <summary>
        ///     Gets screen change buttons.
        /// </summary>
        /// 
        /// <returns>
        ///     An enumerable of screen changebuttons.
        /// </returns>
        [NotNull, ItemNotNull]
        public virtual IEnumerable<ButtonModel> GetScreenChangeButtons()
        {
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>() != null);
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>().All(b => b != null));
            Contract.Ensures(NavButtons != null);

            if (NavButtons == null)
            {
                NavButtons = BuildEmptyButtonList();
            }

            /* Using yield here will build different collections each go
               resulting in binding not functioning. */

            return NavButtons;
        }

        /// <summary>
        ///     Builds a default command buttons with 4 empty buttons with a mode switch button in the center.
        /// </summary>
        /// <returns>
        ///     The command buttons
        /// </returns>
        [NotNull]
        protected IList<ButtonModel> BuildDefaultCommandButtons()
        {
            Contract.Ensures(Contract.Result<IList<ButtonModel>>() != null);
            Contract.Ensures(Contract.Result<IList<ButtonModel>>().All(b => b != null));

            return new ObservableList<ButtonModel>
                   {
                       BuildEmptyButton(),
                       BuildEmptyButton(),
                       ModeSwitchButton,
                       BuildEmptyButton(),
                       BuildEmptyButton()
                   };
        }
    }

}