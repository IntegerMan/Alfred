using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.PresentationAvalon.Commands
{
    /// <summary>
    /// Defines a class capable of managing a user interface in response to application commands from the Alfred framework.
    /// </summary>
    public interface IUserInterfaceDirector
    {
        /// <summary>
        /// Handles the page navigation command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Whether or not the command was handled</returns>
        bool HandlePageNavigationCommand(ShellCommand command);
    }
}