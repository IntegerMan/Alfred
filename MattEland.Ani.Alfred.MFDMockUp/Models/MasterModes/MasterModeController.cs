// ---------------------------------------------------------
// MasterModeController.cs
// 
// Created on:      11/20/2015 at 10:46 PM
// Last Modified:   11/20/2015 at 10:46 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.Contracts;

using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes
{
    /// <summary>
    ///     A controller for managing master modes. This class cannot be inherited.
    /// </summary>
    public sealed class MasterModeController
    {
        [NotNull]
        private readonly MultifunctionDisplay _mfd;

        [NotNull]
        private readonly SystemMasterMode _systemMasterMode;

        [NotNull]
        private readonly BootupMasterMode _bootupMasterMode;

        [NotNull, ItemNotNull]
        private readonly Observable<MasterModeBase> _currentMasterMode;

        /// <summary>
        ///     Initializes a new instance of the MasterModeController class.
        /// </summary>
        /// <param name="mfd"> The multifunction display. </param>
        public MasterModeController([NotNull] MultifunctionDisplay mfd)
        {
            Contract.Requires(mfd != null);

            _mfd = mfd;
            _systemMasterMode = new SystemMasterMode(mfd);
            _bootupMasterMode = new BootupMasterMode(mfd, _systemMasterMode);

            _currentMasterMode = new Observable<MasterModeBase>(_bootupMasterMode);
        }

        /// <summary>
        ///     Gets the current master mode.
        /// </summary>
        /// <value>
        ///     The current master mode.
        /// </value>
        [NotNull]
        public MasterModeBase CurrentMasterMode
        {
            [DebuggerStepThrough]
            get
            { return _currentMasterMode.Value; }
            set
            {
                Contract.Requires(value != null);
                Contract.Ensures(CurrentMasterMode == value);

                _currentMasterMode.Value = value;
            }
        }

    }
}