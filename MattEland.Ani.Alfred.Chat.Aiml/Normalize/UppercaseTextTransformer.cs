// ---------------------------------------------------------
// UppercaseTextTransformer.cs
// 
// Created on:      08/12/2015 at 10:38 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{
    /// <summary>
    /// A text transformer that makes the input text be uppercase for comparison
    /// </summary>
    public class UppercaseTextTransformer : TextTransformer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        /// <param name="inputString">The input string.</param>
        public UppercaseTextTransformer(ChatEngine chatEngine, string inputString)
            : base(chatEngine, inputString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        public UppercaseTextTransformer(ChatEngine chatEngine)
            : base(chatEngine)
        {
        }

        /// <summary>
        /// Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            return TransformInput(InputString);
        }

        /// <summary>
        /// Transforms the input to uppercase.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The transformed input</returns>
        [CanBeNull]
        public static string TransformInput([CanBeNull] string input)
        {
            return input?.ToUpperInvariant();
        }
    }
}