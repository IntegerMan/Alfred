using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Ani.Alfred.Core
{
    public class ConsoleEvent
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

    }
}