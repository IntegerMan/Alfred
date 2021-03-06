﻿using System.Diagnostics.Contracts;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     Encapsulates the result of a single processing frame of a multifunction display processor. 
    /// </summary>
    public sealed class MFDProcessorResult
    {
        /// <summary>
        ///     The processor.
        /// </summary>
        [NotNull]
        private readonly MFDProcessor _processor;

        /// <summary>
        ///     Initializes a new instance of the MFDProcessorResult class.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        public MFDProcessorResult([NotNull] MFDProcessor processor)
        {
            Contract.Requires(processor != null);
            Contract.Requires(processor.MFD != null);
            Contract.Requires(processor.CurrentMasterMode != null);

            _processor = processor;

            CurrentMasterMode = processor.CurrentMasterMode;
            RequestedMasterMode = processor.CurrentMasterMode;

            CurrentScreen = processor.MFD.CurrentScreen;
            RequestedScreen = processor.MFD.CurrentScreen;
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(CurrentMasterMode != null);
            Contract.Invariant(CurrentScreen != null);
        }


        /// <summary>
        ///     Gets the current mode.
        /// </summary>
        /// <value>
        ///     The current mode.
        /// </value>
        public MasterModeBase CurrentMasterMode { get; }

        /// <summary>
        ///     Gets or sets the requested mode. The <see cref="MFDProcessor"/> will attempt to
        ///     transition to this mode at the end of the processor frame.
        /// </summary>
        /// <value>
        ///     The requested mode.
        /// </value>
        public MasterModeBase RequestedMasterMode { get; set; }

        /// <summary>
        ///     Gets the current screen model.
        /// </summary>
        /// <value>
        ///     The current screen.
        /// </value>
        [NotNull]
        public ScreenModel CurrentScreen { get; }

        /// <summary>
        ///     Gets or sets the requested screen.
        /// </summary>
        /// <value>
        ///     The requested screen.
        /// </value>
        [CanBeNull]
        public ScreenModel RequestedScreen { get; set; }

        /// <summary>
        ///     Gets the current screen view model.
        /// </summary>
        /// <value>
        ///     The current screen view model.
        /// </value>
        public ScreenViewModel CurrentScreenViewModel
        {
            get { return CurrentScreen.GetViewModelFor(_processor.MFD); }
        }

    }
}