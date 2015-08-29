// ---------------------------------------------------------
// CommonProviderInstantiationTests.cs
// 
// Created on:      08/27/2015 at 2:49 PM
// Last Modified:   08/28/2015 at 1:53 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using MattEland.Common.Providers;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Common
{
    /// <summary>
    ///     Tests the Dependency Injection / Inversion of Control container capabilities provided by
    ///     MattEland.Common
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "EventExceptionNotDocumented")]
    public class CommonProviderInstantiationTests : AlfredTestBase
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

        public interface ITestInterfaceBase
        {
            int BaseProperty { get; set; }
        }

        public interface ITestInterfaceDerived : ITestInterfaceBase
        {
            object Data { get; }
        }

        /// <summary>
        ///     An abstract class for testing <see cref="CommonProvider" />
        /// </summary>
        public abstract class TestClassBase : ITestInterfaceBase
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="TestClassBase" /> class.
            /// </summary>
            /// <param name="value">The base property value.</param>
            protected TestClassBase(int value)
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
        ///     A private class for testing <see cref="CommonProvider" />
        /// </summary>
        private class PrivateTestClass : TestClassBase
        {

            /// <summary>
            ///     Initializes a new instance of the <see cref="PrivateTestClass" /> class.
            /// </summary>
            private PrivateTestClass() : this(42)
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="PrivateTestClass" /> class.
            /// </summary>
            /// <param name="value">The value to use to set the base property.</param>
            private PrivateTestClass(int value) : base(value)
            {
            }

            /// <summary>
            ///     Builds an instance of <see cref="PrivateTestClass" />.
            /// </summary>
            /// <returns>A new instance of <see cref="PrivateTestClass" /></returns>
            [NotNull]
            internal static PrivateTestClass CreateInstance()
            {
                return new PrivateTestClass();
            }

            /// <summary>
            ///     Builds an instance of <see cref="PrivateTestClass" />.
            /// </summary>
            /// <param name="args">The arguments.</param>
            /// <returns>A new instance of <see cref="PrivateTestClass" /></returns>
            [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
            internal static PrivateTestClass CreateInstanceWithParams(params object[] args)
            {
                var sum = 0;

                // Add items to the sum
                if (args != null) { sum += args.Cast<int>().Sum(); }

                // Pass in the value to the object so we can validate it
                return new PrivateTestClass(sum);
            }
        }

        /// <summary>
        ///     A testing class used by <see cref="CommonProviderInstantiationTests" />
        /// </summary>
        public class TestClass : TestClassBase, ITestInterfaceDerived
        {
            internal const string DefaultConstructorUsed = "Default Constructor was Used";

            /// <summary>
            ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
            /// </summary>
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
            public TestClass([CanBeNull] object data) : base(data?.GetHashCode() ?? 42)
            {
                Data = data;
            }

            /// <summary>
            ///     Gets or sets the data.
            /// </summary>
            /// <value>The data.</value>
            [CanBeNull]
            public object Data { get; }
        }

        /// <summary>
        ///     Determines whether this instance [can define custom default provider].
        /// </summary>
        [Test]
        public void CanDefineCustomDefaultProvider()
        {
            const string Bubba = "Bubba";

            // Change the fallback / default provider to be something that yields our magic string
            var provider = new InstanceProvider(null);
            provider.Register(typeof(string), Bubba);
            CommonProvider.RegisterDefaultProvider(provider);

            // Grab the instance and check to see if its our string
            var instance = CommonProvider.Provide<string>();
            instance.ShouldBeSameAs(Bubba);
        }

        /// <summary>
        ///     Tests that using the IoC container cannot register an interface to be instantiated, even if it
        ///     does have inheritance.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotRegisterAnInterface()
        {
            CommonProvider.Register(typeof(ITestInterfaceBase), typeof(ITestInterfaceDerived));
        }

        /// <summary>
        ///     Tests that using the IoC container cannot register a concrete type that does not implement an
        ///     interface.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotRegisterUnimplementedInterface()
        {
            CommonProvider.Register(typeof(ICustomFormatter), typeof(DateTime));
        }

        /// <summary>
        ///     Tests that using the IoC container cannot register a concrete type that is unrelated to the
        ///     type it is being registered to handle
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotRegisterUnrelatedTypes()
        {
            CommonProvider.Register(typeof(StringBuilder), typeof(DateTime));
        }

        /// <summary>
        ///     Tests containers request values from their parent before a fallback is used when a parent is
        ///     present and no mapping was found.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void ContainersCanGetMappingsFromTheirParent()
        {
            // Get the containers
            var parent = CommonProvider.Container;
            var child = new CommonContainer(parent);

            // Register the same type for both containers with different implementations
            var t = typeof(TestClassBase);
            parent.Register(t, typeof(TestClass));

            // Build our instances
            var instance = child.Provide<TestClassBase>();

            // Check that the item was provided
            instance.ShouldNotBeNull();
        }

        /// <summary>
        ///     Tests that using the IoC container to activate an abstract class throws an exception.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void CreateBaseTypeUsingDefaultConstructorThrowsException()
        {
            CommonProvider.Provide<TestClassBase>();
        }

        /// <summary>
        ///     Tests that the IoC container provides new instances of the requested type when a type is
        ///     registered for itself.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void CreateTypeMultipleTimesCreatesMultipleObjects()
        {
            var a = CommonProvider.Provide<TestClass>();
            var b = CommonProvider.Provide<TestClass>();

            a.ShouldNotBeSameAs(b, "CommonProvider.Create should create multiple instances");
        }

        /// <summary>
        ///     Tests that using the IoC container we can instantiate classes using parameterized constructors.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void CreateTypeWithParameterizedConstructor()
        {
            const int Data = 1980;
            CommonProvider.Register(typeof(TestClassBase), typeof(TestClass));

            var result = CommonProvider.Provide<TestClassBase>(Data);

            result.ShouldNotBeNull("The desired type was not instantiated");
            var test = result as TestClass;
            test.ShouldNotBeNull($"The result was not TestClass. Instead was {result.GetType().FullName}");

            test.Data.ShouldBe(Data,
                            "Test object was not created using the correct constructor");
        }

        /// <summary>
        ///     Ensures the dependency container uses itself to get a default provider when lazy loading.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void EnsureDependencyContainerUsesItselfToGetDefaultProvider()
        {
            // Create our default provider with instructions to yield itself as an IObjectProvider
            var instanceProvider = new InstanceProvider(null);
            instanceProvider.Register(typeof(IObjectProvider), instanceProvider);

            // Register the new provider as a source for IObjectProvider
            CommonProvider.Register(typeof(IObjectProvider), instanceProvider);

            // Cause DefaultProvider to be lazy loaded and store the result
            var defaultProvider = CommonProvider.Container.FallbackProvider;

            // Check that they're the same object
            instanceProvider.ShouldBeSameAs(defaultProvider);
        }

        /// <summary>
        ///     Tests that multiple containers can exist side by side
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void MultipleContainersCanExist()
        {
            // Get the containers
            var containerA = CommonProvider.Container;
            var containerB = new CommonContainer();

            // Register the same type for both containers with different implementations
            var t = typeof(TestClassBase);
            containerA.Register(t, typeof(TestClass));
            containerB.Register(t, (Func<object[], object>)PrivateTestClass.CreateInstanceWithParams);

            // Build our instances
            var instanceA = containerA.Provide<TestClassBase>(35);
            var instanceB = containerB.Provide<TestClassBase>(1, 1);

            // Assert we have different strokes for different folks
            containerA.ShouldNotBeSameAs(containerB);
            instanceA.ShouldNotBeSameAs(instanceB);

            instanceA.ShouldNotBeNull("A was null");
            instanceB.ShouldNotBeNull("B was null");

            instanceA.ShouldBeOfType<TestClass>("A was not expected type");
            instanceB.ShouldBeOfType<PrivateTestClass>("B was not expected type");
        }

        /// <summary>
        ///     Tests that the IoC container allows interfaces to registered as the base type with an
        ///     implementing concrete class as the instantiated type.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void RegisterAndCreateForInterfaceBaseTypeWorks()
        {
            // Tell the container to create types of our object when that type is requested
            CommonProvider.Register(typeof(ITestInterfaceDerived), typeof(TestClass));

            // Use the container to create the instance we want
            var result = CommonProvider.Provide<ITestInterfaceDerived>();

            // Check to see that the container created the object using the default constructor
            result.ShouldNotBeNull();
            result.Data.ShouldBe(result.Data, "Default constructor was not used");
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
            var type = typeof(TestClass);
            type.RegisterProvider(type);

            // Use the container to create the instance we want
            var result = CommonProvider.Provide<TestClass>();

            // Check to see that the container created the object using the default constructor
            result.ShouldNotBeNull();
            result.Data.ShouldBe(TestClass.DefaultConstructorUsed, "Default constructor was not used");
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
            var baseType = typeof(TestClassBase);
            var preferredType = typeof(TestClass);
            baseType.RegisterProvider(preferredType);

            // Use the container to create the instance we want
            var result = CommonProvider.Provide<TestClassBase>();

            // Check to see that the container created the object using the default constructor
            Assert.IsNotNull(result, "Item was not instantiated.");

            var typedResult = result as TestClass;
            Assert.IsNotNull(typedResult,
                             $"Item was not the correct type. Actual type: {result.GetType().FullName}");

            Assert.AreEqual(TestClass.DefaultConstructorUsed,
                            typedResult.Data,
                            "Default constructor was not used");
        }

        /// <summary>
        ///     Tests that using the IoC container to register an abstract class throws an exception.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterBaseTypeForBaseTypeThrowsInvalidOperationException()
        {
            CommonProvider.Register(typeof(TestClassBase), typeof(TestClassBase));
        }

        /// <summary>
        ///     Tests that registering an object as the provided instance causes that instance to be returned
        ///     when the given type is requested.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void RegisteringAnInstanceAsSingletonProvidesInstance()
        {
            var t = typeof(ITestInterfaceBase);
            var instance = new TestClass(42);

            // Register the instance (not type)
            CommonProvider.RegisterProvidedInstance(t, instance);

            // Get the instance from common provider
            var result = t.ProvideInstanceOf();

            // Validate
            result.ShouldNotBeNull();
            result.ShouldBeSameAs(instance);
        }

        /// <summary>
        ///     Tests that using the IoC container we can instantiate classes using activator functions.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void RegisterTypeUsingActivationFunction()
        {
            Func<PrivateTestClass> activator = PrivateTestClass.CreateInstance;

            CommonProvider.Register(typeof(PrivateTestClass), activator);

            var result = CommonProvider.Provide<PrivateTestClass>();

            result.ShouldNotBe(null, "The desired type was not instantiated");
        }

        /// <summary>
        ///     Tests that using the IoC container we can instantiate classes using activator functions with
        ///     parameters.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void RegisterTypeUsingParameterizedActivationFunction()
        {
            var activator =
                new Func<object[], PrivateTestClass>(PrivateTestClass.CreateInstanceWithParams);

            CommonProvider.Register(typeof(PrivateTestClass), activator);

            var result = CommonProvider.TryProvideInstance<PrivateTestClass>(1, 3);

            result.ShouldNotBe(null, "The desired type was not instantiated");
            result.BaseProperty.ShouldBe(4, "Test object was not created using the correct constructor");
        }

        /// <summary>
        ///     Test that requesting an unknown type causes the instance provider to throw a
        ///     <see cref="NotSupportedException" />.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void RequestingAnUnknownTypeCausesInstanceProviderToError()
        {
            // Don't do this. I'm just testing extension methods
            new InstanceProvider().RegisterAsDefaultProvider();

            // Don't code like this either.
            typeof(string).ProvideInstanceOf();
        }

        /// <summary>
        ///     Tests that registering an object as the provided instance causes that instance to be returned
        ///     when the given type is requested.
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void RequestingMultipleInstancesAfterRegisteringProvidedInstanceReturnsSameInstance()
        {
            var t = typeof(ITestInterfaceBase);
            var instance = new TestClass(42);

            // Register as singleton
            instance.RegisterAsProvidedInstance(t);

            // Grab a few instances
            var a = t.ProvideInstanceOf();
            var b = t.ProvideInstanceOf();

            // Check that the items are the same instance
            instance.ShouldBeSameAs(a);
            instance.ShouldBeSameAs(b);
            a.ShouldBeSameAs(b);
        }

        /// <summary>
        ///     Test that using the non-generic <see cref="CommonProvider.ProvideType" /> provides
        ///     instances as expected
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void RequestingTypeSafeReturnsNullWhenNoMapping()
        {
            // Use non-generic retrieval method
            var instance = CommonProvider.TryProvideInstance<IDisposable>();

            // Make sure we didn't get a result
            instance.ShouldBe(null);
        }

        /// <summary>
        ///     Test that using the non-generic <see cref="CommonProvider.ProvideType" /> provides
        ///     instances as expected
        /// </summary>
        /// <remarks>
        ///     See ALF-98
        /// </remarks>
        [Test]
        public void RequestingTypeUsingProvideInstanceOfTypeWorks()
        {
            // Register using extension methods
            var t = typeof(ITestInterfaceBase);
            t.RegisterProvider(typeof(TestClass));

            // Use non-generic retrieval method
            var instance = CommonProvider.ProvideType(t);

            // Make sure we got what we thought we did
            instance.ShouldNotBe(null);
            instance.ShouldBeAssignableTo(t);
        }
    }

}