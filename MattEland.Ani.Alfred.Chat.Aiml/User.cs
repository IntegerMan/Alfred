// ---------------------------------------------------------
// User.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/07/2015 at 10:02 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    /// <summary>
    ///     Represents a user the chat engine interacts with
    /// </summary>
    public sealed class User
    {
        [NotNull]
        [ItemNotNull]
        private readonly List<ChatResult> _results = new List<ChatResult>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="name">The identifier.</param>
        /// <exception cref="ArgumentOutOfRangeException">The id cannot be empty</exception>
        public User([NotNull] string name) : this(name, false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="name">The identifier.</param>
        /// <param name="isSystemUser">
        /// <see langword="true" /> if this instance is system user, <see langword="false" /> if
        /// not.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="name"/> cannot be empty.
        /// </exception>
        internal User([NotNull] string name, bool isSystemUser)
        {
            //- Validation
            if (name.IsEmpty()) { throw new ArgumentOutOfRangeException(nameof(name)); }

            Name = name;
            UniqueId = Guid.NewGuid();
            IsSystemUser = isSystemUser;

            // Set up the variables collection
            UserVariables = new SettingsManager();
            UserVariables.Add("topic", "*");
        }

        /// <summary>
        ///     Gets the predicates. These are variables pertaining to the user.
        /// </summary>
        /// <value>
        /// The predicates.
        /// </value>
        [NotNull]
        internal SettingsManager UserVariables { get; }

        /// <summary>
        ///     Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [NotNull]
        public string Name { get; }


        /// <summary>
        ///     Gets the user's identifier.
        /// </summary>
        /// <value>The user's identifier.</value>
        [NotNull]
        public Guid UniqueId { get; }

        /// <summary>
        ///     Gets the topic.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        [NotNull]
        internal string Topic
        {
            get
            {
                // TODO: Docs need more on topics
                return UserVariables.GetValue("topic").NonNull();
            }
        }

        /// <summary>
        ///     Gets the last result.
        /// </summary>
        /// <value>
        /// The last result.
        /// </value>
        [CanBeNull]
        private ChatResult LastChatResult
        {
            get { return _results.FirstOrDefault(); }
        }

        /// <summary>
        ///     Gets the last chat result's raw output
        /// </summary>
        /// <returns>The last result's raw output</returns>
        [NotNull]
        internal string LastChatOutput
        {
            get { return LastChatResult?.RawOutput ?? "*"; }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance represents a chat engine's system
        ///     <see cref="User" /> .
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this instance is system user, <see langword="false" /> if
        /// not.
        /// </value>
        public bool IsSystemUser { get; }

        /// <summary>
        ///     Gets the output sentence from the specified result and sentence.
        /// </summary>
        /// <param name="resultIndex">Index of the result. Defaults to 1.</param>
        /// <param name="sentenceIndex">Index of the sentence. Defaults to 1.</param>
        /// <returns>The output sentence</returns>
        [NotNull]
        internal string GetOutputSentence(int resultIndex = 1, int sentenceIndex = 1)
        {
            //- Ensure we're not grabbing out of range
            var isValidResult = resultIndex >= 0 & resultIndex < _results.Count;
            if (!isValidResult) { return string.Empty; }

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
        /// <param name="sentenceIndex">Index of the sentence. Defaults to 1.</param>
        /// <returns>The specified sentence.</returns>
        [NotNull]
        internal string GetInputSentence(int resultIndex = 1, int sentenceIndex = 1)
        {
            //- Ensure we're grabbing at an acceptable resultIndex
            var isValidResult = resultIndex >= 0 & resultIndex < _results.Count;
            if (!isValidResult) { return string.Empty; }

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
        /// <param name="result">The latest chat result.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="result" /> is <see langword="null" /> .
        /// </exception>
        internal void AddResult([NotNull] ChatResult result)
        {
            if (result == null) { throw new ArgumentNullException(nameof(result)); }

            _results.Insert(0, result);
        }
    }
}