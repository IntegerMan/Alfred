// ---------------------------------------------------------
// TestShell.cs
// 
// Created on:      08/19/2015 at 12:08 AM
// Last Modified:   08/19/2015 at 12:08 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Tests.Mocks
{
    /// <summary>
    /// A fake user interface controller for testing
    /// </summary>
    public class TestShell : IShellCommandRecipient
    {

        /// <summary>
        /// Processes a shell command by sending it on to the user interface layer.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The redirect target or string.empty for no redirect.</returns>
        public string ProcessShellCommand(ShellCommand command)
        {
            if (command.Name.Matches("Nav"))
            {
                if (command.Target.Matches("Pages"))
                {
                    // We'll pretend it worked unless the data contained fail. Why not?
                    if (!command.Data.ToUpperInvariant().Contains("FAIL"))
                    {
                        return "Navigate Success";
                    }
                }

                // If we got here, its a navigate and it didn't succeed
                return "Navigate Failed";
            }

            // Default to string.empty
            return string.Empty;
        }
    }
}