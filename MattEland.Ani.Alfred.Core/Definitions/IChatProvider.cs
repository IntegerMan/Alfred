// ---------------------------------------------------------
// IChatProvider.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/03/2015 at 1:00 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     An <see langword="interface"/> defining a class that can attempt handle user input in
    ///     text form
    /// </summary>
    public interface IChatProvider
    {

        /// <summary>
        ///     Gets the last response from the system.
        /// </summary>
        /// <value>
        /// The last response.
        /// </value>
        [CanBeNull]
        UserStatementResponse LastResponse { get; }

        /// <summary>
        ///     Gets the last input from the user.
        /// </summary>
        /// <value>
        /// The last input.
        /// </value>
        [CanBeNull]
        string LastInput { get; }

        /// <summary>
        ///     Handles a user statement.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The response to the user statement</returns>
        [NotNull]
        UserStatementResponse HandleUserStatement([NotNull] string userInput);

        /// <summary>
        ///     Handles events from the framework.
        /// </summary>
        /// <param name="frameworkEvent">The event.</param>
        void HandleFrameworkEvent(FrameworkEvents frameworkEvent);
    }
}