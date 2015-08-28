// ---------------------------------------------------------
// CommonProvider.cs
// 
// Created on:      08/27/2015 at 2:55 PM
// Last Modified:   08/28/2015 at 12:21 AM
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

        private static IObjectProvider _defaultObjectProvider;

        /// <summary>
        ///     Gets the mapping dictionary that provides a map from a requested <see cref="Type" /> to an
        ///     <see cref="IObjectProvider" /> capable of providing an instance of that type. The key is the
        ///     <see cref="Type" /> requested and the value is the <see cref="IObjectProvider" />.
        /// </summary>
        /// <value>The provider mappings dictionary.</value>
        [NotNull]
        [ItemNotNull]
        private static IDictionary<Type, IObjectProvider> ProviderMappings { get; } =
            new Dictionary<Type, IObjectProvider>();

        /// <summary>
        ///     Gets the <see cref="IObjectProvider" /> to use when no provider is found.
        /// </summary>
        /// <value>The default object provider.</value>
        [NotNull]
        public static IObjectProvider DefaultProvider
        {
            get
            {
                var provider = _defaultObjectProvider;

                if (provider == null)
                {
                    /* Here we're going to use the container to try to create an instance to use
                    for the default provider. If none is found, we'll use the default activator type. */

                    if (ProviderMappings.ContainsKey(typeof(IObjectProvider)))
                    {
                        provider = ProvideInstance<IObjectProvider>();
                    }
                    else
                    {
                        provider = new ActivatorObjectProvider();
                    }

                    _defaultObjectProvider = provider;
                }

                return provider;
            }
            set
            {
                // Null is allowable since the next time get is called it will reset itself

                _defaultObjectProvider = value;
            }
        }

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

            var instance = ProvideInstanceOfType(type, true);
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
        [CanBeNull]
        public static object ProvideInstanceOfType(
            [NotNull] Type type,
            bool errorOnNoInstance = true)
        {
            // Determine which type to create
            var provider = GetObjectProvider(type);

            try
            {
                /* Create and return an instance of the requested type using the type 
                   determined earlier. This can throw many exceptions which will be
                   wrapped into more user-friendly exceptions with easier error handling. */

                var instance = provider.CreateInstance(type);

                // Some callers want exceptions on not found; others don't
                if (instance == null && errorOnNoInstance)
                {
                    ThrowNotProvidedException(type.FullName);
                }

                return instance;
            }
            catch (MissingMemberException ex)
            {
                // Try to throw the same type of exception with additional information.
                string msg =
                    $"Could not instantiate {type.FullName} due to missing member exception: '{ex.Message}'";

                throw new NotSupportedException(msg, ex);
            }
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
        ///     Gets the object provider for the requested type.
        /// </summary>
        /// <param name="requestedType">Type that was requested.</param>
        /// <returns>The object provider.</returns>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private static IObjectProvider GetObjectProvider([NotNull] Type requestedType)
        {
            //- TODO: It might be nice to have an option to disable defaulting and throw an exception instead

            // Grab our registered mapping. If we don't have one, then use our default provider
            return ProviderMappings.ContainsKey(requestedType)
                       ? ProviderMappings[requestedType]
                       : DefaultProvider;
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

            ProviderMappings[type] = provider;
        }

        /// <summary>
        ///     Resets all mappings for creating types to their default values.
        /// </summary>
        /// <remarks>
        ///     This is useful for unit testing for cleaning up before invoking each time
        /// </remarks>
        public static void ResetMappings()
        {
            // Clear out all usages
            ProviderMappings.Clear();

            // Reset the default provider as well
            _defaultObjectProvider = null;
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
    }

}