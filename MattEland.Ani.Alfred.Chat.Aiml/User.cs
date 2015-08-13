// ---------------------------------------------------------
// User.cs
// 
// Created on:      08/12/2015 at 10:24 PM
// Last Modified:   08/13/2015 at 3:45 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    /// <summary>
    ///     Represents a user the chat engine interacts with
    /// </summary>
    public class User
    {
        [NotNull]
        [ItemNotNull]
        private readonly List<Result> _results = new List<Result>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="chatEngine">The chat engine.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The id cannot be empty</exception>
        public User([NotNull] string id, [NotNull] ChatEngine chatEngine)
        {
            //- Validation
            if (chatEngine == null)
            {
                throw new ArgumentNullException(nameof(chatEngine));
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id), "The id cannot be empty");
            }

            Id = id;
            ChatEngine = chatEngine;

            // Set up Predicates by cloning the engine's collection
            //? What are these for?
            Predicates = new SettingsDictionary();
            ChatEngine.DefaultPredicates.Clone(Predicates);
            Predicates.Add("topic", "*");
        }

        /// <summary>
        ///     Gets the chat engine.
        /// </summary>
        /// <value>The chat engine.</value>
        [NotNull]
        public ChatEngine ChatEngine { get; }

        /// <summary>
        ///     Gets the predicates.
        /// </summary>
        /// <value>The predicates.</value>
        [NotNull]
        public SettingsDictionary Predicates { get; }

        /// <summary>
        ///     Gets the user's identifier.
        /// </summary>
        /// <value>The user's identifier.</value>
        [NotNull]
        public string Id { get; }

        /// <summary>
        ///     Gets the topic.
        /// </summary>
        /// <value>The topic.</value>
        public string Topic
        {
            get { return Predicates.GetValue("topic"); }
        }

        /// <summary>
        ///     Gets the last result.
        /// </summary>
        /// <value>The last result.</value>
        [CanBeNull]
        public Result LastResult
        {
            get { return _results.FirstOrDefault(); }
        }

        /// <summary>
        ///     Gets the last chat result's raw output
        /// </summary>
        /// <returns>The last result's raw output</returns>
        [NotNull]
        public string LastChatOutput
        {
            get { return LastResult?.RawOutput ?? "*"; }
        }

        /// <summary>
        ///     Gets the first output sentence from the first result
        /// </summary>
        /// <returns>The output sentence</returns>
        public string GetOutputSentence()
        {
            return GetOutputSentence(0, 0);
        }

        /// <summary>
        ///     Gets the first output sentence from the specified result.
        /// </summary>
        /// <param name="resultIndex">Index of the result.</param>
        /// <returns>The output sentence</returns>
        public string GetOutputSentence(int resultIndex)
        {
            return GetOutputSentence(resultIndex, 0);
        }

        /// <summary>
        ///     Gets the output sentence from the specified result and sentence.
        /// </summary>
        /// <param name="resultIndex">Index of the result.</param>
        /// <param name="sentenceIndex">Index of the sentence.</param>
        /// <returns>The output sentence</returns>
        public string GetOutputSentence(int resultIndex, int sentenceIndex)
        {
            //- Ensure we're not grabbing out of range
            var isValidResult = resultIndex >= 0 & resultIndex < _results.Count;
            if (!isValidResult)
            {
                return string.Empty;
            }

            // Get the result
            var result = _results[resultIndex];
            Debug.Assert(result != null);

            // Grab the output sentence at the specified index
            if (sentenceIndex >= 0 & sentenceIndex < result.OutputSentences.Count)
            {
                return result.OutputSentences[sentenceIndex];
            }

            return string.Empty;
        }

        /// <summary>
        ///     Gets the first input sentence from the first result.
        /// </summary>
        /// <returns>
        ///     The specified sentenceIndex.
        /// </returns>
        public string GetInputSentence()
        {
            return GetInputSentence(0, 0);
        }

        /// <summary>
        ///     Gets the first input sentence from the specified result.
        /// </summary>
        /// <param name="resultIndex">Index of the result.</param>
        /// <returns>The specified sentenceIndex.</returns>
        public string GetInputSentence(int resultIndex)
        {
            return GetInputSentence(resultIndex, 0);
        }

        /// <summary>
        ///     Gets the specified input sentence from the specified result.
        /// </summary>
        /// <param name="resultIndex">Index of the result.</param>
        /// <param name="sentenceIndex">Index of the sentenceIndex.</param>
        /// <returns>The specified sentenceIndex.</returns>
        public string GetInputSentence(int resultIndex, int sentenceIndex)
        {
            //- Ensure we're grabbing at an acceptable resultIndex
            var isValidResult = resultIndex >= 0 & resultIndex < _results.Count;
            if (!isValidResult)
            {
                return string.Empty;
            }

            //- Grab the result
            var result = _results[resultIndex];
            Debug.Assert(result != null);

            // Grab the appropriate sentenceIndex
            if (sentenceIndex >= 0 & sentenceIndex < result.InputSentences.Count)
            {
                return result.InputSentences[sentenceIndex];
            }

            //- Invalid results yield nothing
            return string.Empty;
        }

        /// <summary>
        ///     Adds a result to the user's results.
        /// </summary>
        /// <param name="latestResult">The latest chat result.</param>
        public void AddResult([NotNull] Result latestResult)
        {
            if (latestResult == null)
            {
                throw new ArgumentNullException(nameof(latestResult));
            }

            _results.Insert(0, latestResult);
        }
    }
}