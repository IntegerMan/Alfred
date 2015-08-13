// ---------------------------------------------------------
// SanitizingTextTransformer.cs
// 
// Created on:      08/12/2015 at 10:39 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{
    /// <summary>
    /// A text transformer to remove illegal characters.
    /// </summary>
    public class SanitizingTextTransformer : TextTransformer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        public SanitizingTextTransformer(ChatEngine chatEngine)
            : base(chatEngine)
        {
        }

        /// <summary>
        /// Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // Removes illegal characters and replaces them with spaces
            return ChatEngine.Strippers.Replace(InputString.NonNull(), " ");
        }
    }
}