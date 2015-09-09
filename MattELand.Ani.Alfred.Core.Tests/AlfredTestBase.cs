using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Common;
using MattEland.Common.Providers;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests
{
    /// <summary>
    /// Represents common logic useful for helping initialize Alfred test classes
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
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

            ConfigureTestContainer(Container);
        }

        /// <summary>
        ///     Registers the default Alfred <paramref name="container" /> with good default testing
        ///     values.
        /// </summary>
        /// <param name="container"> The container. </param>
        public void ConfigureTestContainer([NotNull] IObjectContainer container)
        {
            var console = new DiagnosticConsole(container);
            console.RegisterAsProvidedInstance(typeof(IConsole), container);

            container.ApplyDefaultAlfredMappings();

            // Register mappings for promised types
            container.Register(typeof(IConsoleEvent), typeof(ConsoleEvent));
            container.Register(typeof(IAlfredCommand), typeof(AlfredCommand));
            container.Register(typeof(MetricProviderBase), typeof(ValueMetricProvider));
            container.Register(typeof(ISearchController), typeof(AlfredSearchController));

            // We'll want to get at the same factory any time we request a factory for test purposes
            var factory = new ValueMetricProviderFactory();
            factory.RegisterAsProvidedInstance(typeof(IMetricProviderFactory), container);
            factory.RegisterAsProvidedInstance(typeof(ValueMetricProviderFactory), container);
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
            set
            {
                value.RegisterAsProvidedInstance(typeof(IAlfred), Container);
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
        protected SimpleSubsystem TestSubsystem { get; set; }

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
            TestSubsystem = BuildTestSubsystem();
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
        protected virtual void PrepareTestSubsystem([NotNull] SimpleSubsystem testSubsystem)
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
        protected static ApplicationManagerOptions BuildOptions()
        {
            var options = new ApplicationManagerOptions
            {
                IsSpeechEnabled = false,
                ShowMindExplorerPage = true,
                AdditionalSubsystems = { }
            };

            return options;
        }

        /// <summary>
        ///     Builds a page mock.
        /// </summary>
        /// <param name="mockBehavior"> The mocking behavior for the new mock. </param>
        /// <returns>
        ///     The mock page.
        /// </returns>
        protected Mock<IAlfredPage> BuildPageMock(MockBehavior mockBehavior)
        {
            // Some tests will want strict control over mocking and others won't
            var mock = new Mock<IAlfredPage>(mockBehavior);

            // Set up simple members we expect to be hit during startup
            mock.SetupGet(p => p.IsRootLevel).Returns(true);
            mock.Setup(p => p.OnRegistered(It.IsAny<IAlfred>()));
            mock.Setup(p => p.OnInitializationCompleted());
            mock.Setup(p => p.Update());
            mock.Setup(p => p.OnShutdownCompleted());

            // When initialize is hit, set Status to Online
            mock.Setup(p => p.Initialize(It.IsAny<IAlfred>()))
                .Callback(() => mock.SetupGet(p => p.Status)
                                    .Returns(AlfredStatus.Online));

            // When shutdown is hit, set Status to Offline
            mock.Setup(p => p.Shutdown())
                .Callback(() => mock.SetupGet(p => p.Status)
                                    .Returns(AlfredStatus.Offline));

            return mock;
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

        /// <summary>
        ///     Builds a <see cref="SimpleSubsystem"/> for testing.
        /// </summary>
        /// <returns>
        ///     The subsystem.
        /// </returns>
        protected SimpleSubsystem BuildTestSubsystem()
        {
            return new SimpleSubsystem(Container, "Test Subsystem");
        }

        /// <summary>
        ///     Builds a mock <see cref="IAlfredSubsystem"/>.
        /// </summary>
        /// <param name="mockBehavior"> The mocking behavior. </param>
        /// <returns>
        ///     A mock subsystem
        /// </returns>
        protected Mock<IAlfredSubsystem> BuildMockSubsystem(MockBehavior mockBehavior)
        {
            // Build the Mock
            var mock = new Mock<IAlfredSubsystem>(mockBehavior);

            // Setup Simple properties
            mock.SetupGet(s => s.Id).Returns("Test");
            mock.SetupGet(s => s.NameAndVersion).Returns("Test 1.0.0.0");
            mock.SetupGet(s => s.Status).Returns(AlfredStatus.Offline);
            mock.SetupGet(s => s.Pages).Returns(Container.ProvideCollection<IAlfredPage>());

            // Setup simple methods
            mock.Setup(s => s.Update());
            mock.Setup(s => s.OnRegistered(It.IsAny<IAlfred>()));
            mock.Setup(s => s.OnInitializationCompleted());
            mock.Setup(s => s.OnShutdownCompleted());

            // Initialize causes the subsystem to go online
            mock.Setup(s => s.Initialize(It.IsAny<IAlfred>()))
                .Callback(() => mock.SetupGet(s => s.Status).Returns(AlfredStatus.Online));

            // Shutdown causes the subsystem to go offline
            mock.Setup(s => s.Shutdown())
                .Callback(() => mock.SetupGet(s => s.Status).Returns(AlfredStatus.Offline));

            return mock;
        }

        /// <summary>
        ///     Creates an Alfred instance.
        /// </summary>
        /// <returns>
        ///     The new Alfred instance.
        /// </returns>
        [NotNull]
        protected AlfredApplication CreateAlfredInstance()
        {
            return new AlfredApplication(Container);
        }
    }
}