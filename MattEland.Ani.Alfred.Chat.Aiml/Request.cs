// ---------------------------------------------------------
// Request.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/07/2015 at 10:01 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    /// <summary>
    ///     Represents a chat request or a sub-component of an existing request
    /// </summary>
    public sealed class Request
    {
        /// <summary>
        ///     The child request.
        /// </summary>
        private Request _child;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Request" /> class.
        /// </summary>
        /// <param name="rawInput">The raw input.</param>
        /// <param name="user">The user.</param>
        /// <param name="chatEngine">The chat engine.</param>
        /// <exception cref="ArgumentNullException" />
        public Request(
            [NotNull] string rawInput,
            [NotNull] User user,
            [NotNull] ChatEngine chatEngine) : this(rawInput, user, chatEngine, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Request" /> class as a child of a prior
        ///     Request
        /// </summary>
        /// <param name="rawInput">The raw input.</param>
        /// <param name="user">The user.</param>
        /// <param name="chatEngine">The chat engine.</param>
        /// <param name="parent">The parent.</param>
        /// <exception cref="ArgumentNullException" />
        internal Request(
            [NotNull] string rawInput,
            [NotNull] User user,
            [NotNull] ChatEngine chatEngine,
            [CanBeNull] Request parent)
        {
            //- Validate
            if (rawInput == null) { throw new ArgumentNullException(nameof(rawInput)); }
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (chatEngine == null) { throw new ArgumentNullException(nameof(chatEngine)); }

            //- Set Properties
            RawInput = rawInput;
            User = user;
            ChatEngine = chatEngine;

            if (parent != null)
            {
                // Set up the parent / child hierarchy
                Parent = parent;
                parent.RegisterChild(this);

                // Copy some additional properties from the parent
                StartedOn = parent.StartedOn;
                HasTimedOut = parent.HasTimedOut;
            }
            else
            {
                // Set the current start time as the StartedOn field
                StartedOn = DateTime.Now;
            }
        }

        /// <summary>
        ///     Gets the chat engine.
        /// </summary>
        /// <value>
        /// The chat engine.
        /// </value>
        [NotNull]
        public ChatEngine ChatEngine { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has timed out. Call
        ///     CheckForTimeOut to update this.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has timed out; otherwise, <c>false</c> .
        /// </value>
        public bool HasTimedOut { get; private set; }

        /// <summary>
        ///     Gets the raw input.
        /// </summary>
        /// <value>
        /// The raw input.
        /// </value>
        [NotNull]
        public string RawInput { get; }

        /// <summary>
        ///     Gets or sets the result of the request.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        [CanBeNull]
        public ChatResult ChatResult { get; set; }

        /// <summary>
        ///     Gets or sets when the request started.
        /// </summary>
        /// <value>
        /// The started on.
        /// </value>
        public DateTime StartedOn { get; }

        /// <summary>
        ///     Gets the user associated with the request.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        [NotNull]
        public User User { get; }

        /// <summary>
        ///     Gets the parent request, if any.
        /// </summary>
        /// <remarks>
        ///     Requests can have parents if they were spawned by a prior Request
        /// </remarks>
        /// <value>
        /// The parent request.
        /// </value>
        [CanBeNull]
        public Request Parent { get; set; }

        /// <summary>
        ///     Gets the child request, if any.
        /// </summary>
        /// <remarks>
        ///     Requests can have children if they spawn an additional Request object
        /// </remarks>
        /// <value>
        /// The child request.
        /// </value>
        [CanBeNull]
        public Request Child
        {
            get { return _child; }
        }

        /// <summary>
        ///     Registers a child request.
        /// </summary>
        /// <param name="childRequest">The child request.</param>
        /// <exception cref="ArgumentNullException"><paramref name="childRequest"/></exception>
        private void RegisterChild([NotNull] Request childRequest)
        {
            if (childRequest == null) { throw new ArgumentNullException(nameof(childRequest)); }

            _child = childRequest;
        }

        /// <summary>
        ///     Checks to see if this has timed out and returns <see langword="true"/> if that has
        ///     happened. This will also log if the request timed out and update HasTimedOut.
        /// </summary>
        /// <remarks>
        ///     If ChatEngine.Timeout is set to 0 or negative values, timeout will never occur
        /// </remarks>
        /// <returns><c>true</c> if the request has timed out, <c>false</c> otherwise.</returns>
        public bool CheckForTimedOut()
        {
            if (HasTimedOut) { return HasTimedOut; }
            var timeLimit = ChatEngine.Timeout;

            // Allow disabling timeout by setting it to <= 0 values
            if (!(timeLimit > 0)) { return HasTimedOut; }

            // Calculate timeout based on start time and now
            HasTimedOut = StartedOn.AddMilliseconds(timeLimit) < DateTime.Now;

            // Do appropriate logging on new timeouts
            if (HasTimedOut)
            {
                var message = string.Format(ChatEngine.Locale,
                                            Resources.RequestTimedOut.NonNull(),
                                            User.Name,
                                            RawInput);

                ChatEngine.Log(message, LogLevel.Warning);
            }

            return HasTimedOut;
        }
    }
}