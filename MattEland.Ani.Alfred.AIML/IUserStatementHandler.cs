using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    /// An interface defining a class that can attempt handle user input in text form
    /// </summary>
    public interface IUserStatementHandler
    {
        /// <summary>
        /// Handles a user statement.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The response to the user statement</returns>
        [NotNull]
        UserStatementResponse HandleUserStatement([NotNull] string userInput);
    }
}