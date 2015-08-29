﻿using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Common.Providers;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests
{
    /// <summary>
    /// Represents common logic useful for helping initialize Alfred test classes
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
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
                // TODO: It might be a good idea to create a new container for tests
                return _container;
            }
        }
    }
}