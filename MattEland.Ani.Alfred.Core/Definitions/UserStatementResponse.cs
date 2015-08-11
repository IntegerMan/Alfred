// ---------------------------------------------------------
// UserStatementResponse.cs
// 
// Created on:      08/10/2015 at 5:01 PM
// Last Modified:   08/10/2015 at 11:04 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     A response to a user statement
    /// </summary>
    public struct UserStatementResponse : IEquatable<UserStatementResponse>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public UserStatementResponse([CanBeNull] string userInput, [CanBeNull] string responseText, bool wasHandled)
        {
            UserInput = userInput ?? string.Empty;
            ResponseText = responseText ?? string.Empty;
            WasHandled = wasHandled;
        }

        /// <summary>
        ///     Gets or sets the original user input text.
        /// </summary>
        /// <value>The user input.</value>
        [NotNull]
        public string UserInput { get; }

        /// <summary>
        ///     Gets or sets the response text.
        /// </summary>
        /// <value>The response text.</value>
        [NotNull]
        public string ResponseText { get; }

        /// <summary>
        ///     Gets or sets whether or not the statement was handled.
        /// </summary>
        /// <value>Whether or not the statement was handled.</value>
        public bool WasHandled { get; }

        /// <summary>
        ///     Determines if this instance is equivalent to the other instance
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><c>true</c> if the instances are equivalent, <c>false</c> otherwise.</returns>
        public bool Equals(UserStatementResponse other)
        {
            return string.Equals(UserInput, other.UserInput) && string.Equals(ResponseText, other.ResponseText) &&
                   WasHandled == other.WasHandled;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is UserStatementResponse && Equals((UserStatementResponse)obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        [SuppressMessage("ReSharper", "ConstantNullCoalescingCondition")]
        public override int GetHashCode()
        {
            unchecked
            {
                // Guards against GetHashCode getting called unexpectedly - maybe in constructor? Saw errors with unknown stack
                // when this was not present
                var userInput = UserInput ?? string.Empty;
                var responseText = ResponseText ?? string.Empty;

                var hashCode = userInput.GetHashCode();
                hashCode = (hashCode * 397) ^ responseText.GetHashCode();
                hashCode = (hashCode * 397) ^ WasHandled.GetHashCode();

                return hashCode;
            }
        }

        /// <summary>
        ///     Implements the == operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(UserStatementResponse left, UserStatementResponse right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Implements the != operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(UserStatementResponse left, UserStatementResponse right)
        {
            return !left.Equals(right);
        }
    }
}