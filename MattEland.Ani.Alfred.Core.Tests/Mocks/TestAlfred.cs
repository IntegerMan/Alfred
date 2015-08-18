﻿// ---------------------------------------------------------
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
            get { return Subsystems.SelectMany(subsystem => subsystem.Pages); }
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
    }
}