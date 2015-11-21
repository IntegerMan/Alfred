using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes
{
    /// <summary>
    ///     The system MFD master mode. This class cannot be inherited.
    /// </summary>
    public sealed class SystemMasterMode : MasterModeBase
    {
        [NotNull]
        private readonly ButtonModel _systemButton;

        [NotNull]
        private readonly ButtonModel _alfredButton;

        [NotNull]
        private readonly ButtonModel _logButton;

        [NotNull]
        private readonly ButtonModel _performanceButton;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="display"> The display. </param>
        /// <param name="nextMode"> The next mode. </param>
        public SystemMasterMode([NotNull] MultifunctionDisplay display,
            [CanBeNull] MasterModeBase nextMode = null)
            : base(display, nextMode)
        {
            var screens = ScreenProvider;

            // Set up buttons
            _systemButton = new NavigationButtonModel(screens.HomeScreen, this);
            _alfredButton = new NavigationButtonModel(screens.AlfredScreen, this);
            _logButton = new NavigationButtonModel(screens.LogScreen, this);
            _performanceButton = new NavigationButtonModel(screens.PerformanceScreen, this);

        }

        /// <summary>
        ///     Gets the default screen when this master mode is switched to.
        /// </summary>
        /// <value>
        ///     The default screen.
        /// </value>
        [NotNull]
        public override ScreenModel DefaultScreen
        {
            get { return ScreenProvider.HomeScreen; }
        }

        /// <summary>
        ///     Gets screen change buttons.
        /// </summary>
        /// 
        /// <returns>
        ///     An enumerable of screen changebuttons.
        /// </returns>
        [NotNull, ItemNotNull]
        public override IEnumerable<ButtonModel> GetScreenChangeButtons()
        {
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>() != null);
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>().All(b => b != null));

            yield return _systemButton;
            yield return _alfredButton;
            yield return ModeSwitchButton;
            yield return _performanceButton;
            yield return _logButton;
        }

        /// <summary>
        ///     Gets the screen command buttons related to the current screen.
        /// </summary>
        /// <returns>
        /// An enumerable of screen command buttons.
        /// </returns>
        [NotNull, ItemNotNull]
        public override IEnumerable<ButtonModel> GetScreenCommandButtons()
        {
            yield break;
        }
    }
}