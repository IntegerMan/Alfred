// ---------------------------------------------------------
// Result.cs
// 
// Created on:      08/12/2015 at 10:23 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    /// <summary>
    /// Represents the result of an Aiml query
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets the chat engine.
        /// </summary>
        /// <value>The chat engine.</value>
        [NotNull]
        public ChatEngine ChatEngine { get; }

        /// <summary>
        /// Gets the duration of the request.
        /// </summary>
        /// <value>The duration.</value>
        public TimeSpan Duration { get; private set; }
        /// <summary>
        /// Gets the input sentences.
        /// </summary>
        /// <value>The input sentences.</value>
        [NotNull]
        public List<string> InputSentences { get; } = new List<string>();
        /// <summary>
        /// Gets the normalized paths.
        /// </summary>
        /// <value>The normalized paths.</value>
        [NotNull]
        public List<string> NormalizedPaths { get; } = new List<string>();
        /// <summary>
        /// Gets the output sentences.
        /// </summary>
        /// <value>The output sentences.</value>
        [NotNull]
        public List<string> OutputSentences { get; } = new List<string>();
        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        [NotNull]
        public Request Request { get; }
        /// <summary>
        /// Gets the SubQueries.
        /// </summary>
        /// <value>The SubQueries.</value>
        [NotNull]
        public List<SubQuery> SubQueries { get; } = new List<SubQuery>();
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <value>The user.</value>
        [NotNull]
        public User User { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="chatEngine">The chat engine.</param>
        /// <param name="request">The request.</param>
        public Result([NotNull] User user, [NotNull] ChatEngine chatEngine, [NotNull] Request request)
        {
            //- Validation
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (chatEngine == null)
            {
                throw new ArgumentNullException(nameof(chatEngine));
            }
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            //- Set Fields
            User = user;
            ChatEngine = chatEngine;

            // Ensure the request is linked to this
            Request = request;
            Request.Result = this;
        }

        public string RawInput
        {
            get { return Request.RawInput; }
        }

        public string Output
        {
            get
            {
                if (OutputSentences.Count > 0)
                {
                    return RawOutput;
                }
                if (Request.HasTimedOut)
                {
                    return ChatEngine.TimeOutMessage;
                }
                var stringBuilder = new StringBuilder();
                foreach (var str in NormalizedPaths)
                {
                    stringBuilder.Append(str + Environment.NewLine);
                }
                ChatEngine.writeToLog("The ChatEngine could not find any response for the input: " + RawInput + " with the path(s): " +
                               Environment.NewLine + stringBuilder + " from the user with an id: " + User.UserID);
                return string.Empty;
            }
        }

        public string RawOutput
        {
            get
            {
                var stringBuilder = new StringBuilder();
                foreach (var str in OutputSentences)
                {
                    var sentence = str.Trim();
                    if (!checkEndsAsSentence(sentence))
                    {
                        sentence += ".";
                    }
                    stringBuilder.Append(sentence + " ");
                }
                return stringBuilder.ToString().Trim();
            }
        }

        public override string ToString()
        {
            return Output;
        }

        private bool checkEndsAsSentence(string sentence)
        {
            foreach (var str in ChatEngine.Splitters)
            {
                if (sentence.Trim().EndsWith(str))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tells the result that the request completed and it should calculate the Duration of the request
        /// </summary>
        public void Completed()
        {
            Duration = DateTime.Now - Request.StartedOn;
        }
    }
}