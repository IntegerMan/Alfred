// ---------------------------------------------------------
// TimeModuleTestsBase.cs
// 
// Created on:      09/06/2015 at 10:37 PM
// Last Modified:   09/06/2015 at 10:37 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Pages;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Modules
{
    /// <summary>
    ///     A base class for tests targeting <see cref="AlfredTimeModule" />
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public abstract class TimeModuleTestsBase : AlfredTestBase
    {
        /// <summary>
        ///     Gets the Alfred instance.
        /// </summary>
        /// <value>
        ///     The Alfred instance.
        /// </value>
        [NotNull]
        protected new AlfredApplication Alfred { get; private set; }

        /// <summary>
        ///     Gets the time module.
        /// </summary>
        /// <value>
        ///     The time module.
        /// </value>
        [NotNull]
        protected AlfredTimeModule Module { get; private set; }

        /// <summary>
        ///     Gets the page.
        /// </summary>
        /// <value>
        ///     The page.
        /// </value>
        [NotNull]
        public ModuleListPage Page { get; private set; }

        /// <summary>
        ///     Gets the subsystem.
        /// </summary>
        /// <value>
        ///     The subsystem.
        /// </value>
        [NotNull]
        public IAlfredSubsystem Subsystem { get; private set; }

        /// <summary>
        ///     Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Alfred = new AlfredApplication(Container);

            Page = new ModuleListPage(Container, "Time", "Time");
            Module = new AlfredTimeModule(Container);
            Page.Register(Module);

            var subsystem = BuildTestSubsystem();
            subsystem.PagesToRegister.Add(Page);
            Subsystem = subsystem;

            Alfred.RegistrationProvider.Register(Subsystem);
        }
    }
}