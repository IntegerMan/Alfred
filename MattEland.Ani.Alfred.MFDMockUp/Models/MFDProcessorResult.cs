﻿using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
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
        public MFDProcessorResult(MFDProcessor processor)
        {
            _processor = processor;
            CurrentMode = processor.CurrentMode;
            CurrentScreen = processor.MFD.CurrentScreen;
        }

        /// <summary>
        ///     Gets the current mode.
        /// </summary>
        /// <value>
        ///     The current mode.
        /// </value>
        public MFDMode CurrentMode { get; }

        /// <summary>
        ///     Gets or sets the requested mode. The <see cref="MFDProcessor"/> will attempt to
        ///     transition to this mode at the end of the processor frame.
        /// </summary>
        /// <value>
        ///     The requested mode.
        /// </value>
        public MFDMode RequestedMode { get; set; }

        /// <summary>
        ///     Gets the current screen.
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

    }
}