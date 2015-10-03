using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Ani.Alfred.PresentationAvalon.Commands;
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

            ConfigureTestContainer(AlfredContainer);
        }

        /// <summary>
        ///     Registers the default Alfred <paramref name="container" /> with good default testing
        ///     values.
        /// </summary>
        /// <param name="container"> The container. </param>
        public void ConfigureTestContainer([NotNull] IAlfredContainer container)
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
                var alfred = AlfredContainer.Alfred;
                alfred.ShouldNotBeNull("Could not find the Alfred instance");

                return alfred;
            }
            set
            {
                AlfredContainer.Alfred = value;
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
        protected ApplicationManager StartAlfred()
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
            var app = new ApplicationManager(AlfredContainer, options);
            var alfred = app.Alfred;

            // Register the application instance in the container
            app.RegisterAsProvidedInstance(AlfredContainer);

            // Start up Alfred
            alfred.ShouldNotBeNull();
            alfred.Initialize();

            return app;
        }

        /// <summary>
        ///     Gets the Alfred container.
        /// </summary>
        /// <value>
        ///     The Alfred container.
        /// </value>
        [NotNull]
        public IAlfredContainer AlfredContainer
        {
            get
            {
                return Container as IAlfredContainer;
            }
        }

        /// <summary>
        ///     Builds the container.
        /// </summary>
        /// <returns>
        ///     An IObjectContainer.
        /// </returns>
        protected override IObjectContainer BuildContainer()
        {
            return new AlfredContainer();
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
        [SuppressMessage("ReSharper", "RedundantEmptyObjectOrCollectionInitializer")]
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
            return new SimpleSubsystem(AlfredContainer, "Test Subsystem");
        }

        /// <summary>
        ///     Creates an Alfred instance.
        /// </summary>
        /// <returns>The new Alfred instance.</returns>
        [NotNull]
        protected AlfredApplication BuildAlfredInstance()
        {
            return new AlfredApplication(AlfredContainer);
        }

        /// <summary>
        ///     Builds a Subsystem for module testing. This subsystem only exists to provide a specific
        ///     module a legitimate testing context within the Alfred Application Framework's context.
        /// </summary>
        /// <param name="module"> </param>
        /// <returns>
        ///     A SimpleSubsystem.
        /// </returns>
        protected SimpleSubsystem BuildSubsystemForModule(IAlfredModule module)
        {
            var page = new ModuleListPage(AlfredContainer, "Test Page", "TestPage");
            page.Register(module);

            var subsystem = BuildTestSubsystem();
            subsystem.PagesToRegister.Add(page);

            return subsystem;
        }

        /// <summary>
        ///     Gets the console from the <see cref="Container"/>. If the console does not exist a
        ///     <see cref="NullReferenceException"/> will be thrown.
        /// </summary>
        /// <value>
        ///     The console.
        /// </value>
        [NotNull]
        protected IConsole Console
        {
            get { return Container.Provide<IConsole>(); }
        }

        /// <summary>
        ///     Builds the application.
        /// </summary>
        /// <returns>
        ///     An ApplicationManager.
        /// </returns>
        [NotNull]
        protected ApplicationManager BuildApplicationInstance()
        {
            return new ApplicationManager(AlfredContainer, BuildOptions());
        }
    }

}