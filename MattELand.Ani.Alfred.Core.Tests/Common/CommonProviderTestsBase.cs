using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Common.Providers;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Common
{
    /// <summary>
    ///     A base class for common provider tests.
    /// </summary>
    public abstract class CommonProviderTestsBase : AlfredTestBase
    {
        /// <summary>
        ///     Sets up the test environment for test runs.
        /// </summary>
        [SetUp]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public override void SetUp()
        {
            base.SetUp();

            // These things live at the static level. Clear so tests don't hit each other
            CommonProvider.ResetMappings();
            CommonProvider.RegisterDefaultProvider(null);
        }

        /// <summary>
        ///     A testing class used by <see cref="CommonProviderTests" />
        /// </summary>
        public sealed class TestClass : TestClassBase, ITestInterfaceDerived
        {
            /// <summary> The default constructor used. </summary>
            internal const string DefaultConstructorUsed = "Default Constructor was Used";

            /// <summary> Initializes a new instance of the <see cref="T:System.Object" /> class. </summary>
            /// <remarks>
            ///     This is public so that it can be instantiated by
            ///     <see cref="CommonProvider.Provide{TRequested}" />
            /// </remarks>
            [UsedImplicitly]
            public TestClass() : this(DefaultConstructorUsed)
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
            /// </summary>
            /// <param name="data"> The data. </param>
            public TestClass([CanBeNull] object data) : base(data?.GetHashCode() ?? 42)
            {
                Data = data;
            }

            /// <summary>
            ///     Gets the data.
            /// </summary>
            /// <value>
            ///     The data.
            /// </value>
            /// <seealso cref="P:ITestInterfaceDerived.Data"/>
            [CanBeNull]
            public object Data { get; }
        }

        /// <summary>
        ///     A private class for testing <see cref="CommonProvider" />
        /// </summary>
        /// <seealso cref="T:TestClassBase"/>
        protected sealed class PrivateTestClass : TestClassBase
        {
            /// <summary>
            ///     Initializes a new instance of the
            ///     <see cref="PrivateTestClass" />
            ///     class.
            /// </summary>
            private PrivateTestClass() : this(42)
            {
            }

            /// <summary>
            ///     Initializes a new instance of the
            ///     <see cref="PrivateTestClass" />
            ///     class.
            /// </summary>
            /// <param name="value"> The value to use to set the base property. </param>
            private PrivateTestClass(int value) : base(value)
            {
            }

            /// <summary>
            ///     Builds an instance of
            ///     <see cref="PrivateTestClass" />.
            /// </summary>
            /// <returns>
            ///     A new instance of
            ///     <see cref="PrivateTestClass" />
            /// </returns>
            [NotNull]
            internal static PrivateTestClass CreateInstance()
            {
                return new PrivateTestClass();
            }

            /// <summary>
            ///     Builds an instance of
            ///     <see cref="PrivateTestClass" />.
            /// </summary>
            /// <param name="args"> The arguments. </param>
            /// <returns>
            ///     A new instance of
            ///     <see cref="PrivateTestClass" />
            /// </returns>
            [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
            internal static PrivateTestClass CreateInstanceWithParams(params object[] args)
            {
                var sum = 0;

                // Add items to the sum
                sum += args.Cast<int>().Sum();

                // Pass in the value to the object so we can validate it
                return new PrivateTestClass(sum);
            }
        }

        /// <summary>
        ///     An <see langword="abstract"/> class for testing <see cref="CommonProvider" />
        /// </summary>
        /// <seealso cref="T:ITestInterfaceBase"/>
        public abstract class TestClassBase : ITestInterfaceBase
        {
            /// <summary>
            ///     Initializes a new instance of the
            ///     <see cref="TestClassBase" /> class.
            /// </summary>
            /// <param name="value"> The base property value. </param>
            protected TestClassBase(int value)
            {
                BaseProperty = value;
            }

            /// <summary>
            ///     Gets or sets the base property.
            /// </summary>
            /// <value>
            ///     The base property.
            /// </value>
            public int BaseProperty { get; set; }
        }

        /// <summary>
        ///     Interface for test interface derived.
        /// </summary>
        protected interface ITestInterfaceDerived : ITestInterfaceBase
        {
            /// <summary>
            ///     Gets the data.
            /// </summary>
            /// <value>
            ///     The data.
            /// </value>
            object Data { get; }
        }

        /// <summary>
        ///     A test <see langword="interface"/> that <see cref="ITestInterfaceDerived"/> is derived
        ///     from.
        /// </summary>
        protected interface ITestInterfaceBase
        {
            /// <summary>
            ///     Gets or sets the base property.
            /// </summary>
            /// <value>
            ///     The base property.
            /// </value>
            int BaseProperty { get; set; }
        }
    }
}