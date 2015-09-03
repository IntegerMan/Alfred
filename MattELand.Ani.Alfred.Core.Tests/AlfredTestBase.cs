using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Ani.Alfred.Tests.Mocks;
using MattEland.Common;
using MattEland.Common.Providers;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests
{
    /// <summary>
    /// Represents common logic useful for helping initialize Alfred test classes
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public abstract class AlfredTestBase : UnitTestBase
    {
        /// <summary>
        ///     Sets up the test fixture.
        /// </summary>
        [TestFixtureSetUp]
        public override void SetUpFixture()
        {
            base.SetUpFixture();
        }

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Container.RegisterDefaultAlfredMappings();
        }

        /// <summary>
        ///     Gets the Alfred instance.
        /// </summary>
        /// <seealso cref="IAlfred"/>
        /// <value>
        ///     The Alfred instance.
        /// </value>
        [NotNull]
        public IAlfred Alfred
        {
            get
            {
                var alfred = Container.Provide<IAlfred>();
                alfred.ShouldNotBeNull("Could not find the Alfred instance");

                return alfred;
            }
        }

        /// <summary>
        ///     Gets the <see cref="ChatEngine"/>.
        /// </summary>
        /// <value>
        ///     The chat engine.
        /// </value>
        [NotNull]
        public ChatEngine ChatEngine
        {
            get
            {
                Container.HasMapping(typeof(ChatEngine)).ShouldBe(true, "Could not find the chat engine");

                return Container.Provide<ChatEngine>();
            }
        }

        /// <summary>
        ///     Gets or sets the test subsystem.
        /// </summary>
        /// <value>
        ///     The test subsystem.
        /// </value>
        protected TestSubsystem TestSubsystem { get; set; }

        /// <summary>
        ///     Creates and starts up the <see cref="IAlfred"/> instance.
        /// </summary>
        /// <returns>
        ///     The Alfred instance.
        /// </returns>
        [NotNull]
        protected AlfredApplication StartAlfred()
        {
            // Create test subsystem
            TestSubsystem = new TestSubsystem(Container);
            TestSubsystem.RegisterAsProvidedInstance(Container);

            // Allow individual tests to customize the Test Subsystem as needed
            PrepareTestSubsystem(TestSubsystem);

            // Build out options. Ensure the test subsystem is present
            var options = BuildOptions();
            options.IsSpeechEnabled = false;
            options.AdditionalSubsystems.Add(TestSubsystem);

            // Build the Application
            var app = new ApplicationManager(Container, options);
            var alfred = app.Alfred;

            // Register the application instance in the container
            app.RegisterAsProvidedInstance(Container);

            // Start up Alfred
            alfred.ShouldNotBeNull();
            alfred.Initialize();

            return alfred;
        }

        /// <summary>
        ///     Prepare the test subsystem prior to registration and startup.
        /// </summary>
        /// <param name="testSubsystem"> The test subsystem. </param>
        protected virtual void PrepareTestSubsystem([NotNull] TestSubsystem testSubsystem)
        {
            // Do nothing. Individual tests can manipulate this as needed via overrides
        }

        /// <summary>
        ///     Gets a subsystem.
        /// </summary>
        /// <param name="subsystemId"> The subsystem's Id. </param>
        /// <returns>
        ///     The subsystem.
        /// </returns>
        [NotNull]
        protected IAlfredSubsystem GetSubsystem(string subsystemId)
        {
            var alfred = Container.Provide<IAlfred>();

            var subsystem = alfred.Subsystems.FirstOrDefault(s => s.Id.Matches(subsystemId));
            subsystem.ShouldNotBeNull($"The Subsystem with id '{subsystemId}' could not be found");

            return subsystem;
        }

        /// <summary>
        ///     Gets a subsystem and casts it to the specified type.
        /// </summary>
        /// <typeparam name="T"> The type the subsystem should be cast to. </typeparam>
        /// <param name="subsystemId"> The subsystem's Id. </param>
        /// <returns>
        ///     The subsystem.
        /// </returns>
        [NotNull]
        protected T GetSubsystem<T>(string subsystemId)
        {
            // Do the basic search for the subsystem
            var subsystem = GetSubsystem(subsystemId);

            // Cast and verify
            var typedSubsystem = subsystem.ShouldBeOfType<T>();
            typedSubsystem.ShouldNotBeNull();

            return typedSubsystem;
        }

        /// <summary>
        ///     Gets the <see cref="IConsole"/> and fails if the console is not available.
        /// </summary>
        /// <returns>
        ///     The console.
        /// </returns>
        [NotNull]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        protected IConsole RequireConsole()
        {
            var console = Container.TryProvide<IConsole>();

            console.ShouldNotBeNull();

            return console;
        }

        /// <summary>
        ///     Builds the options for creating an <see cref="ApplicationManager"/>.
        /// </summary>
        /// <returns>The <see cref="ApplicationManagerOptions" />.</returns>
        [NotNull]
        protected ApplicationManagerOptions BuildOptions()
        {
            var options = new ApplicationManagerOptions { IsSpeechEnabled = false };

            return options;
        }

        /// <summary>
        ///     Builds test page.
        /// </summary>
        /// <param name="isRoot"> <see langword="true"/> if this instance is root. </param>
        /// <returns>
        ///     A <see cref="TestPage"/>.
        /// </returns>
        [NotNull]
        protected TestPage BuildTestPage(bool isRoot)
        {
            return new TestPage(Container) { IsRootLevel = isRoot };
        }

        /// <summary>
        ///     Gets the <see cref="IConsole"/> instance.
        /// </summary>
        /// <returns>
        ///     The console.
        /// </returns>
        protected IConsole GetConsole()
        {
            var console = Container.TryProvide<IConsole>();

            console.ShouldNotBeNull($"Could not find a console in container {Container}");

            return console;
        }
    }
}