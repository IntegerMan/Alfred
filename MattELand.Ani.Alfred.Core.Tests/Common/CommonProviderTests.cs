// ---------------------------------------------------------
// CommonProviderTests.cs
// 
// Created on:      08/27/2015 at 2:49 PM
// Last Modified:   08/29/2015 at 1:02 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Tests.Pages;
using MattEland.Common.Providers;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Common
{
    /// <summary>
    ///     Tests the Dependency Injection / Inversion of Control container capabilities provided by
    ///     MattEland.Common.
    /// </summary>
    /// <seealso cref="T:MattEland.Ani.Alfred.Tests.AlfredTestBase"/>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "EventExceptionNotDocumented")]
    public class CommonProviderTests : AlfredTestBase
    {
        /// <summary> Sets up the test environment for test runs. </summary>
        /// <seealso cref="M:MattEland.Ani.Alfred.Tests.AlfredTestBase.SetUp()"/>
        [SetUp]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public override void SetUp()
        {
            base.SetUp();

            // These things live at the static level. Clear so tests don't hit each other
            CommonProvider.ResetMappings();
            CommonProvider.RegisterDefaultProvider(null);
        }

        #region Test Classes

        /// <summary> A testing class used by <see cref="CommonProviderTests" /> </summary>
        /// <seealso cref="T:TestClassBase"/>
        /// <seealso cref="T:ITestInterfaceDerived"/>
        public class TestClass : TestClassBase, ITestInterfaceDerived
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

            /// <summary> Initializes a new instance of the <see cref="T:System.Object" /> class. </summary>
            /// <param name="data"> The data. </param>
            public TestClass([CanBeNull] object data) : base(data?.GetHashCode() ?? 42)
            {
                Data = data;
            }

            /// <summary> Gets the data. </summary>
            /// <value> The data. </value>
            /// <seealso cref="P:ITestInterfaceDerived.Data"/>
            [CanBeNull]
            public object Data { get; }
        }

        /// <summary> A private class for testing <see cref="CommonProvider" /> </summary>
        /// <seealso cref="T:TestClassBase"/>
        private class PrivateTestClass : TestClassBase
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

        /// <summary> An abstract class for testing <see cref="CommonProvider" /> </summary>
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

            /// <summary> Gets or sets the base property. </summary>
            /// <seealso cref="P:MattEland.Ani.Alfred.Tests.Common.CommonProviderTests.ITestInterfaceBase.BaseProperty"/>
            public int BaseProperty { get; set; }
        }

        /// <summary> Interface for test interface derived. </summary>
        /// <seealso cref="T:MattEland.Ani.Alfred.Tests.AlfredTestBase"/>
        public interface ITestInterfaceDerived : ITestInterfaceBase
        {
            /// <summary> Gets the data. </summary>
            /// <value> The data. </value>
            object Data { get; }
        }

        /// <summary>
        ///     A test <see langword="interface"/> that <see cref="ITestInterfaceDerived"/> is derived
        ///     from.
        /// </summary>
        /// <seealso cref="T:MattEland.Ani.Alfred.Tests.AlfredTestBase"/>
        public interface ITestInterfaceBase
        {
            /// <summary> Gets or sets the base property. </summary>
            /// <value> The base property. </value>
            int BaseProperty { get; set; }
        }

        #endregion

        /// <summary> Determines whether this instance [can define custom default provider]. </summary>
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
        ///     Tests that using the IoC container cannot register an interface to be instantiated, even
        ///     if it does have inheritance.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotRegisterAnInterface()
        {
            CommonProvider.Register(typeof(ITestInterfaceBase), typeof(ITestInterfaceDerived));
        }

        /// <summary>
        ///     Tests that using the IoC container cannot register a concrete type that does not
        ///     implement an
        ///     <see langword="interface" />.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotRegisterUnimplementedInterface()
        {
            CommonProvider.Register(typeof(ICustomFormatter), typeof(DateTime));
        }

        /// <summary>
        ///     Tests that using the IoC container cannot register a concrete type that is unrelated to
        ///     the type it is being registered to handle.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void CannotRegisterUnrelatedTypes()
        {
            CommonProvider.Register(typeof(StringBuilder), typeof(DateTime));
        }

        /// <summary>
        ///     Tests containers request values from their parent before a fallback is used when a parent
        ///     is present and no mapping was found.
        /// </summary>
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
        /// <remarks> See ALF-98. </remarks>
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
        [Test]
        public void CreateTypeMultipleTimesCreatesMultipleObjects()
        {
            var a = CommonProvider.Provide<TestClass>();
            var b = CommonProvider.Provide<TestClass>();

            a.ShouldNotBeSameAs(b, "CommonProvider.Create should create multiple instances");
        }

        /// <summary>
        ///     Tests that using the IoC container we can instantiate classes using parameterized
        ///     constructors.
        /// </summary>
        [Test]
        public void CreateTypeWithParameterizedConstructor()
        {
            const int Data = 1980;
            CommonProvider.Register(typeof(TestClassBase), typeof(TestClass));

            var result = CommonProvider.Provide<TestClassBase>(Data);

            result.ShouldNotBeNull("The desired type was not instantiated");
            var test = result as TestClass;
            test.ShouldNotBeNull(
                                 $"The result was not TestClass. Instead was {result.GetType().FullName}");

            test.Data.ShouldBe(Data, "Test object was not created using the correct constructor");
        }

        /// <summary>
        ///     Ensures the dependency container uses itself to get a default provider when lazy loading.
        /// </summary>
        /// <remarks> See ALF-98. </remarks>
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

        /// <summary> Tests that multiple containers can exist side by side. </summary>
        /// <remarks> See ALF-98. </remarks>
        [Test]
        public void MultipleContainersCanExist()
        {
            // Get the containers
            var containerA = CommonProvider.Container;
            var containerB = new CommonContainer();

            // Register the same type for both containers with different implementations
            var t = typeof(TestClassBase);
            containerA.Register(t, typeof(TestClass));
            containerB.Register(t,
                                (Func<object[], object>)PrivateTestClass.CreateInstanceWithParams);

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
        /// <remarks> See ALF-98. </remarks>
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
        /// <remarks> See ALF-98. </remarks>
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
            result.Data.ShouldBe(TestClass.DefaultConstructorUsed,
                                 "Default constructor was not used");
        }

        /// <summary>
        ///     Tests that the IoC container provides new instances of the requested type when a type is
        ///     registered for itself.
        /// </summary>
        /// <remarks> See ALF-98. </remarks>
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
        /// <remarks> See ALF-98. </remarks>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterBaseTypeForBaseTypeThrowsInvalidOperationException()
        {
            CommonProvider.Register(typeof(TestClassBase), typeof(TestClassBase));
        }

        /// <summary>
        ///     Tests that registering an object as the provided instance causes that instance to be
        ///     returned when the given type is requested.
        /// </summary>
        /// <remarks> See ALF-98. </remarks>
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
        /// <remarks> See ALF-98. </remarks>
        [Test]
        public void RegisterTypeUsingActivationFunction()
        {
            Func<PrivateTestClass> activator = PrivateTestClass.CreateInstance;

            CommonProvider.Register(typeof(PrivateTestClass), activator);

            var result = CommonProvider.Provide<PrivateTestClass>();

            result.ShouldNotBeNull("The desired type was not instantiated");
        }

        /// <summary>
        ///     Tests that using the IoC container we can instantiate classes using activator functions
        ///     with parameters.
        /// </summary>
        /// <remarks> See ALF-98. </remarks>
        [Test]
        public void RegisterTypeUsingParameterizedActivationFunction()
        {
            var activator =
                new Func<object[], PrivateTestClass>(PrivateTestClass.CreateInstanceWithParams);

            CommonProvider.Register(typeof(ITestInterfaceBase), activator);

            var result = CommonProvider.TryProvideInstance<ITestInterfaceBase>(1, 3);

            result.ShouldNotBeNull("The desired type was not instantiated");
            result.BaseProperty.ShouldBe(4, "Test object did not use the correct constructor");
        }

        /// <summary>
        ///     Test that requesting an unknown type causes the instance provider to throw a
        ///     <see cref="NotSupportedException" />.
        /// </summary>
        /// <remarks> See ALF-98. </remarks>
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
        ///     Tests that registering an <see langword="object"/> as the provided instance causes that
        ///     instance to be returned when the given type is requested.
        /// </summary>
        /// <remarks> See ALF-98. </remarks>
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
        ///     instances as expected.
        /// </summary>
        /// <remarks> See ALF-98. </remarks>
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
        ///     instances as expected.
        /// </summary>
        /// <remarks> See ALF-98. </remarks>
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

        /// <summary>
        ///     Tests that the container can provide empty collections of objects.
        /// </summary>
        /// <remarks> See ALF-98. </remarks>
        [Test]
        public void ProviderCanCreateCollections()
        {
            var collection = CommonProvider.ProvideCollection<TestClassBase>();

            collection.ShouldNotBeNull();
            collection.ShouldBeEmpty();
        }

        /// <summary>
        ///     Tests that the container can provide empty collections of objects that use a preferred collection.
        /// </summary>
        /// <remarks> See ALF-98. </remarks>
        [Test]
        public void ProviderCanCreateCollectionsOfCustomType()
        {
            CommonProvider.CollectionType = typeof(HashSet<>);

            var collection = CommonProvider.ProvideCollection<ITestInterfaceDerived>();

            collection.ShouldNotBeNull();
            collection.ShouldBeEmpty();
            collection.ShouldBeOfType(typeof(HashSet<ITestInterfaceDerived>));
        }

        /// <summary>
        ///     Container.TryRegister should succeed if a mapping does not exist.
        /// </summary>
        [Test]
        public void TryRegisterIfNotMappedShouldRegister()
        {
            var container = new CommonContainer();

            var type = typeof(UnitTestBase);
            var registerType = typeof(CommonProviderTests);

            container.HasMapping(type).ShouldBe(false);
            var result = container.TryRegister(type, registerType);
            result.ShouldBe(true);
            container.HasMapping(type).ShouldBe(true);

            var instance = container.ProvideType(type);
            instance.ShouldBeOfType(registerType);
        }

        /// <summary>
        ///     Container.TryRegister should fail if a mapping already exists.
        /// </summary>
        [Test]
        public void TryRegisterIfMappedShouldNotRegister()
        {
            var type = typeof(AlfredTestBase);
            var registerType = typeof(CommonProviderTests);

            Container.Register(type, typeof(EventLogPageTests));
            Container.HasMapping(type).ShouldBe(true);
            var result = Container.TryRegister(type, registerType);

            result.ShouldBe(false);

            var instance = Container.ProvideType(type);
            instance.ShouldNotBeOfType(registerType);
        }
    }

}