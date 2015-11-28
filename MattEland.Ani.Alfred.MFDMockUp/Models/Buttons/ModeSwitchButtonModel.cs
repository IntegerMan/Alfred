// ---------------------------------------------------------
// ModeSwitchButtonModel.cs
// 
// Created on:      11/22/2015 at 2:41 PM
// Last Modified:   11/22/2015 at 2:41 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    /// <summary>
    ///     A data model for a mode switch button that moves to or cycles between other Master Modes.
    /// </summary>
    public sealed class ModeSwitchButtonModel : ButtonModel
    {
        /// <summary>
        ///     Initializes a new instance of the ModeSwitchButtonModel class.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <param name="listener"> The listener. </param>
        /// <param name="index"> Zero-based index of the button within its panel. </param>
        public ModeSwitchButtonModel([CanBeNull] string text = null,
            [CanBeNull] IButtonClickListener listener = null,
            int index = -1) : base(text, listener, index)
        {
        }

        /// <summary>
        ///     Gets or sets the master mode cycler that controls which mode comes next.
        /// </summary>
        /// <value>
        ///     The master mode cycler.
        /// </value>
        [CanBeNull]
        public MasterModeCycler MasterModeCycler { get; set; }

        /// <summary>
        ///     Process the command by interacting with the current processor frame.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="result"> The result. </param>
        internal override void ProcessCommand(MFDProcessor processor, MFDProcessorResult result)
        {
            if (MasterModeCycler != null)
            {
                var newMode = MasterModeCycler.MoveToNextMode();

                result.RequestedMasterMode = newMode;
                result.RequestedScreen = newMode.DefaultScreen;
            }
        }

    }
}