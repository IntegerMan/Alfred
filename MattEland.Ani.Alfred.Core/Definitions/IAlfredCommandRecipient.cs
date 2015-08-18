using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    /// An interface marking the target as something that cares about Alfred Commands.
    /// </summary>
    public interface IAlfredCommandRecipient
    {
        /// <summary>
        /// Processes an Alfred Command. If the command is handled, result should be modified accordingly and the method should return true. Returning false will not stop the message from being propogated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The result. If the command was handled, this should be updated.</param>
        /// <returns><c>True</c> if the command was handled; otherwise false.</returns>
        bool ProcessAlfredCommand(ChatCommand command, [NotNull] AlfredCommandResult result);
    }
}