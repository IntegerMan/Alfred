// ---------------------------------------------------------
// CommonProviderRegisterTests.cs
// 
// Created on:      09/06/2015 at 10:45 PM
// Last Modified:   09/06/2015 at 10:45 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Text;

using MattEland.Ani.Alfred.Tests.Pages;
using MattEland.Common.Providers;
using MattEland.Common.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Common
{
    /// <summary>
    ///     Tests for common provider registration.
    /// </summary>
    public sealed class CommonProviderRegisterTests : CommonProviderTestsBase
    {

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

        /// <summary>
        ///     Tests that the IoC container allows interfaces to registered as the base type with
        ///     an implementing concrete class as the instantiated type.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
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
        ///     Tests that the IoC container provides new instances of the requested type when a
        ///     type is registered for itself.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
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
            result.Data.ShouldBe(TestClass.DefaultConstructorUsed,
                                 "Default constructor was not used");
        }

        /// <summary>
        ///     Tests that the IoC container provides new instances of the requested type when a
        ///     type is registered for itself.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
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
        ///     Tests that using the IoC container to register an <see langword="abstract"/> class
        ///     throws an exception.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
        /// </remarks>
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
        /// <remarks>
        ///     See ALF-98.
        /// </remarks>
        [Test]
        public void RegisteringAnInstanceAsSingletonProvidesInstance()
        {
            var t = typeof(ITestInterfaceBase);
            var instance = new TestClass(42);

            // Register the instance (not type)
            CommonProvider.RegisterProvidedInstance(t, instance);

            // Get the instance from common provider
            var result = t.ProvideInstanceOf(CommonProvider.Container);

            // Validate
            result.ShouldNotBeNull();
            result.ShouldBeSameAs(instance);
        }

        /// <summary>
        ///     Tests that using the IoC container we can instantiate classes using activator
        ///     functions.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
        /// </remarks>
        [Test]
        public void RegisterTypeUsingActivationFunction()
        {
            Func<PrivateTestClass> activator = PrivateTestClass.CreateInstance;

            CommonProvider.Register(typeof(PrivateTestClass), activator);

            var result = CommonProvider.Provide<PrivateTestClass>();

            result.ShouldNotBeNull("The desired type was not instantiated");
        }

        /// <summary>
        ///     Tests that using the IoC container we can instantiate classes using activator
        ///     functions with parameters.
        /// </summary>
        /// <remarks>
        ///     See ALF-98.
        /// </remarks>
        [Test]
        public void RegisterTypeUsingParameterizedActivationFunction()
        {
            Func<object[], PrivateTestClass> activator =
                PrivateTestClass.CreateInstanceWithParams;

            CommonProvider.Register(typeof(ITestInterfaceBase), activator);

            var result = CommonProvider.TryProvideInstance<ITestInterfaceBase>(1, 3);

            result.ShouldNotBeNull("The desired type was not instantiated");
            result.BaseProperty.ShouldBe(4, "Test object did not use the correct constructor");
        }

        /// <summary>
        ///     Tests that using the IoC container cannot register an <see langword="interface"/> to
        ///     be instantiated, even if it does have inheritance.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotRegisterAnInterface()
        {
            CommonProvider.Register(typeof(ITestInterfaceBase), typeof(ITestInterfaceDerived));
        }

        /// <summary>
        ///     Tests that using the IoC container cannot register a concrete type that does not
        ///     implement an <see langword="interface" /> .
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotRegisterUnimplementedInterface()
        {
            CommonProvider.Register(typeof(ICustomFormatter), typeof(DateTime));
        }

        /// <summary>
        ///     Tests that using the IoC container cannot register a concrete type that is unrelated
        ///     to the type it is being registered to handle.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotRegisterUnrelatedTypes()
        {
            CommonProvider.Register(typeof(StringBuilder), typeof(DateTime));
        }
    }
}