// ---------------------------------------------------------
// IShellCommandRecipient.cs
// 
// Created on:      08/18/2015 at 10:59 PM
// Last Modified:   08/18/2015 at 10:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------
namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    /// Represents an object capable of handling Alfred Shell commands
    /// </summary>
    public interface IShellCommandRecipient
    {

        /// <summary>
        /// Processes a shell command by sending it on to the user interface layer.
        /// </summary>
        /// <param name="command">The command.</param>
        void ProcessShellCommand(ShellCommand command);

    }
}