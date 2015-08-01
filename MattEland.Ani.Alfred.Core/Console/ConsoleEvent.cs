using System;
// ReSharper disable MemberCanBePrivate.Global

namespace MattEland.Ani.Alfred.Core.Console
{
    public struct ConsoleEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleEvent"/> class using the current utcTime.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public ConsoleEvent(string title, string message) : this(title, message, DateTime.UtcNow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleEvent"/> class.
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
        /// Gets or sets the time the event was logged in UTC.
        /// </summary>
        /// <value>The time the event was logged in UTC.</value>
        public DateTime UtcTime { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the time in local system time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time
        {
            get { return UtcTime.ToLocalTime(); }
        }


        #region Equality Members

        public bool Equals(ConsoleEvent other)
        {
            return UtcTime.Equals(other.UtcTime) && string.Equals(Title, other.Title) && string.Equals(Message, other.Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is ConsoleEvent && Equals((ConsoleEvent)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = UtcTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (Title?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Message?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        #endregion
    }
}