// ---------------------------------------------------------
// TestChatProvider.cs
// 
// Created on:      09/02/2015 at 6:20 PM
// Last Modified:   09/03/2015 at 1:05 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Tests.Mocks
{
    /// <summary>
    ///     A test implementation of <see cref="IChatProvider" />
    /// </summary>
    public class TestChatProvider : IChatProvider
    {

        /// <summary>
        ///     Handles events from the framework.
        /// </summary>
        /// <param name="frameworkEvent">The event.</param>
        public void HandleFrameworkEvent(FrameworkEvent frameworkEvent) { }

        /// <summary>
        ///     Handles a user statement.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The response to the user statement</returns>
        public UserStatementResponse HandleUserStatement(string userInput)
        {
            var response = new UserStatementResponse();

            LastInput = userInput;
            LastResponse = response;

            return response;
        }

        /// <summary>
        ///     Gets the last input from the user.
        /// </summary>
        /// <value>
        /// The last input.
        /// </value>
        public string LastInput { get; private set; }

        /// <summary>
        ///     Gets the last response from the system.
        /// </summary>
        /// <value>
        /// The last response.
        /// </value>
        public UserStatementResponse LastResponse { get; private set; }

        /// <summary>
        ///     Does an initial greeting to the user without requiring any input. This should set
        ///     <see cref="LastInput"/> to null.
        /// </summary>
        public void DoInitialGreeting() { }
    }
}