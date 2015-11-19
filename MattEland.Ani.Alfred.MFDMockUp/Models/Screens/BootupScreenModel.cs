using Assisticant.Fields;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data Model for the bootup screen.
    /// </summary>
    public sealed class BootupScreenModel : ScreenModel
    {
        private const double MaxProgressIncrement = 0.125;
        private const int DifficultyToIncrement = 3;

        [NotNull]
        private readonly Observable<string> _loadingMessage;

        [NotNull]
        private readonly Observable<MasterModeBase> _nextMode;

        /// <summary>
        ///     The loading progress.
        /// </summary>
        [NotNull]
        private readonly Observable<double> _progress;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="nextMode"> The next master mode. </param>
        public BootupScreenModel([CanBeNull] MasterModeBase nextMode) : base("BTUP")
        {
            _progress = new Observable<double>(0.0);
            _nextMode = new Observable<MasterModeBase>(nextMode);
            _loadingMessage = new Observable<string>("Standby");
        }

        /// <summary>
        ///     Gets or sets a message describing the loading.
        /// </summary>
        /// <value>
        ///     A message describing the loading.
        /// </value>
        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set { _loadingMessage.Value = value; }
        }

        /// <summary>
        ///     Gets or sets the master mode to activate after the load operation is completed.
        /// </summary>
        /// <value>
        ///     The next master mode.
        /// </value>
        [CanBeNull]
        public MasterModeBase NextMode
        {
            get { return _nextMode; }
            set { _nextMode.Value = value; }
        }

        /// <summary>
        ///     Gets or sets the progress of the loading operation as a value from 0.0 to 1.0.
        /// </summary>
        /// <value>
        ///     The progress.
        /// </value>
        public double Progress
        {
            get { return _progress; }
            set { _progress.Value = value; }
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState([NotNull] MFDProcessor processor,
            [NotNull] MFDProcessorResult processorResult)
        {

            // Only update the progress some update cycles to give it the appearance of realism / noise
            if (processor.Randomizer.Next(DifficultyToIncrement) == 0)
            {
                var incrementAmount = processor.Randomizer.NextDouble() * MaxProgressIncrement;
                Progress += incrementAmount;
            }

            // If we've completed, tell the application we're ready to move on.
            if (Progress >= 1.0)
            {
                processorResult.RequestedMasterMode = NextMode;

                if (NextMode != null)
                {
                    processorResult.RequestedScreen = NextMode.DefaultScreen;
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the sensor of interest indicator is shown.
        /// </summary>
        /// <value>
        ///     true if the screen shows the sensor of interest indicator, false if not.
        /// </value>
        public override bool ShowSensorOfInterestIndicator { get { return false; } }
    }

}