using System;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    /// A response to a user statement
    /// </summary>
    public class UserStatementResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public UserStatementResponse([CanBeNull] string userInput, [CanBeNull] string responseText, bool wasHandled)
        {
            UserInput = userInput ?? string.Empty;
            ResponseText = responseText ?? string.Empty;
            WasHandled = wasHandled;
        }

        /// <summary>
        /// Gets or sets the original user input text.
        /// </summary>
        /// <value>The user input.</value>
        [NotNull]
        public string UserInput { get; set; }

        /// <summary>
        /// Gets or sets the response text.
        /// </summary>
        /// <value>The response text.</value>
        [NotNull]
        public string ResponseText { get; set; }

        /// <summary>
        /// Gets or sets whether or not the statement was handled.
        /// </summary>
        /// <value>Whether or not the statement was handled.</value>
        public bool WasHandled { get; set; }
    }
}