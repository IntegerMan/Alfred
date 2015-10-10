using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.PresentationCommon.Commands
{
    /// <summary>
    /// Defines a class capable of managing a user interface in response to application commands from the Alfred framework.
    /// </summary>
    public interface IUserInterfaceDirector
    {
        /// <summary>
        /// Handles the event when a web page is requested.
        /// </summary>
        /// <param name="url">The URL that was requested.</param>
        void HandleWebPageRequested([NotNull] string url);

        /// <summary>
        /// Handles the page navigation command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Whether or not the command was handled</returns>
        bool HandlePageNavigationCommand(ShellCommand command);
    }
}