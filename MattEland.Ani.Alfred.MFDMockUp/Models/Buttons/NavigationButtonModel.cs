using System.Diagnostics;

using Assisticant.Fields;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    /// <summary>
    ///     A data model for navigation buttons. This class cannot be inherited.
    /// </summary>
    public sealed class NavigationButtonModel : ButtonModel
    {
        /// <summary>
        ///     The observable property for whether or not the button is selected.
        /// </summary>
        [NotNull]
        private readonly Observable<bool> _isSelected;

        /// <summary>
        ///     Target for the navigation.
        /// </summary>
        [NotNull]
        private readonly ScreenModel _target;

        /// <summary>
        ///     Initializes a new instance of the ButtonModel class.
        /// </summary>
        /// <param name="target"> Target for the navigation. </param>
        /// <param name="listener"> The button provider. </param>
        /// <param name="index"> The button's index. </param>
        public NavigationButtonModel([NotNull] ScreenModel target,
            [NotNull] IButtonClickListener listener,
            int index = -1) : base(target.ButtonText, listener, index)
        {
            _target = target;

            _isSelected = new Observable<bool>();
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is selected. This will be <c>true</c> if
        ///     <see cref="Target"/> is the current screen.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is selected, <c>false</c> if not.
        /// </value>
        public override bool IsSelected
        {
            get
            {
                return _isSelected;
            }
        }

        /// <summary>
        ///     Gets the Target for the navigation
        /// </summary>
        /// <value>
        ///     The target.
        /// </value>
        [NotNull]
        public ScreenModel Target
        {
            [DebuggerStepThrough]
            get
            { return _target; }
        }

        /// <summary>
        ///     Process the command by interacting with the current processor frame.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="result"> The result. </param>
        internal override void ProcessCommand([NotNull] MFDProcessor processor,
            [NotNull] MFDProcessorResult result)
        {
            // Tell the processor we want to navigate to our configured target.
            result.RequestedScreen = Target;
        }

        /// <summary>
        ///     Processes the current state.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="result"> The result. </param>
        public override void ProcessCurrentState(MFDProcessor processor, MFDProcessorResult result)
        {
            //! Update whether or not we're selected
            _isSelected.Value = result.CurrentScreen == Target;
        }
    }
}
