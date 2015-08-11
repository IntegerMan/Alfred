// ---------------------------------------------------------
// UserStatementResponse.cs
// 
// Created on:      08/10/2015 at 5:01 PM
// Last Modified:   08/11/2015 at 1:41 PM
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
        ///     Initializes a new instance of the <see cref="T:UserStatementResponse" /> class.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <param name="responseText">The response text.</param>
        /// <param name="template">The template.</param>
        /// <param name="command">The Command.</param>
        public UserStatementResponse([CanBeNull] string userInput,
                                     [CanBeNull] string responseText,
                                     string template,
                                     string command)
        {
            UserInput = userInput ?? string.Empty;
            ResponseText = responseText ?? string.Empty;
            Template = template;
            Command = command;
        }

        /// <summary>
        ///     Gets the original user input text.
        /// </summary>
        /// <value>The user input.</value>
        [NotNull]
        public string UserInput { get; }

        /// <summary>
        ///     Gets the response text.
        /// </summary>
        /// <value>The response text.</value>
        [NotNull]
        public string ResponseText { get; }

        /// <summary>
        ///     Gets the AIML template used to generate a response.
        /// </summary>
        /// <value>The template.</value>
        [CanBeNull]
        public string Template { get; }

        /// <summary>
        ///     Gets the system Command to execute.
        /// </summary>
        /// <value>The Command.</value>
        public string Command { get; }

        /// <summary>
        ///     Determines if this instance is equivalent to the other instance
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><c>true</c> if the instances are equivalent, <c>false</c> otherwise.</returns>
        public bool Equals(UserStatementResponse other)
        {
            return string.Equals(UserInput, other.UserInput) && string.Equals(ResponseText, other.ResponseText) &&
                   string.Equals(Template, other.Template) && string.Equals(Command, other.Command);
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
                // ?? Guards against GetHashCode getting called unexpectedly - maybe in constructor? Saw errors with
                // unknown stack when this was not present
                var hashCode = (UserInput ?? string.Empty).GetHashCode();
                hashCode = (hashCode * 397) ^ (ResponseText ?? string.Empty).GetHashCode();
                hashCode = (hashCode * 397) ^ (Command ?? string.Empty).GetHashCode();
                hashCode = (hashCode * 397) ^ (Template ?? string.Empty).GetHashCode();

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