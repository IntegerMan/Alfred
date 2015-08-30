using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Common.Providers;

using NUnit.Framework;

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
    }
}