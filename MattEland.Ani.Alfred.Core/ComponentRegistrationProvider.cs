// ---------------------------------------------------------
// ComponentRegistrationProvider.cs
// 
// Created on:      09/03/2015 at 12:34 PM
// Last Modified:   09/03/2015 at 12:36 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     This registration provider handles component registration capabilities for Alfred.
    /// </summary>
    internal class ComponentRegistrationProvider : IRegistrationProvider
    {
        [NotNull]
        private readonly AlfredApplication _alfred;

        [NotNull]
        [ItemNotNull]
        private readonly ICollection<IAlfredPage> _rootPages;

        [NotNull]
        [ItemNotNull]
        private readonly ICollection<IAlfredSubsystem> _subsystems;

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of the <see cref="ComponentRegistrationProvider" />
        ///     </para>
        ///     <para>class.</para>
        /// </summary>
        /// <param name="alfred">The alfred instance.</param>
        /// <param name="subsystems">The subsystems instance.</param>
        /// <param name="rootPages">The root pages instance.</param>
        internal ComponentRegistrationProvider(
            [NotNull] AlfredApplication alfred,
            [NotNull] ICollection<IAlfredSubsystem> subsystems,
            [NotNull] ICollection<IAlfredPage> rootPages)
        {
            _alfred = alfred;
            _subsystems = subsystems;
            _rootPages = rootPages;
        }

        /// <summary>
        ///     Registers the <paramref name="page" /> as a root page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        public void Register([NotNull] IAlfredPage page)
        {
            if (page == null) { throw new ArgumentNullException(nameof(page)); }

            if (page.IsRootLevel) { _rootPages.Add(page); }

            page.OnRegistered(_alfred);
        }

        /// <summary>
        ///     Registers the <paramref name="shell" /> command recipient that will allow the
        ///     <paramref name="shell" /> to get commands from the Alfred layer.
        /// </summary>
        /// <remarks>
        ///     TODO: We're not really doing much with <paramref name="shell" /> yet. That might
        ///     need to be tweaked later.
        /// </remarks>
        /// <param name="shell">The command recipient.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="InvalidOperationException">Thrown when Alfred is online.</exception>
        public void Register([NotNull] IShellCommandRecipient shell)
        {
            if (shell == null) { throw new ArgumentNullException(nameof(shell)); }

            AssertNotOnline();

            _alfred.ShellCommandHandler = shell;
        }

        /// <summary>
        ///     Registers the user statement handler as the framework's user statement handler.
        /// </summary>
        /// <param name="chatProvider">The user statement handler.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        public void Register([NotNull] IChatProvider chatProvider)
        {
            if (chatProvider == null) { throw new ArgumentNullException(nameof(chatProvider)); }

            _alfred.ChatProvider = chatProvider;
        }

        /// <summary>
        ///     Registers a sub system with Alfred.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="InvalidOperationException">Thrown when Alfred is online.</exception>
        public void Register(IAlfredSubsystem subsystem)
        {
            if (subsystem == null) { throw new ArgumentNullException(nameof(subsystem)); }

            AssertNotOnline();

            _subsystems.AddSafe(subsystem);
            subsystem.OnRegistered(_alfred);
        }

        /// <summary>
        ///     <para>
        ///         Checks if Alfred is offline and throws an <see cref="InvalidOperationException" />
        ///     </para>
        ///     <para>if he isn't.</para>
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when Alfred is online.</exception>
        private void AssertNotOnline()
        {
            if (_alfred.Status != AlfredStatus.Offline)
            {
                throw new InvalidOperationException("Alfred must be offline to register components");
            }
        }
    }
}