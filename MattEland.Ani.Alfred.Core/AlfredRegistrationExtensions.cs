// ---------------------------------------------------------
// AlfredRegistrationExtensions.cs
// 
// Created on:      09/03/2015 at 12:59 PM
// Last Modified:   09/03/2015 at 1:00 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Extension methods for registering items with Alfred.
    /// </summary>
    /// <remarks>
    ///     This is to preserve clean registration syntax while still delegating registration
    ///     responsibilities to an <see cref="IRegistrationProvider"/> instance.
    /// </remarks>
    public static class AlfredRegistrationExtensions
    {
        /// <summary>
        ///     An <see cref="IAlfred"/> extension method that registers a subsystem.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="alfred"> The Alfred instance to act on. </param>
        /// <param name="subsystem"> The subsystem. </param>
        public static void Register(
            [NotNull] this IAlfred alfred, [NotNull] IAlfredSubsystem subsystem)
        {
            if (alfred == null) { throw new ArgumentNullException(nameof(alfred)); }

            alfred.RegistrationProvider.Register(subsystem);
        }

        /// <summary>
        ///     An <see cref="IAlfred"/> extension method that registers a page.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="alfred"> The Alfred instance to act on. </param>
        /// <param name="page"> The page. </param>
        public static void Register(
            [NotNull] this IAlfred alfred, [NotNull] IAlfredPage page)
        {
            if (alfred == null) { throw new ArgumentNullException(nameof(alfred)); }

            alfred.RegistrationProvider.Register(page);
        }

        /// <summary>
        ///     An <see cref="IAlfred"/> extension method that registers a chat provider.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="alfred"> The Alfred instance to act on. </param>
        /// <param name="provider"> The chat provider. </param>
        public static void Register(
            [NotNull] this IAlfred alfred, [NotNull] IChatProvider provider)
        {
            if (alfred == null) { throw new ArgumentNullException(nameof(alfred)); }

            alfred.RegistrationProvider.Register(provider);
        }

        /// <summary>
        ///     An <see cref="IAlfred"/> extension method that registers a shell.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="alfred"> The Alfred instance to act on. </param>
        /// <param name="recipient"> The command recipient. </param>
        public static void Register(
            [NotNull] this IAlfred alfred, [NotNull] IShellCommandRecipient recipient)
        {
            if (alfred == null) { throw new ArgumentNullException(nameof(alfred)); }

            alfred.RegistrationProvider.Register(recipient);
        }
    }
}