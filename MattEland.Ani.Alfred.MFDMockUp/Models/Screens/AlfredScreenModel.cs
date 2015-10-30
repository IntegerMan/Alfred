using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;

using Assisticant.Fields;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Common;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data model for the Alfred status screen. This class cannot be inherited.
    /// </summary>
    public sealed class AlfredScreenModel : ScreenModel
    {

        [NotNull]
        private readonly Observable<string> _statusText;

        [NotNull]
        private readonly Observable<bool> _isOnline;

        [NotNull, ItemCanBeNull]
        private readonly IEnumerable<ButtonModel> _rightButtons;

        /// <summary>
        ///     Initializes a new instance of the AlfredScreenModel class.
        /// </summary>
        public AlfredScreenModel([NotNull] AlfredApplication alfredApplication) : base("ALF")
        {
            Contract.Requires(alfredApplication != null);

            // Hook up to Alfred's property changed notifications since Alfred doesn't use Assisticant
            AlfredApplication = alfredApplication;
            AlfredApplication.PropertyChanged += OnAlfredPropertyChanged;

            // Create observables
            _statusText = new Observable<string>(AlfredApplication.Status.ToString());
            _isOnline = new Observable<bool>(AlfredApplication.IsOnline);

            // Create buttons
            var powerButton = new ActionButtonModel("PWR", ToggleAlfredPower, () => AlfredApplication.IsOnline);

            // Build out the right button strip with some spacer items
            _rightButtons = new List<ButtonModel>
                            {
                                ButtonStripModel.BuildEmptyButton(0),
                                ButtonStripModel.BuildEmptyButton(1),
                                ButtonStripModel.BuildEmptyButton(2),
                                powerButton,
                                ButtonStripModel.BuildEmptyButton(4)
                            };
        }

        /// <summary>
        ///     Responds to property change notifications within the Alfred application.
        /// </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        private void OnAlfredPropertyChanged([CanBeNull] object sender, [NotNull] PropertyChangedEventArgs e)
        {
            // Empty string / null notifications relate to all properties
            var allProps = e.PropertyName.IsEmpty();

            // If it's a property we care about, update our Assisticant fields
            if (allProps || e.PropertyName.Matches(nameof(AlfredApplication.Status)) ||
                e.PropertyName.Matches(nameof(AlfredApplication.IsOnline)))
            {
                _statusText.Value = AlfredApplication.Status.ToString();
                _isOnline.Value = AlfredApplication.IsOnline;
            }
        }

        /// <summary>
        ///     Gets the Alfred application.
        /// </summary>
        /// <value>
        ///     The Alfred application.
        /// </value>
        [NotNull]
        public AlfredApplication AlfredApplication
        {
            get;
        }

        /// <summary>
        ///     Gets a Alfred's status in textual form.
        /// </summary>
        /// <value>
        ///     A message describing Alfred's status.
        /// </value>
        [NotNull]
        public string StatusText { get { return _statusText.Value.NonNull(); } }

        /// <summary>
        ///     Gets a value indicating whether Alfred is online.
        /// </summary>
        /// <value>
        ///     true if Alfred is online, false if not.
        /// </value>
        public bool IsOnline { get { return _isOnline; } }

        /// <summary>
        ///     Gets the buttons to appear along an <paramref name="edge"/>.
        /// </summary>
        /// <param name="result"> The result. </param>
        /// <param name="edge"> The docking edge for the buttons to appear along. </param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the buttons in this collection.
        /// </returns>
        internal override IEnumerable<ButtonModel> GetButtons(MFDProcessorResult result, ButtonStripDock edge)
        {
            if (edge == ButtonStripDock.Right)
            {
                return _rightButtons;
            }

            return null;
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor,
            MFDProcessorResult processorResult)
        {
            // No operation (yet)
        }

        /// <summary>
        ///     Toggles Alfred's power.
        /// </summary>
        private void ToggleAlfredPower()
        {
            if (AlfredApplication.IsOnline)
            {
                AlfredApplication.Shutdown();
            }
            else
            {
                AlfredApplication.Initialize();
            }
        }
    }

}
