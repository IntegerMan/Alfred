﻿// ---------------------------------------------------------
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
using MattEland.Common;

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
        /// <exception cref="ArgumentOutOfRangeException">The id cannot be empty</exception>
        public User([NotNull] string id)
        {
            //- Validation
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id), Resources.UserCtorNullId);
            }

            Id = id;

            // Set up the variables collection
            UserVariables = new SettingsManager();
            UserVariables.Add("topic", "*");
        }

        /// <summary>
        ///     Gets the predicates. These are variables pertaining to the user.
        /// </summary>
        /// <value>The predicates.</value>
        [NotNull]
        public SettingsManager UserVariables { get; }

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
        [NotNull]
        public string Topic
        {
            get { return UserVariables.GetValue("topic").NonNull(); }
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
        ///     Gets the output sentence from the specified result and sentence.
        /// </summary>
        /// <param name="resultIndex">Index of the result. Defaults to 1.</param>
        /// <param name="sentenceIndex">Index of the sentence. Defaults to 1.</param>
        /// <returns>The output sentence</returns>
        [NotNull]
        public string GetOutputSentence(int resultIndex = 1, int sentenceIndex = 1)
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
                return result.OutputSentences[sentenceIndex].NonNull();
            }

            return string.Empty;
        }

        /// <summary>
        ///     Gets the specified input sentence from the specified result.
        /// </summary>
        /// <param name="resultIndex">Index of the result. Defaults to 1.</param>
        /// <param name="sentenceIndex">Index of the sentenceIndex. Defaults to 1.</param>
        /// <returns>The specified sentenceIndex.</returns>
        [NotNull]
        public string GetInputSentence(int resultIndex = 1, int sentenceIndex = 1)
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
                return result.InputSentences[sentenceIndex].NonNull();
            }

            //- Invalid results yield nothing
            return string.Empty;
        }

        /// <summary>
        ///     Adds a result to the user's results.
        /// </summary>
        /// <param name="latestResult">The latest chat result.</param>
        /// <exception cref="ArgumentNullException"><paramref name="latestResult"/> is <see langword="null" />.</exception>
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