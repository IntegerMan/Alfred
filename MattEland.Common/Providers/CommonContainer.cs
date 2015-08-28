// ---------------------------------------------------------
// CommonContainer.cs
// 
// Created on:      08/28/2015 at 12:53 AM
// Last Modified:   08/28/2015 at 1:14 AM
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
    ///     A dependency injection container used for registering and providing types and instances of
    ///     types to reduce coupling and provide inversion of control. This class can be instantiated and
    ///     used on its own or the default container can be used via the  methods on
    ///     <see cref="CommonProvider" />
    /// </summary>
    /// <remarks>
    ///     Dependency injection containers are discrete containers for <see cref="Type" /> to
    ///     <see cref="IObjectProvider" /> mappings. You can instantiate multiple CommonContainers and each
    ///     one can have different mappings that are entirely separate from each other.
    /// 
    /// Containers can have a parent container which is deferred to if a mapping wasn't found in this container.
    /// </remarks>
    [PublicAPI]
    public class CommonContainer : IObjectProvider
    {

        /// <summary>
        /// The backing field for <see cref="FallbackProvider"/>.
        /// </summary>
        [CanBeNull]
        private IObjectProvider _fallbackProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommonContainer" /> class.
        /// </summary>
        public CommonContainer()
        {
            ProviderMappings = new Dictionary<Type, IObjectProvider>();
            InstanceProvider = new InstanceProvider();
        }

        /// <summary>
        ///     Gets the mapping dictionary that provides a map from a requested <see cref="Type" /> to an
        ///     <see cref="IObjectProvider" /> capable of providing an instance of that type. The key is the
        ///     <see cref="Type" /> requested and the value is the <see cref="IObjectProvider" />.
        /// </summary>
        /// <value>The provider mappings dictionary.</value>
        [NotNull]
        [ItemNotNull]
        private IDictionary<Type, IObjectProvider> ProviderMappings { get; }

        /// <summary>
        ///     Gets the <see cref="IObjectProvider" /> to use when no provider is found.
        /// </summary>
        /// <value>The default object provider.</value>
        public IObjectProvider FallbackProvider
        {
            [NotNull]
            get
            {
                var provider = _fallbackProvider;

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

                    _fallbackProvider = provider;
                }

                return provider;
            }
            [CanBeNull]
            set
            {
                // Null is allowable since the next time get is called it will reset itself

                _fallbackProvider = value;
            }
        }

        /// <summary>
        ///     Gets the instance provider that is used as the provider when
        ///     <see cref="RegisterProvidedInstance" /> is called.
        /// </summary>
        /// <value>The instance provider.</value>
        [NotNull]
        private InstanceProvider InstanceProvider { get; }

        /// <summary>
        ///     Creates an instance of the requested type.
        /// </summary>
        /// <param name="requestedType">The type that was requested.</param>
        /// <returns>A new instance of the requested type</returns>
        public object CreateInstance(Type requestedType)
        {
            return null;
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
        public TRequested ProvideInstance<TRequested>()
        {
            var type = typeof(TRequested);

            var instance = ProvideInstanceOfType(type);
            Debug.Assert(instance != null);

            return (TRequested)instance;
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
        ///     Registers a custom <see cref="IObjectProvider" /> as a source for future requests for
        ///     <paramref name="type" /> in this container
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="provider">The object provider.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="provider" /> is <see langword="null" />.
        /// </exception>
        public void Register([NotNull] Type type, [NotNull] IObjectProvider provider)
        {
            //- Validate
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (provider == null) { throw new ArgumentNullException(nameof(provider)); }

            ProviderMappings[type] = provider;
        }

        /// <summary>
        ///     Gets the object provider for the requested type.
        /// </summary>
        /// <param name="requestedType">Type that was requested.</param>
        /// <returns>The object provider.</returns>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private IObjectProvider GetObjectProvider([NotNull] Type requestedType)
        {
            //- TODO: It might be nice to have an option to disable defaulting and throw an exception instead

            // Grab our registered mapping. If we don't have one, then use our default provider
            return ProviderMappings.ContainsKey(requestedType)
                       ? ProviderMappings[requestedType]
                       : FallbackProvider;
        }

        /// <summary>
        ///     Resets all mappings for creating types to their default values.
        /// </summary>
        /// <remarks>
        ///     This is useful for unit testing for cleaning up before invoking each time
        /// </remarks>
        public void ResetMappings()
        {
            // Clear out all usages
            ProviderMappings.Clear();

            // Reset the default provider as well
            _fallbackProvider = null;
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
        public object ProvideInstanceOfType([NotNull] Type type, bool errorOnNoInstance = true)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            // Determine which type to create
            var provider = GetObjectProvider(type);

            // When using the Instance Provider, make sure the fallback is accurate
            if (provider == InstanceProvider)
            {
                InstanceProvider.FallbackProvider = FallbackProvider;
            }

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
        ///     Registers the provided instance as the object to return when <paramref name="type" /> is
        ///     requested.
        /// </summary>
        /// <param name="type">The type that will be requested.</param>
        /// <param name="instance">The instance that will be returned.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="instance" /> is
        ///     <see langword="null" />.
        /// </exception>
        public void RegisterProvidedInstance([NotNull] Type type, [NotNull] object instance)
        {
            //- Validate
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (instance == null) { throw new ArgumentNullException(nameof(instance)); }

            // Put this instance inside the instance provider
            InstanceProvider.Register(type, instance);

            // Tell the system to read from the Instance Provider when getting values for this.
            ProviderMappings.Add(type, InstanceProvider);
        }
    }
}