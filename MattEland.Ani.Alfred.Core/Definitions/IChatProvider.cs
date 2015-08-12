﻿using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    /// An interface defining a class that can attempt handle user input in text form
    /// </summary>
    public interface IChatProvider
    {
        /// <summary>
        /// Handles a user statement.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The response to the user statement</returns>
        [NotNull]
        UserStatementResponse HandleUserStatement([NotNull] string userInput);

        /// <summary>
        /// Gets the last response from the system.
        /// </summary>
        /// <value>The last response.</value>
        [CanBeNull]
        UserStatementResponse LastResponse { get; }

        /// <summary>
        /// Gets the last input from the user.
        /// </summary>
        /// <value>The last input.</value>
        [CanBeNull]
        string LastInput { get; }

        /// <summary>
        /// Does an initial greeting to the user without requiring any input. This should set LastInput to null.
        /// </summary>
        void DoInitialGreeting();
    }
}