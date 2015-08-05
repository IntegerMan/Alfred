// ---------------------------------------------------------
// ConsoleEvent.cs
// 
// Created on:      07/26/2015 at 2:23 PM
// Last Modified:   08/05/2015 at 2:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

// ReSharper disable MemberCanBePrivate.Global

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     Represents a logged event to the console
    /// </summary>
    public struct ConsoleEvent : IEquatable<ConsoleEvent>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConsoleEvent" /> class using the current utcTime.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public ConsoleEvent(string title, string message) : this(title, message, DateTime.UtcNow)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConsoleEvent" /> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="utcTime">The utcTime in UTC.</param>
        public ConsoleEvent(string title, string message, DateTime utcTime)
        {
            UtcTime = utcTime;
            Title = title;
            Message = message;
        }

        /// <summary>
        ///     Gets or sets the time the event was logged in UTC.
        /// </summary>
        /// <value>The time the event was logged in UTC.</value>
        public DateTime UtcTime { get; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; }

        /// <summary>
        ///     Gets or sets the time in local system time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time
        {
            get { return UtcTime.ToLocalTime(); }
        }

        #region Equality Members

        /// <summary>
        ///     Determines if this instance is equivalent to another
        /// </summary>
        /// <param name="other">The other event.</param>
        /// <returns><c>true</c> if the events are equivalent, <c>false</c> otherwise.</returns>
        public bool Equals(ConsoleEvent other)
        {
            return UtcTime.Equals(other.UtcTime) && string.Equals(Title, other.Title) &&
                   string.Equals(Message, other.Message);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is ConsoleEvent && Equals((ConsoleEvent) obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = UtcTime.GetHashCode();
                hashCode = (hashCode*397) ^ (Title?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (Message?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        /// <summary>
        ///     Implements the == operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ConsoleEvent left, ConsoleEvent right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Implements the != operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ConsoleEvent left, ConsoleEvent right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}