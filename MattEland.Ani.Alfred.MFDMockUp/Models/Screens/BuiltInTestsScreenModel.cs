// ---------------------------------------------------------
// BuiltInTestsScreenModel.cs
// 
// Created on:      12/16/2015 at 10:16 PM
// Last Modified:   12/16/2015 at 10:16 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------
namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data model for the built in tests screen.
    /// </summary>
    public sealed class BuiltInTestsScreenModel : ScreenModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BuiltInTestsScreenModel"/> class.
        /// </summary>
        public BuiltInTestsScreenModel() : base("BIT")
        {
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor, MFDProcessorResult processorResult)
        {
            //- No Action Needed
        }
    }
}