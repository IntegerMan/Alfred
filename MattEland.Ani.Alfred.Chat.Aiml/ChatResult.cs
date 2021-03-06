﻿// ---------------------------------------------------------
// ChatResult.cs
// 
// Created on:      09/03/2015 at 11:00 PM
// Last Modified:   09/07/2015 at 9:56 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    /// <summary>
    ///     Represents the result of an Aiml query
    /// </summary>
    public sealed class ChatResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatResult" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="chatEngine">The chat engine.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="ArgumentNullException">
        /// user, chatEngine, <paramref name="request"/>
        /// </exception>
        public ChatResult(
            [NotNull] User user,
            [NotNull] ChatEngine chatEngine,
            [NotNull] Request request)
        {
            //- Validation
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (chatEngine == null) { throw new ArgumentNullException(nameof(chatEngine)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            //- Set Fields
            User = user;
            ChatEngine = chatEngine;

            // Ensure the request is linked to this
            Request = request;
            Request.ChatResult = this;
        }

        /// <summary>
        ///     Gets the chat engine.
        /// </summary>
        /// <value>
        /// The chat engine.
        /// </value>
        [NotNull]
        private ChatEngine ChatEngine { get; }

        /// <summary>
        ///     Gets the duration of the request.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        [UsedImplicitly]
        public TimeSpan Duration { get; private set; }

        /// <summary>
        ///     Gets the input sentences.
        /// </summary>
        /// <value>
        /// The input sentences.
        /// </value>
        [NotNull]
        internal IList<string> InputSentences { get; } = new List<string>();

        /// <summary>
        ///     Gets the normalized paths.
        /// </summary>
        /// <value>
        /// The normalized paths.
        /// </value>
        [NotNull]
        internal ICollection<string> NormalizedPaths { get; } = new List<string>();

        /// <summary>
        ///     Gets the output sentences.
        /// </summary>
        /// <value>
        /// The output sentences.
        /// </value>
        [NotNull]
        internal IList<string> OutputSentences { get; } = new List<string>();

        /// <summary>
        ///     Gets the request.
        /// </summary>
        /// <value>
        /// The request.
        /// </value>
        [NotNull]
        public Request Request { get; }

        /// <summary>
        ///     Gets the SubQueries.
        /// </summary>
        /// <value>
        /// The SubQueries.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public ICollection<SubQuery> SubQueries { get; } = new List<SubQuery>();

        /// <summary>
        ///     Gets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        [NotNull]
        public User User { get; }

        /// <summary>
        ///     Gets the raw input.
        /// </summary>
        /// <value>
        /// The raw input.
        /// </value>
        public string RawInput
        {
            get { return Request.RawInput; }
        }

        /// <summary>
        ///     <para>Gets the output.</para>
        ///     <para>
        ///         This is an additional layer on top of <see cref="RawOutput"/> that handles timeouts
        ///         and no responses.
        ///     </para>
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        [NotNull]
        public string Output
        {
            get
            {
                // If we have sentences, just defer to the raw output
                if (OutputSentences.Count > 0) { return RawOutput; }

                // If it timed out, we'll use the timeout message
                if (Request.HasTimedOut) { return Resources.ChatEngineRequestTimedOut.NonNull(); }

                // No response and it didn't time out. Log it and return a different message.
                var stringBuilder = new StringBuilder();
                foreach (var path in NormalizedPaths)
                {
                    stringBuilder.AppendFormat(ChatEngine.Locale,
                                               "{0}{1}",
                                               path,
                                               Environment.NewLine);
                }

                var message = string.Format(ChatEngine.Locale,
                                            Resources.ChatEngineErrorCouldNotFindResponse.NonNull(),
                                            RawInput,
                                            Environment.NewLine,
                                            stringBuilder,
                                            User.Name);
                ChatEngine.Log(message, LogLevel.Warning);

                return ChatEngine.FallbackResponse;
            }
        }

        /// <summary>
        ///     Gets the raw output. For displaying results to the user, use <see cref="Output"/>
        ///     instead.
        /// </summary>
        /// <value>
        /// The raw output.
        /// </value>
        [NotNull]
        internal string RawOutput
        {
            get
            {
                // Loop through each sentence and append it to the output
                var stringBuilder = new StringBuilder();
                foreach (var outputSentence in OutputSentences)
                {
                    Debug.Assert(outputSentence != null);

                    var sentence = outputSentence.Trim();
                    if (!SentenceEndsWithPunctuation(sentence)) { sentence += "."; }
                    stringBuilder.AppendFormat(ChatEngine.Locale, "{0} ", sentence);
                }
                return stringBuilder.ToString().Trim();
            }
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Output;
        }

        /// <summary>
        ///     Calculates whether the input <paramref name="sentence"/> ends with proper
        ///     punctuation according to the ChatEngine.SentenceSplitters collection.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <returns>
        ///     True if the <paramref name="sentence"/> has the correct punctuation;
        ///     <see langword="false"/> otherwise.
        /// </returns>
        private bool SentenceEndsWithPunctuation([NotNull] string sentence)
        {
            return
                ChatEngine.SentenceSplitters.Any(
                                                 splitter =>
                                                 sentence.Trim()
                                                         .EndsWith(splitter,
                                                                   StringComparison
                                                                       .OrdinalIgnoreCase));
        }

        /// <summary>
        ///     Tells the result that the request completed and it should calculate the
        ///     <see cref="Duration"/> of the request
        /// </summary>
        internal void Completed()
        {
            Duration = DateTime.Now - Request.StartedOn;
        }
    }
}