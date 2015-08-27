// ---------------------------------------------------------
// CommonProviderTests.cs
// 
// Created on:      08/27/2015 at 2:49 PM
// Last Modified:   08/27/2015 at 3:54 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Common.Providers;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Common
{
    /// <summary>
    ///     Tests the Dependency Injection / Inversion of Control container capabilities provided by
    ///     MattEland.Common
    /// </summary>
    [TestFixture]
    public class CommonProviderTests
    {
        public class CommonProviderTestObjectBase
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
            /// </summary>
            /// <param name="value">The base property value.</param>
            public CommonProviderTestObjectBase(int value)
            {
                BaseProperty = value;
            }

            /// <summary>
            ///     Gets or sets the base property.
            /// </summary>
            /// <value>The base property.</value>
            public int BaseProperty { get; set; }
        }

        /// <summary>
        ///     A testing class used by <see cref="CommonProviderTests" />
        /// </summary>
        public class CommonProviderTestObject : CommonProviderTestObjectBase
        {
            internal const string DefaultConstructorUsed = "Default Constructor was Used";

            /// <summary>
            ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
            /// </summary>
            public CommonProviderTestObject() : this(DefaultConstructorUsed)
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
            /// </summary>
            public CommonProviderTestObject([CanBeNull] object data)
                : base(data?.GetHashCode() ?? 42)
            {
                Data = data;
            }

            /// <summary>
            ///     Gets or sets the data.
            /// </summary>
            /// <value>The data.</value>
            [CanBeNull]
            public object Data { get; set; }
        }

        /// <summary>
        ///     Tests that the IoC container provides new instances of the requested type when a type is
        ///     registered for itself.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void RegisterAndCreateInstantiatesUsingDefaultConstructor()
        {
            // Tell the container to create types of our object when that type is requested
            CommonProvider.Register(typeof(CommonProviderTestObject));

            // Use the container to create the instance we want
            var result = CommonProvider.Create<CommonProviderTestObject>();

            // Check to see that the container created the object using the default constructor
            Assert.IsNotNull(result, "Item was not instantiated.");
            Assert.AreEqual(CommonProviderTestObject.DefaultConstructorUsed,
                            result.Data,
                            "Default constructor was not used");
        }

        /// <summary>
        ///     Tests that the IoC container provides new instances of the requested type when a type is
        ///     registered for itself.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void RegisterBaseTypeAndCreateInstantiatesUsingDefaultConstructor()
        {
            // Tell the container to create types of our object when that type is requested
            CommonProvider.Register(typeof(CommonProviderTestObjectBase),
                                    typeof(CommonProviderTestObject));

            // Use the container to create the instance we want
            var result = CommonProvider.Create<CommonProviderTestObjectBase>();

            // Check to see that the container created the object using the default constructor
            Assert.IsNotNull(result, "Item was not instantiated.");

            var typedResult = result as CommonProviderTestObject;
            Assert.IsNotNull(typedResult,
                             $"Item was not the correct type. Actual type: {result.GetType().FullName}");

            Assert.AreEqual(CommonProviderTestObject.DefaultConstructorUsed,
                            typedResult.Data,
                            "Default constructor was not used");
        }

        /// <summary>
        ///     Tests that using the IoC container to activate a base class throws an exception.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterBaseTypeUsingDefaultConstructorThrowsException()
        {
            CommonProvider.Create<CommonProviderTestObjectBase>();
        }
    }
}