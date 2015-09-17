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

using MattEland.Common;
using MattEland.Common.Providers;

using NUnit.Framework;

namespace MattEland.Testing
{
    /// <summary>A <see langword="base" /> class for all unit tests</summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public abstract class UnitTestBase : IHasContainer
    {
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
        public IObjectContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new CommonContainer();
                }

                // Ensure the name is accurate
                _container.Name = CurrentTestName;

                return _container;

            }
            set { _container = value; }
        }

        /// <summary>
        ///     Gets the current test's name.
        /// </summary>
        /// <value>
        ///     The name of the current test's name.
        /// </value>
        [NotNull]
        public string CurrentTestName
        {
            get
            {
                var currentContext = TestContext.CurrentContext;

                // Sanity check in an uncertain land
                currentContext.ShouldNotBeNull();
                currentContext.Test.ShouldNotBeNull();

                return $"Con_{currentContext.Test.Name.NonNull()}";
            }
        }

        /// <summary>Sets up the test fixture.</summary>
        [TestFixtureSetUp]
        public virtual void SetUpFixture() { Randomizer = new Random(); }

        /// <summary>Sets up the environment for each test.</summary>
        [SetUp]
        public virtual void SetUp() { }

        /// <summary>
        ///     Marks the test as inconclusive with a not implemented message
        /// </summary>
        protected static void TestIsNotImplemented()
        {
            const string NotImplementedMessage = "Test Not Implemented";
            Assert.Fail(NotImplementedMessage);
        }
    }
}