// ---------------------------------------------------------
// TestAlfred.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:48 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Tests.Mocks
{
    /// <summary>
    ///     A mock implementation of Alfred for testing purposes
    /// </summary>
    public class TestAlfred : IAlfred
    {

        [NotNull]
        private readonly ICollection<IAlfredPage> _rootPages = new List<IAlfredPage>();

        [NotNull]
        public IList<IAlfredSubsystem> SubsystemsList { get; } = new List<IAlfredSubsystem>();

        /// <summary>
        ///     Gets or sets the last command received.
        /// </summary>
        /// <value>The last command.</value>
        public ChatCommand LastCommand { get; set; }

        /// <summary>
        ///     Gets the chat provider.
        /// </summary>
        /// <value>The chat provider.</value>
        public IChatProvider ChatProvider { get; private set; }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IObjectContainer Container
        {
            get { return CommonProvider.Container; }
        }

        /// <summary>
        ///     Gets the console provider. This can be null.
        /// </summary>
        /// <value>The console.</value>
        [NotNull]
        public IConsole Console { get; } = new SimpleConsole();

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Status = AlfredStatus.Initializing;

            foreach (var subsystem in Subsystems) { subsystem.Initialize(this); }

            Status = AlfredStatus.Online;
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
        ///     Gets the locale.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
        public CultureInfo Locale
        {
            get { return CultureInfo.CurrentCulture; }
        }

        /// <summary>
        ///     Registers the page as a root page.
        /// </summary>
        /// <param name="page">The page.</param>
        public void Register(IAlfredPage page) { _rootPages.Add(page); }

        /// <summary>
        ///     Registers the shell command recipient that will allow the shell to get commands from the Alfred
        ///     layer.
        /// </summary>
        /// <param name="shell">The command recipient.</param>
        public void Register(IShellCommandRecipient shell) { ShellCommandHandler = shell; }

        /// <summary>
        ///     Registers the user statement handler as the framework's user statement handler.
        /// </summary>
        /// <param name="chatProvider">The user statement handler.</param>
        public void Register(IChatProvider chatProvider) { ChatProvider = chatProvider; }

        /// <summary>
        ///     Registers a subsystem with Alfred.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        public void Register(IAlfredSubsystem subsystem) { SubsystemsList.Add(subsystem); }

        /// <summary>
        ///     Gets the root pages.
        /// </summary>
        /// <value>The root pages.</value>
        public IEnumerable<IAlfredPage> RootPages
        {
            get { return _rootPages; }
        }

        /// <summary>
        ///     Gets the shell command handler that can pass shell commands on to the user interface.
        /// </summary>
        /// <value>The shell command handler.</value>
        public IShellCommandRecipient ShellCommandHandler { get; private set; }

        /// <summary>
        ///     Shutdowns this instance.
        /// </summary>
        public void Shutdown()
        {
            Status = AlfredStatus.Terminating;

            foreach (var subsystem in Subsystems) { subsystem.Shutdown(); }

            Status = AlfredStatus.Offline;
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
        ///     Tells modules to take a look at their content and update as needed.
        /// </summary>
        public void Update() { foreach (var subsystem in Subsystems) { subsystem.Update(); } }

        /// <summary>
        ///     Gets the display name for use in the user interface.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        ///     Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public string ItemTypeName
        {
            get { return "Test Framework"; }
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public string Name
        {
            get { return "Test Alfred"; }
        }

        /// <summary>
        ///     Gets a list of properties provided by this item.
        /// </summary>
        /// <returns>The properties</returns>
        public IEnumerable<IPropertyItem> Properties
        {
            get { yield break; }
        }

        /// <summary>
        ///     Gets the property providers.
        /// </summary>
        /// <value>The property providers.</value>
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { yield break; }
        }

        /// <summary>
        ///     Handles a chat command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void HandleChatCommand(ChatCommand command) { LastCommand = command; }
    }
}