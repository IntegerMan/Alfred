using System;

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
        private Random _randomizer;

        public MFDProcessor([NotNull] MultifunctionDisplay multifunctionDisplay)
        {
            _mfd = multifunctionDisplay;
        }

        /// <summary>
        ///     Updates the MFD's state
        /// </summary>
        public void Update()
        {
            var processorResult = new MFDProcessorResult(this);

            _mfd.CurrentScreen.ProcessCurrentState(this, processorResult);
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
    }

}