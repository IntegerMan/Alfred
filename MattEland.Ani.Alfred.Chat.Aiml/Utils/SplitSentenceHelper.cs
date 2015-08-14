// ---------------------------------------------------------
// SplitSentenceHelper.cs
// 
// Created on:      08/12/2015 at 10:38 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    /// A utility class for splitting strings into sentences
    /// </summary>
    internal static class SplitSentenceHelper
    {

        /// <summary>
        /// Transforms the input string into sentences.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <param name="chatEngine">The chat engine.</param>
        /// <returns>Sentences</returns>
        [NotNull]
        [ItemNotNull]
        internal static IEnumerable<string> Split([NotNull] string inputString, [NotNull] ChatEngine chatEngine)
        {
            //- Validate
            if (inputString == null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }
            if (chatEngine == null)
            {
                throw new ArgumentNullException(nameof(chatEngine));
            }

            // Use the engine's splitters to split the input into sentences
            var sentences = inputString.Split(chatEngine.Splitters.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            // Yield all sentences
            return sentences.Select(s => s?.Trim());
        }
    }
}