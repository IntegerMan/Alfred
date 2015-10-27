using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    /// <summary>
    ///     A data model for navigation buttons. This class cannot be inherited.
    /// </summary>
    public sealed class NavigationButtonModel : ButtonModel
    {
        [NotNull]
        private readonly ScreenModel _target;

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
        ///     Initializes a new instance of the ButtonModel class.
        /// </summary>
        /// <param name="target"> Target for the navigation. </param>
        /// <param name="text"> The text. </param>
        /// <param name="listener"> The button provider. </param>
        /// <param name="isSelected"> true if this instance is selected. </param>
        /// <param name="index"> The button's index. </param>
        public NavigationButtonModel([NotNull] ScreenModel target,
            [CanBeNull] string text,
            [NotNull] ButtonProvider listener,
            bool isSelected = false,
            int index = -1) : base(text, listener, isSelected, index)
        {
            _target = target;
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
    }
}
