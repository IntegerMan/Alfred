// ---------------------------------------------------------
// TestAlfred.cs
// 
// Created on:      08/17/2015 at 11:48 PM
// Last Modified:   08/17/2015 at 11:54 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.SubSystems;

namespace MattEland.Ani.Alfred.Tests.Mocks
{
    /// <summary>
    /// A mock implementation of Alfred for testing purposes
    /// </summary>
    public class TestAlfred : IAlfred
    {
        [NotNull]
        private readonly IConsole _console = new SimpleConsole();

        [NotNull]
        private readonly IPlatformProvider _platformProvider = new SimplePlatformProvider();
        private IChatProvider _chatProvider;
        private IShellCommandRecipient _shellCommandHandler;

        [NotNull]
        private readonly ICollection<IAlfredPage> _rootPages = new List<IAlfredPage>();

        [NotNull]
        public IList<IAlfredSubsystem> SubsystemsList { get; } = new List<IAlfredSubsystem>();

        /// <summary>
        ///     Gets the console provider. This can be null.
        /// </summary>
        /// <value>The console.</value>
        public IConsole Console
        {
            get { return _console; }
        }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public AlfredStatus Status { get; set; }

        /// <summary>
        ///     Gets the sub systems associated wih Alfred.
        /// </summary>
        /// <value>The sub systems.</value>
        public IEnumerable<IAlfredSubsystem> Subsystems
        {
            get { return SubsystemsList; }
        }

        /// <summary>
        ///     Gets the root pages.
        /// </summary>
        /// <value>The root pages.</value>
        public IEnumerable<IAlfredPage> RootPages
        {
            get { return _rootPages; }
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "Test Alfred"; }
        }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Status = AlfredStatus.Initializing;

            foreach (var subsystem in Subsystems)
            {
                subsystem.Initialize(this);
            }

            Status = AlfredStatus.Online;
        }

        /// <summary>
        ///     Shutdowns this instance.
        /// </summary>
        public void Shutdown()
        {
            Status = AlfredStatus.Terminating;

            foreach (var subsystem in Subsystems)
            {
                subsystem.Shutdown();
            }

            Status = AlfredStatus.Offline;
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is online.
        /// </summary>
        /// <value><c>true</c> if this instance is online; otherwise, <c>false</c>.</value>
        public bool IsOnline
        {
            get { return Status == AlfredStatus.Online; }
        }

        /// <summary>
        ///     Gets the chat provider.
        /// </summary>
        /// <value>The chat provider.</value>
        public IChatProvider ChatProvider
        {
            get { return _chatProvider; }
        }

        /// <summary>
        ///     Gets the platform provider.
        /// </summary>
        /// <value>The platform provider.</value>
        public IPlatformProvider PlatformProvider
        {
            get { return _platformProvider; }
        }

        /// <summary>
        /// Registers the page as a root page.
        /// </summary>
        /// <param name="page">The page.</param>
        public void Register(IAlfredPage page)
        {
            _rootPages.Add(page);
        }

        /// <summary>
        /// Gets the shell command handler that can pass shell commands on to the user interface.
        /// </summary>
        /// <value>The shell command handler.</value>
        public IShellCommandRecipient ShellCommandHandler
        {
            get { return _shellCommandHandler; }
        }

        /// <summary>
        /// Registers the shell command recipient that will allow the shell to get commands from the Alfred layer.
        /// </summary>
        /// <param name="shell">The command recipient.</param>
        public void Register(IShellCommandRecipient shell)
        {
            _shellCommandHandler = shell;
        }

        /// <summary>
        ///     Registers the user statement handler as the framework's user statement handler.
        /// </summary>
        /// <param name="chatProvider">The user statement handler.</param>
        public void Register(IChatProvider chatProvider)
        {
            _chatProvider = chatProvider;
        }

        /// <summary>
        ///     Registers a sub system with Alfred.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        public void Register(AlfredSubsystem subsystem)
        {
            SubsystemsList.Add(subsystem);
        }

        /// <summary>
        ///     Tells modules to take a look at their content and update as needed.
        /// </summary>
        public void Update()
        {
            foreach (var subsystem in Subsystems)
            {
                subsystem.Update();
            }
        }

        /// <summary>
        /// Handles a chat command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void HandleChatCommand(ChatCommand command)
        {
            LastCommand = command;
        }

        /// <summary>
        /// Gets or sets the last command received.
        /// </summary>
        /// <value>The last command.</value>
        public ChatCommand LastCommand
        {
            get; set;
        }
    }
}