using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common.Providers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Interface for an Alfred-specific container implementation.
    /// </summary>
    public interface IAlfredContainer : IObjectContainer
    {
        /// <summary>
        ///     Builds a command.
        /// </summary>
        /// <param name="action"> The action the command will execute. </param>
        /// <returns>
        ///     A command.
        /// </returns>
        [NotNull]
        IAlfredCommand BuildCommand([CanBeNull] Action action = null);

        /// <summary>
        ///     Gets or sets the command router responsible for routing <see cref="ChatCommand"/> objects
        ///     to their intended recipient.
        /// </summary>
        /// <value>
        ///     The command router.
        /// </value>
        [NotNull]
        IAlfredCommandRecipient CommandRouter { get; set; }

        /// <summary>
        ///     Gets or sets the localization culture.
        /// </summary>
        /// <value>
        ///     The locale.
        /// </value>
        [NotNull]
        CultureInfo Locale { get; set; }

        /// <summary>
        ///     Gets or sets the console.
        /// </summary>
        /// <value>
        ///     The console.
        /// </value>
        [NotNull]
        IConsole Console { get; set; }

        /// <summary>
        ///     Gets the chat provider.
        /// </summary>
        /// <value>
        ///     The chat provider.
        /// </value>
        [NotNull]
        IChatProvider ChatProvider { get; }

        /// <summary>
        ///     Gets or sets the search controller.
        /// </summary>
        /// <value>
        ///     The search controller.
        /// </value>
        [NotNull]
        ISearchController SearchController { get; set; }

        /// <summary>
        ///     Gets the shell.
        /// </summary>
        /// <value>
        ///     The shell.
        /// </value>
        [NotNull]
        IShellCommandRecipient Shell { get; }

        /// <summary>
        ///     Gets the Alfred instance.
        /// </summary>
        /// <value>
        ///     The Alfred.
        /// </value>
        [CanBeNull]
        IAlfred Alfred { get; set; }

        /// <summary>
        ///     Gets the error manager.
        /// </summary>
        /// <value>
        ///     The error manager.
        /// </value>
        [NotNull]
        ErrorManager ErrorManager { get; }

        /// <summary>
        ///     Handles an encountered exception.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <param name="errorCode"> The error code. </param>
        /// <param name="additionalMessage"> An additional message. </param>
        /// <returns>
        ///     An ErrorInstance.
        /// </returns>
        [NotNull]
        ErrorInstance HandleException([NotNull] Exception exception,
            [CanBeNull] string errorCode,
            [CanBeNull] string additionalMessage);

        /// <summary>
        ///     Handles an encountered exception.
        /// </summary>
        ///
        /// <param name="exception"> The exception. </param>
        /// <param name="errorCode"> The error code. </param>
        /// <returns>
        ///     An ErrorInstance.
        /// </returns>
        [NotNull]
        ErrorInstance HandleException([NotNull] Exception exception,
            [CanBeNull] string errorCode);

    }
}
