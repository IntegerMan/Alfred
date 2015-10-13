// ---------------------------------------------------------
// AlfredSearchResult.cs
// 
// Created on:      10/12/2015 at 10:37 PM
// Last Modified:   10/12/2015 at 10:37 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using MattEland.Common.Annotations;
using System.Diagnostics.Contracts;

using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Represents a search result within the Alfred system.
    /// </summary>
    public class AlfredSearchResult : SearchResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResult"/> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="title"> The title. </param>
        public AlfredSearchResult([NotNull] IAlfredContainer container, [NotNull] string title)
            : base(container, title)
        {
            Contract.Requires(container != null);
            Contract.Requires(title.HasText());

            _container = container;

        }

        /// <summary>
        ///     The Alfred container.
        /// </summary>
        [NotNull]
        private readonly IAlfredContainer _container;

        /// <summary>
        /// Builds the more details action.
        /// </summary>
        /// <returns>An action</returns>
        protected override Action BuildMoreDetailsAction()
        {
            {
                var router = _container.CommandRouter;
                var shell = _container.Shell;

                // Build out a command handler
                return () =>
                {
                    // Build out a command to navigate the browser widget to the proper locationText
                    var result = new AlfredCommandResult();
                    var command = new ChatCommand("Core", "Browse", Url);

                    router.ProcessAlfredCommand(command, result);

                    /* Tell the shell to navigate as well. This is separate from the prior command since
                       we don't want the user interface to have specific knowledge of its contents. */
                    if (shell != null)
                    {
                        var shellCommand = new ShellCommand("Nav", "Pages", "Browser");
                        shell.ProcessShellCommand(shellCommand);
                    }
                };
            }
        }
    }
}