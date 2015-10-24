using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     Encapsulates the result of a single processing frame of a multifunction display processor. 
    /// </summary>
    public sealed class MFDProcessorResult
    {
        /// <summary>
        ///     The processor.
        /// </summary>
        [NotNull]
        private readonly MFDProcessor _processor;

        /// <summary>
        ///     Initializes a new instance of the MFDProcessorResult class.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        public MFDProcessorResult(MFDProcessor processor)
        {
            _processor = processor;
        }
    }
}