// ---------------------------------------------------------
// CommonProviderTests.cs
// 
// Created on:      09/03/2015 at 11:00 PM
// Last Modified:   09/06/2015 at 10:45 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
    /// <seealso cref="T:MattEland.Ani.Alfred.Tests.AlfredTestBase" />
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "EventExceptionNotDocumented")]
    public sealed class CommonProviderTests : CommonProviderTestsBase
    {

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
        ///     Tests containers request values from their parent before a fallback is used when a
        ///     parent is present and no mapping was found.
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
        ///     Tests that using the IoC container to activate an <see langword="abstract"/> class
        ///     throws an exception.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
        /// </remarks>
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void CreateBaseTypeUsingDefaultConstructorThrowsException()
        {
            CommonProvider.Provide<TestClassBase>();
        }

        /// <summary>
        ///     Tests that the IoC container provides new instances of the requested type when a
        ///     type is registered for itself.
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
        ///     Ensures the dependency container uses itself to get a default provider when lazy
        ///     loading.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
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
        ///     Tests that multiple containers can exist side by side.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
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
        ///     Test that requesting an unknown type causes the instance provider to
        ///     <see langword="throw"/> a <see cref="NotSupportedException" /> .
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
        /// </remarks>
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void RequestingAnUnknownTypeCausesInstanceProviderToError()
        {
            // Don't do this. I'm just testing extension methods
            new InstanceProvider().RegisterAsDefaultProvider();

            // Don't code like this either.
            typeof(string).ProvideInstanceOf(Container);
        }

        /// <summary>
        ///     Tests that registering an <see langword="object" /> as the provided instance causes
        ///     that instance to be returned when the given type is requested.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
        /// </remarks>
        [Test]
        public void RequestingMultipleInstancesAfterRegisteringProvidedInstanceReturnsSameInstance()
        {
            var t = typeof(ITestInterfaceBase);
            var instance = new TestClass(42);

            // Register as singleton
            instance.RegisterAsProvidedInstance(t, Container);

            // Grab a few instances
            var a = t.ProvideInstanceOf(Container);
            var b = t.ProvideInstanceOf(Container);

            // Check that the items are the same instance
            instance.ShouldBeSameAs(a);
            instance.ShouldBeSameAs(b);
            a.ShouldBeSameAs(b);
        }

        /// <summary>
        ///     Test that using the non-generic <see cref="CommonProvider.ProvideType" /> provides
        ///     instances as expected.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
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
        ///     instances as expected.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
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

        /// <summary>
        ///     Tests that the container can provide empty collections of objects.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
        /// </remarks>
        [Test]
        public void ProviderCanCreateCollections()
        {
            var collection = CommonProvider.ProvideCollection<TestClassBase>();

            collection.ShouldNotBeNull();
            collection.ShouldBeEmpty();
        }

        /// <summary>
        ///     Tests that the container can provide empty collections of objects that use a
        ///     preferred collection.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
        /// </remarks>
        [Test]
        public void ProviderCanCreateCollectionsOfCustomType()
        {
            CommonProvider.CollectionType = typeof(HashSet<>);

            var collection = CommonProvider.ProvideCollection<ITestInterfaceDerived>();

            collection.ShouldNotBeNull();
            collection.ShouldBeEmpty();
            collection.ShouldBeOfType(typeof(HashSet<ITestInterfaceDerived>));
        }
    }

}