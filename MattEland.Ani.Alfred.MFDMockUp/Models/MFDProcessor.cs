using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using MattEland.Common.Annotations;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A multifunction display processor. This class cannot be inherited.
    /// </summary>
    public sealed class MFDProcessor : IHasContainer<IObjectContainer>
    {
        [NotNull]
        private readonly MultifunctionDisplay _mfd;

        [CanBeNull]
        private Random _randomizer;

        /// <summary>
        ///     Initializes a new instance of the MFDProcessor class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="multifunctionDisplay"> The multifunction display. </param>
        public MFDProcessor([NotNull] IObjectContainer container,
            [NotNull] MultifunctionDisplay multifunctionDisplay)
        {
            Contract.Requires(container != null);

            _mfd = multifunctionDisplay;

            Container = container;
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
        [NotNull]
        public Random Randomizer
        {
            get
            {
                if (_randomizer == null)
                {
                    _randomizer = Container.Provide<Random>();
                    _randomizer.RegisterAsProvidedInstance(typeof(Random), Container);
                }

                return _randomizer;
            }
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IObjectContainer Container { get; }

        /// <summary>
        ///     Updates the MFD's state
        /// </summary>
        public void Update()
        {
            var processorResult = new MFDProcessorResult(this);

            // Allow the screen to interact with the result
            _mfd.CurrentScreen.ProcessCurrentState(this, processorResult);

            // Update the buttons based on the current state
            _mfd.ButtonProvider.ProcessCurrentState(this, processorResult);

            CurrentMode = processorResult.RequestedMode;

            // Set the current screen to the new screen
            if (processorResult.RequestedScreen != null)
            {
                _mfd.CurrentScreen = processorResult.RequestedScreen;
            }
        }
    }

}