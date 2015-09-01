// ---------------------------------------------------------
// UnitTestBase.cs
// 
// Created on:      09/01/2015 at 1:17 PM
// Last Modified:   09/01/2015 at 1:22 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Common.Providers;

using NUnit.Framework;

namespace MattEland.Testing
{
    /// <summary>A <see langword="base" /> class for all unit tests</summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public abstract class UnitTestBase
    {
        [NotNull]
        private IObjectContainer _container;

        /// <summary>Gets the random number generator.</summary>
        /// <remarks>
        ///     The random number generator is re-used between tests and set up at test fixture setup to
        ///     avoid the same number being generated repetitively.
        /// </remarks>
        /// <value>The randomizer.</value>
        [NotNull]
        public Random Randomizer { get; set; }

        /// <summary>Gets the <see cref="IObjectContainer" /> used by the test.</summary>
        /// <value>The container.</value>
        [NotNull]
        protected IObjectContainer Container
        {
            get { return _container; }
        }

        /// <summary>Sets up the test fixture.</summary>
        [TestFixtureSetUp]
        public virtual void SetUpFixture() { Randomizer = new Random(); }

        /// <summary>Sets up the environment for each test.</summary>
        [SetUp]
        public virtual void SetUp() { _container = new CommonContainer(); }
    }
}