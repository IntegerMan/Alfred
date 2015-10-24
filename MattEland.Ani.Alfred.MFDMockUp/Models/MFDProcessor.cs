using System;
using System.Diagnostics;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A multifunction display processor. This class cannot be inherited.
    /// </summary>
    public sealed class MFDProcessor
    {
        [NotNull]
        private readonly MultifunctionDisplay _mfd;

        [CanBeNull]
        private static Random _randomizer;

        /// <summary>
        ///     Initializes a new instance of the MFDProcessor class.
        /// </summary>
        /// <param name="multifunctionDisplay"> The multifunction display. </param>
        public MFDProcessor([NotNull] MultifunctionDisplay multifunctionDisplay)
        {
            _mfd = multifunctionDisplay;
        }

        /// <summary>
        ///     Gets the current mode.
        /// </summary>
        /// <value>
        ///     The current mode.
        /// </value>
        public MFDMode CurrentMode { get; private set; }

        /// <summary>
        ///     Gets the multifunction display.
        /// </summary>
        /// <value>
        ///     The multifunction display.
        /// </value>
        [NotNull]
        public MultifunctionDisplay MFD
        {
            [DebuggerStepThrough]
            get
            { return _mfd; }
        }

        /// <summary>
        ///     Gets the random number generator.
        /// </summary>
        /// <value>
        ///     The randomizer.
        /// </value>
        public Random Randomizer
        {
            get
            {
                if (_randomizer == null)
                {
                    _randomizer = new Random();
                }

                return _randomizer;
            }
        }

        /// <summary>
        ///     Updates the MFD's state
        /// </summary>
        public void Update()
        {
            var processorResult = new MFDProcessorResult(this);

            _mfd.CurrentScreen.ProcessCurrentState(this, processorResult);

            CurrentMode = processorResult.RequestedMode;
        }
    }

}