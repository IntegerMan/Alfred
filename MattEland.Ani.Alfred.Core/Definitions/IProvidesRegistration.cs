using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Interface defining a component that provides registration for other components.
    /// </summary>
    public interface IProvidesRegistration
    {
        /// <summary>
        ///     Registers the <paramref name="page"/> as a root page.
        /// </summary>
        /// <param name="page">The page.</param>
        void Register([NotNull] IAlfredPage page);

        /// <summary>
        ///     Registers the <paramref name="shell"/> command recipient that will allow the
        ///     <paramref name="shell"/> to get commands from the Alfred layer.
        /// </summary>
        /// <param name="shell">The command recipient.</param>
        void Register([NotNull] IShellCommandRecipient shell);

        /// <summary>
        ///     Registers the user statement handler as the framework's user statement handler.
        /// </summary>
        /// <param name="chatProvider">The user statement handler.</param>
        void Register([NotNull] IChatProvider chatProvider);

        /// <summary>
        ///     Registers a sub system with Alfred.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        void Register([NotNull] IAlfredSubsystem subsystem);

    }
}