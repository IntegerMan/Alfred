using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="display"> The display. </param>
        public SystemMasterMode([NotNull] MultifunctionDisplay display)
            : base(display)
        {
            var screens = ScreenProvider;

            // Set up buttons
            SystemButton = new NavigationButtonModel(screens.HomeScreen, this);
            AlfredButton = new NavigationButtonModel(screens.AlfredScreen, this);
            LogButton = new NavigationButtonModel(screens.LogScreen, this);
            PerformanceButton = new NavigationButtonModel(screens.PerformanceScreen, this);

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

            yield return SystemButton;
            yield return AlfredButton;
            yield return PerformanceButton;
            yield return LogButton;
            yield return EmptyButton;
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
            yield return EmptyButton;
            yield return EmptyButton;
            yield return ModeSwitchButton;
            yield return EmptyButton;
            yield return EmptyButton;

        }

        /// <summary>
        ///     Gets the system button.
        /// </summary>
        /// <value>
        ///     The system button.
        /// </value>
        [NotNull]
        public ButtonModel SystemButton
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets the alfred button.
        /// </summary>
        /// <value>
        ///     The alfred button.
        /// </value>
        [NotNull]
        public ButtonModel AlfredButton
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets the log button.
        /// </summary>
        /// <value>
        ///     The log button.
        /// </value>
        [NotNull]
        public ButtonModel LogButton
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets the performance button.
        /// </summary>
        /// <value>
        ///     The performance button.
        /// </value>
        [NotNull]
        public ButtonModel PerformanceButton
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets the text to display on the screen identifying the current mdoe.
        /// </summary>
        /// <value>
        ///     The screen identification text.
        /// </value>
        public override string ScreenIdentificationText { get { return "SYS"; } }
    }
}