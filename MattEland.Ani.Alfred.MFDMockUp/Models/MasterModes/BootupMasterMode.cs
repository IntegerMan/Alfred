using System.Collections.Generic;
using System.Diagnostics.Contracts;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes
{
    /// <summary>
    ///     The bootup master mode. This class cannot be inherited.
    /// </summary>
    public sealed class BootupMasterMode : MasterModeBase
    {
        [NotNull]
        private readonly BootupScreenModel _bootScreen;

        /// <summary>
        ///     Initializes a new instance of the BootupMasterMode class.
        /// </summary>
        /// <param name="display"> The display. </param>
        /// <param name="nextMasterMode"> The next master mode. </param>
        public BootupMasterMode([NotNull] MultifunctionDisplay display,
                                [NotNull] MasterModeBase nextMasterMode) : base(display)
        {
            Contract.Requires(display != null);
            Contract.Requires(nextMasterMode != null);

            _bootScreen = new BootupScreenModel(nextMasterMode);
        }

        /// <summary>
        ///     Gets the default screen when this master mode is switched to.
        /// </summary>
        /// <value>
        ///     The default screen.
        /// </value>
        public override ScreenModel DefaultScreen { get { return _bootScreen; } }

        /// <summary>
        ///     Gets screen change buttons.
        /// </summary>
        /// <returns>
        ///     An enumerable of screen change buttons.
        /// </returns>
        public override IEnumerable<ButtonModel> GetScreenChangeButtons()
        {
            yield break;
        }

        /// <summary>
        ///     Gets the screen command buttons related to the current screen.
        /// </summary>
        /// <returns>
        /// An enumerable of screen command buttons.
        /// </returns>
        public override IEnumerable<ButtonModel> GetScreenCommandButtons()
        {
            yield break;
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(_bootScreen != null);
        }

    }
}