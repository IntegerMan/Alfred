// ---------------------------------------------------------
// CommonProvider.cs
// 
// Created on:      08/27/2015 at 2:55 PM
// Last Modified:   08/28/2015 at 1:11 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace MattEland.Common.Providers
{

    /// <summary>
    ///     A dependency injection container used to quickly instantiate classes without coupling ourselves
    ///     to that implementation.
    /// </summary>
    [PublicAPI]
    public static class CommonProvider
    {

        /// <summary>
        ///     Gets the default dependency injection container.
        /// </summary>
        /// <value>The container.</value>
        [NotNull]
        public static CommonContainer Container { get; } = new CommonContainer();

        /// <summary>
        ///     Registers the preferred type as the type to instantiate when the base type is requested.
        /// </summary>
        /// <param name="baseType">The type that will be requested.</param>
        /// <param name="preferredType">
        ///     The type that should be created when <see cref="baseType" /> is
        ///     requested.
        /// </param>
        /// <param name="arguments">The arguments (if any) to pass to the class's constructor.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="baseType" /> or
        ///     <paramref name="preferredType" /> is <see langword="null" />.
        /// </exception>
        public static void Register(
            [NotNull] Type baseType,
            [NotNull] Type preferredType,
            [CanBeNull] params object[] arguments)
        {
            //- Validate
            if (baseType == null) { throw new ArgumentNullException(nameof(baseType)); }
            if (preferredType == null) { throw new ArgumentNullException(nameof(preferredType)); }

            ValidateTypeRegistration(baseType, preferredType);

            // Register the type mapping using the default Activator-based model.
            var provider = new ActivatorObjectProvider(preferredType, arguments);
            Register(baseType, provider);
        }

        /// <summary>
        ///     Validates that <see cref="preferredType" /> is something that can be created.
        /// </summary>
        /// <param name="baseType">The type that will be requested.</param>
        /// <param name="preferredType">
        ///     The type that should be created when <see cref="baseType" /> is
        ///     requested.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Various scenarios where
        ///     <paramref name="preferredType" /> cannot be instantiated or cannot be cast to
        ///     <paramref name="baseType" />.
        /// </exception>
        private static void ValidateTypeRegistration(
            [NotNull] Type baseType,
            [NotNull] Type preferredType)
        {
            // Ya kinda can't instantiate an abstract type
            if (preferredType.IsAbstract)
            {
                throw new InvalidOperationException("Cannot create an abstract type");
            }

            // Check interface implementation, inheritance, and whether they're the same type
            if (!baseType.IsAssignableFrom(preferredType))
            {
                throw new InvalidOperationException(
                    $"{preferredType.FullName} cannot be cast to {baseType.FullName}");
            }
        }

        /// <summary>
        ///     Provides an instance of the requested type.
        /// </summary>
        /// <typeparam name="TRequested">The type that was requested to be provided.</typeparam>
        /// <returns>An instance of the requested type</returns>
        /// <exception cref="NotSupportedException">
        ///     The type is not correctly configured to allow for
        ///     instantiation.
        /// </exception>
        [NotNull]
        public static TRequested ProvideInstance<TRequested>()
        {
            var type = typeof(TRequested);

            var instance = ProvideInstanceOfType(type);
            Debug.Assert(instance != null);

            return (TRequested)instance;
        }

        /// <summary>
        ///     Provides an instance of the requested type.
        /// </summary>
        /// <paramref name="type">The type that was requested to be provided.</paramref>
        /// <returns>An instance of the requested type</returns>
        /// <exception cref="NotSupportedException">
        ///     The type is not correctly configured to allow for
        ///     instantiation.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="type" /> is <see langword="null" />.</exception>
        [CanBeNull]
        public static object ProvideInstanceOfType(
            [NotNull] Type type,
            bool errorOnNoInstance = true)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            return Container.ProvideInstanceOfType(type, errorOnNoInstance);
        }

        /// <summary>
        ///     Throws the not provided <see cref="NotSupportedException" />.
        /// </summary>
        /// <param name="typeName">The name of the type that was requested</param>
        /// <exception cref="NotSupportedException">
        ///     Thrown if the operation was not supported given the current
        ///     configuration.
        /// </exception>
        private static void ThrowNotProvidedException(string typeName)
        {
            var message = $"The activator function for creating {typeName} returned a null value.";
            throw new NotSupportedException(message);
        }

        /// <summary>
        ///     Registers an activator function responsible for instantiating the desired type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="activator">The activator function.</param>
        /// <param name="arguments">The arguments to pass in to the activator function.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="activator" /> is <see langword="null" />.
        /// </exception>
        public static void Register(
            [NotNull] Type type,
            [NotNull] Delegate activator,
            [CanBeNull] params object[] arguments)
        {
            //- Validate
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (activator == null) { throw new ArgumentNullException(nameof(activator)); }

            // Build a function-based provider
            var provider = new DelegateObjectProvider(activator, arguments);

            Register(type, provider);
        }

        /// <summary>
        ///     Registers a custom <see cref="IObjectProvider" /> as a source for future requests for
        ///     <paramref name="type" />
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="provider">The object provider.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="provider" /> is <see langword="null" />.
        /// </exception>
        public static void Register([NotNull] Type type, [NotNull] IObjectProvider provider)
        {
            //- Validate
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (provider == null) { throw new ArgumentNullException(nameof(provider)); }

            Container.Register(type, provider);
        }

        /// <summary>
        ///     Resets all mappings for creating types to their default values.
        /// </summary>
        /// <remarks>
        ///     This is useful for unit testing for cleaning up before invoking each time
        /// </remarks>
        public static void ResetMappings()
        {
            Container.ResetMappings();
        }

        /// <summary>
        ///     Tries to provide an instance of type <typeparamref name="T" /> and returns null if it cannot.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns>A new instance if things were successful; otherwise false.</returns>
        [CanBeNull]
        [SuppressMessage("ReSharper", "CatchAllClause")]
        [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
        [SuppressMessage("ReSharper", "ThrowingSystemException")]
        public static T TryProvideInstance<T>() where T : class
        {
            try
            {
                var instance = ProvideInstanceOfType(typeof(T), false);

                return instance as T;
            }
            catch (Exception ex)
            {
                // We only want certain Exceptions, but enough to not use multiple catches
                if (ex is MissingMemberException || ex is TypeInitializationException
                    || ex is NotSupportedException || ex is InvalidOperationException
                    || ex is InvalidCastException)
                {
                    return null;
                }

                // Rethrow anything else we caught in this block
                throw;
            }
        }

        /// <summary>
        ///     Registers the provided instance as the object to return when <paramref name="type" /> is
        ///     requested.
        /// </summary>
        /// <param name="type">The type that will be requested.</param>
        /// <param name="instance">The instance that will be returned.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="instance" /> is
        ///     <see langword="null" />.
        /// </exception>
        public static void RegisterProvidedInstance([NotNull] Type type, [NotNull] object instance)
        {
            //- Validate
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (instance == null) { throw new ArgumentNullException(nameof(instance)); }

            // Delegate to the container
            Container.RegisterProvidedInstance(type, instance);
        }

        /// <summary>
        ///     Registers the provider as the default provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public static void RegisterDefaultProvider(IObjectProvider provider)
        {
            Container.FallbackProvider = provider;
        }
    }

}