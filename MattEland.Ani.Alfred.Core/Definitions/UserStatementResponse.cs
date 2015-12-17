// ---------------------------------------------------------
// UserStatementResponse.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/01/2015 at 1:00 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     A response from the chat engine to a user statement or system event.
    /// </summary>
    public class UserStatementResponse
    {

        /// <summary>Initializes a new instance of the <see cref="UserStatementResponse" /> class.</summary>
        /// <param name="userInput">The user input.</param>
        /// <param name="responseText">The response text.</param>
        /// <param name="template">The template.</param>
        /// <param name="command">The Command.</param>
        /// <param name="resultData">Information describing the result.</param>
        public UserStatementResponse(
            [CanBeNull] string userInput,
            [CanBeNull] string responseText,
            [CanBeNull] string template,
            ChatCommand command,
            [CanBeNull] object resultData)
        {
            UserInput = userInput ?? string.Empty;
            ResponseText = responseText ?? string.Empty;
            Template = template;
            Command = command;
            ResultData = resultData;
        }

        /// <summary>Initializes a new instance of the <see cref="UserStatementResponse" /> class.</summary>
        public UserStatementResponse() : this(null, null, null, ChatCommand.Empty, null)
        {
        }

        /// <summary>Gets information describing the result.</summary>
        /// <value>Information describing the result.</value>
        public object ResultData { get; }

        /// <summary>Gets the original user input text.</summary>
        /// <value>The user input.</value>
        [NotNull]
        public string UserInput { get; }

        /// <summary>Gets the response text.</summary>
        /// <value>The response text.</value>
        [NotNull]
        public string ResponseText { get; }

        /// <summary>Gets the AIML template used to generate a response.</summary>
        /// <value>The template.</value>
        [CanBeNull]
        public string Template { get; }

        /// <summary>Gets the system
        ///     <see cref="MattEland.Ani.Alfred.Core.Definitions.UserStatementResponse.Command" />
        ///     to execute.</summary>
        /// <value>The Command.</value>
        public ChatCommand Command { get; }
    }
}