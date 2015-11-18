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
    public sealed class MasterMode : IButtonClickListener
    {
        [NotNull]
        private readonly ButtonModel _systemButton;

        [NotNull]
        private readonly ButtonModel _alfredButton;

        [NotNull]
        private readonly ButtonModel _logButton;

        [NotNull]
        private readonly ButtonModel _performanceButton;

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
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MasterMode([NotNull] MultifunctionDisplay display)
        {
            Contract.Requires(display != null);
            Contract.Requires(display.ScreenProvider != null);

            IButtonClickListener listener = this;
            var screens = display.ScreenProvider;

            _systemButton = new NavigationButtonModel(screens.HomeScreen, listener);
            _alfredButton = new NavigationButtonModel(screens.AlfredScreen, listener);
            _logButton = new NavigationButtonModel(screens.LogScreen, listener);
            _performanceButton = new NavigationButtonModel(screens.PerformanceScreen, listener);

            // TODO: This will need to move to the next available mode
            _modeButton = new ButtonModel("MODE", listener);

        }

        /// <summary>
        ///     Gets screen change buttons.
        /// </summary>
        /// <param name="buttonProvider"> The button provider. </param>
        /// <param name="display"> The display. </param>
        /// <returns>
        ///     An enumerable of screen changebuttons.
        /// </returns>
        [NotNull, ItemNotNull]
        public IEnumerable<ButtonModel> GetScreenChangeButtons([NotNull] ButtonProvider buttonProvider,
            [NotNull] MultifunctionDisplay display)
        {
            Contract.Requires(buttonProvider != null);
            Contract.Requires(display != null);
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>() != null);
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>().All(b => b != null));

            yield return _systemButton;
            yield return _alfredButton;
            yield return _modeButton;
            yield return _performanceButton;
            yield return _logButton;
        }

        /// <summary>
        ///     Executes when a button is clicked.
        /// </summary>
        /// <param name="button"> The button. </param>
        public void OnButtonClicked(ButtonModel button)
        {
            // Pass the event on to any interested party
            ButtonClickListener?.OnButtonClicked(button);
        }
    }
}