using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
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

        [NotNull, ItemNotNull]
        private readonly Queue<ButtonModel> _buttonPresses;

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
            _buttonPresses = new Queue<ButtonModel>();
        }

        /// <summary>
        ///     Gets the current master mode.
        /// </summary>
        /// <value>
        ///     The current mode.
        /// </value>
        public MasterModeBase CurrentMasterMode { get; private set; }

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
        internal void Update()
        {

            // Build the result object that will be used to update the application
            var processorResult = new MFDProcessorResult(this);

            // Handle all button presses that have occurred since the last update
            ProcessButtonPresses(processorResult);

            // Allow the screen to interact with the result
            _mfd.CurrentScreen.ProcessCurrentState(this, processorResult);

            // Update the buttons based on the current state
            _mfd.ButtonProvider.ProcessCurrentState(this, processorResult);

            HandleProcessorFrameResults(processorResult);
        }

        private void HandleProcessorFrameResults([NotNull] MFDProcessorResult processorResult)
        {
            // Update the mode
            CurrentMasterMode = processorResult.RequestedMasterMode;

            // Set the current screen to the new screen
            if (processorResult.RequestedScreen != null)
            {
                _mfd.CurrentScreen = processorResult.RequestedScreen;
            }
        }

        /// <summary>
        ///     Process the button presses.
        /// </summary>
        /// <param name="processorResult"> The processor result. </param>
        private void ProcessButtonPresses([NotNull] MFDProcessorResult processorResult)
        {
            const int MaxPressesPerFrame = 5;

            if (!_buttonPresses.Any()) return;

            // If any button was pressed, the MFD is now the SOI
            _mfd.IsSensorOfInterest = true;

            // Handle each button press
            var pressesHandled = 0;

            // Keep handling until we've hit our frame limit or run out of items in the queue
            do
            {
                var button = _buttonPresses.Dequeue();

                // Allow the button to influence the result of this processor frame
                button.ProcessCommand(this, processorResult);

                pressesHandled += 1;

            } while (_buttonPresses.Any() && pressesHandled <= MaxPressesPerFrame);
        }

        internal void EnqueueButtonPress(ButtonModel button)
        {
            _buttonPresses.Enqueue(button);
        }
    }

}