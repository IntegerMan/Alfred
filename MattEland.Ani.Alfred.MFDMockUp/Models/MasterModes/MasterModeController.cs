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
        private readonly MultifunctionDisplay _display;

        [NotNull]
        private readonly SystemMasterMode _systemMasterMode;

        [NotNull]
        private readonly BootupMasterMode _bootupMasterMode;

        [NotNull, ItemNotNull]
        private readonly Observable<MasterModeBase> _currentMasterMode;

        [NotNull]
        private readonly ProgrammingMasterMode _programmingMasterMode;

        [NotNull]
        private readonly OrganizerMasterMode _organizerMasterMode;

        [NotNull]
        private readonly MasterModeCycler _mainModesCycler;

        /// <summary>
        ///     Initializes a new instance of the MasterModeController class.
        /// </summary>
        /// <param name="display"> The multifunction display. </param>
        public MasterModeController([NotNull] MultifunctionDisplay display)
        {
            Contract.Requires(display != null);

            _display = display;
            _systemMasterMode = new SystemMasterMode(display);
            _bootupMasterMode = new BootupMasterMode(display, _systemMasterMode);
            _programmingMasterMode = new ProgrammingMasterMode(display);
            _organizerMasterMode = new OrganizerMasterMode(display);

            _mainModesCycler = new MasterModeCycler(display, _systemMasterMode, _organizerMasterMode, _programmingMasterMode);

            _currentMasterMode = new Observable<MasterModeBase>(_bootupMasterMode);
        }

        /// <summary>
        ///     Gets the multifunction display.
        /// </summary>
        /// <value>
        ///     The display.
        /// </value>
        [NotNull]
        public MultifunctionDisplay Display
        {
            [DebuggerStepThrough]
            get
            { return _display; }
        }

        /// <summary>
        ///     Gets the system master mode.
        /// </summary>
        /// <value>
        ///     The system master mode.
        /// </value>
        [NotNull]
        public SystemMasterMode SystemMasterMode
        {
            [DebuggerStepThrough]
            get
            { return _systemMasterMode; }
        }

        /// <summary>
        ///     Gets the bootup master mode.
        /// </summary>
        /// <value>
        ///     The bootup master mode.
        /// </value>
        [NotNull]
        public BootupMasterMode BootupMasterMode
        {
            [DebuggerStepThrough]
            get
            { return _bootupMasterMode; }
        }

        /// <summary>
        ///     Gets the programming master mode.
        /// </summary>
        /// <value>
        ///     The programming master mode.
        /// </value>
        [NotNull]
        public ProgrammingMasterMode ProgrammingMasterMode
        {
            [DebuggerStepThrough]
            get
            { return _programmingMasterMode; }
        }

        /// <summary>
        ///     Gets the organizer master mode.
        /// </summary>
        /// <value>
        ///     The organizer master mode.
        /// </value>
        [NotNull]
        public OrganizerMasterMode OrganizerMasterMode
        {
            [DebuggerStepThrough]
            get
            { return _organizerMasterMode; }
        }

        /// <summary>
        ///     Gets the main modes cycler used to cycle through the various modes.
        /// </summary>
        /// <value>
        ///     The main modes cycler.
        /// </value>
        [NotNull]
        public MasterModeCycler MainModesCycler
        {
            [DebuggerStepThrough]
            get
            { return _mainModesCycler; }
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