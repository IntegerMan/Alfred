using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Common;
using MattEland.Common.Providers;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests
{
    /// <summary>
    /// Represents common logic useful for helping initialize Alfred test classes
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public abstract class AlfredTestBase
    {
        [NotNull]
        private IObjectContainer _container;

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public virtual void SetUp()
        {
            _container = new CommonContainer();
            _container.RegisterDefaultAlfredMappings();
        }

        /// <summary>
        /// Gets the <see cref="IObjectContainer"/> used by the test.
        /// </summary>
        /// <value>The container.</value>
        [NotNull]
        protected IObjectContainer Container
        {
            get
            {
                return _container;
            }
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
        ///     Creates and starts up the <see cref="IAlfred"/> instance.
        /// </summary>
        /// <returns>
        ///     The Alfred instance.
        /// </returns>
        [NotNull]
        protected AlfredApplication StartAlfred()
        {
            // Build the Application
            var app = new ApplicationManager(Container);
            var alfred = app.Alfred;

            // Register the application instance in the container
            app.RegisterAsProvidedInstance(Container);

            // Start up Alfred
            alfred.ShouldNotBeNull();
            alfred.Initialize();

            return alfred;
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

    }
}