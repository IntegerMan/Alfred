// ---------------------------------------------------------
// SplitIntoSentences.cs
// 
// Created on:      08/12/2015 at 10:38 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Linq;
using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{
    /// <summary>
    /// A utility class for splitting strings into sentences
    /// </summary>
    /// <remarks>
    /// TODO: Look into usages and make this a static 
    /// </remarks>
    public class SplitIntoSentences
    {
        [NotNull]
        private readonly ChatEngine _chatEngine;

        [NotNull]
        private string _inputString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitIntoSentences"/> class.
        /// </summary>
        /// <param name="chatEngine">The chat engine.</param>
        /// <param name="inputString">The input string.</param>
        public SplitIntoSentences([NotNull] ChatEngine chatEngine, string inputString)
        {
            if (chatEngine == null) throw new ArgumentNullException(nameof(chatEngine));
            _chatEngine = chatEngine;
            _inputString = inputString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitIntoSentences"/> class.
        /// </summary>
        /// <param name="chatEngine">The chat engine.</param>
        public SplitIntoSentences([NotNull] ChatEngine chatEngine)
        {
            if (chatEngine == null) throw new ArgumentNullException(nameof(chatEngine));
            _chatEngine = chatEngine;
            _inputString= string.Empty;
        }

        /// <summary>
        /// Transforms the specified input string.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>Sentences</returns>
        public string[] Transform([NotNull] string inputString)
        {
            if (inputString == null) throw new ArgumentNullException(nameof(inputString));
            _inputString = inputString;
            return Transform();
        }

        /// <summary>
        /// Transforms the input string into sentences.
        /// </summary>
        /// <returns>Sentences</returns>
        [NotNull]
        public string[] Transform()
        {
            var strArray = _inputString.Split(_chatEngine.Splitters.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            return strArray.Select(word => word.Trim()).Where(wordTrimmed => wordTrimmed.Length > 0).ToArray();
        }
    }
}