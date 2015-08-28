// ---------------------------------------------------------
// CommonProvider.cs
// 
// Created on:      08/27/2015 at 2:55 PM
// Last Modified:   08/27/2015 at 10:11 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
        /// <exception cref="ArgumentNullException"
        ///            accessor="set">
        ///     <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        [NotNull]
        public static IObjectProvider DefaultObjectProvider
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
                        provider = Create<IObjectProvider>();
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
                if (value == null) { throw new ArgumentNullException(nameof(value)); }

                // TODO: Test this!

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
            ProviderMappings[baseType] = provider;
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
        ///     Creates an instance of the requested type.
        /// </summary>
        /// <typeparam name="TRequested">The type that was requested to be created.</typeparam>
        /// <returns>An instance of the requested type</returns>
        /// <exception cref="InvalidOperationException">
        ///     The type is not correctly configured to allow for
        ///     instantiation.
        /// </exception>
        [NotNull]
        public static TRequested Create<TRequested>()
        {
            var requestedType = typeof(TRequested);

            // Determine which type to create
            var provider = GetObjectProvider(requestedType);

            try
            {
                /* Create and return an instance of the requested type using the type 
                   determined earlier. This can throw many exceptions which will be
                   wrapped into more user-friendly exceptions with easier error handling. */

                // ReSharper disable once EventExceptionNotDocumented
                var instance = provider.CreateInstance(requestedType);

                if (instance == null)
                {
                    throw new InvalidOperationException("The activator function for creating "
                                                        + requestedType.FullName
                                                        + " returned a null value.");
                }

                return (TRequested)instance;
            }
            catch (MissingMemberException ex)
            {
                // Improve the thrown exception with more information.
                string message =
                    $"Could not instantiate {requestedType.FullName} due to missing member exception: '{ex.Message}'";

                throw new InvalidOperationException(message, ex);
            }
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
            return ProviderMappings.ContainsKey(requestedType)
                       ? ProviderMappings[requestedType]
                       : DefaultObjectProvider;
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

            ProviderMappings[type] = provider;
        }

        /// <summary>
        ///     Clears all mappings for creating types.
        /// </summary>
        /// <remarks>
        ///     This is useful for unit testing for cleaning up before invoking each time
        /// </remarks>
        public static void ClearMappings()
        {
            ProviderMappings.Clear();
        }
    }

}