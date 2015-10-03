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
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public static class CommonProvider
    {
        [CanBeNull]
        private static IObjectContainer _container;

        /// <summary>
        ///     Gets or sets the default dependency injection container.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">
        ///     Cannot access the common container when
        ///     <see ref="RestrictCommonContainer" /> is
        ///     <see langword="true" />. This is typically enabled for access troubleshooting.
        /// </exception>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public static IObjectContainer Container
        {
            get
            {
                // Restrict permissions for diagnostic purposes. #IHatePragmas
                CheckAccessRestrictions();

                // Lazy load
                if (_container == null)
                {
                    _container = new CommonContainer() { Name = "CommonProvider Container" };
                }
                return _container;
            }
            set
            {
                _container = value;
            }
        }

        /// <summary>
        ///     Checks <see cref="RestrictCommonContainer"/> and throws an UnauthorizedAccessException if
        ///     access should be restricted.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">
        ///     Thrown when an Unauthorized Access error condition occurs.
        /// </exception>
        private static void CheckAccessRestrictions()
        {
            if (RestrictCommonContainer)
            {
                throw new UnauthorizedAccessException(
                    "Cannot access the common container when RestrictCommonContainer is True. This is typically enabled for access troubleshooting.");
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to restrict access to the common container by
        ///     throwing an <see cref="UnauthorizedAccessException"/> when it is accessed. This is
        ///     typically <see langword="false"/> except in some debugging scenarios.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if restrict common container, <see langword="false"/> if not.
        /// </value>
        public static bool RestrictCommonContainer { get; set; }

        /// <summary>
        ///     Registers the preferred type as the type to instantiate when the base type is requested.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="baseType" /> or
        ///     <paramref name="preferredType" /> is <see langword="null" />.
        /// </exception>
        /// <param name="baseType"> The type that will be requested. </param>
        /// <param name="preferredType">
        ///     The type that should be created when <see cref="baseType" /> is requested.
        /// </param>
        public static void Register(
            [NotNull] Type baseType,
            [NotNull] Type preferredType)
        {
            //- Validate
            if (baseType == null) { throw new ArgumentNullException(nameof(baseType)); }
            if (preferredType == null) { throw new ArgumentNullException(nameof(preferredType)); }

            Container.Register(baseType, preferredType);
        }

        /// <summary>
        ///     Provides an instance of the requested type.
        /// </summary>
        /// <typeparam name="TRequested"> The type that was requested to be provided. </typeparam>
        /// <param name="args"> The arguments. </param>
        /// <returns>
        ///     An instance of the requested type.
        /// </returns>
        ///
        /// ### <exception cref="NotSupportedException">
        ///     The type is not correctly configured to allow for instantiation.
        /// </exception>
        [NotNull]
        public static TRequested Provide<TRequested>(params object[] args)
        {
            var type = typeof(TRequested);

            var instance = Container.ProvideType(type, args);
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
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null" />.</exception>
        [CanBeNull]
        public static object ProvideType([NotNull] Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            return Container.ProvideType(type);
        }

        /// <summary>
        ///     Registers an activator function responsible for instantiating the desired type.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="activator" /> is <see langword="null" />.
        /// </exception>
        /// <param name="type"> The type. </param>
        /// <param name="activator"> The activator function. </param>
        public static void Register(
            [NotNull] Type type,
            [NotNull] Delegate activator)
        {
            //- Validate
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (activator == null) { throw new ArgumentNullException(nameof(activator)); }

            Container.Register(type, activator);
        }

        /// <summary>
        ///     Registers a custom <see cref="IObjectProvider" /> as a source for future requests for
        ///     <paramref name="type" />
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="provider" /> is <see langword="null" />.
        /// </exception>
        /// <param name="type"> The type. </param>
        /// <param name="provider"> The object provider. </param>
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
            Container.ClearMappings();
        }

        /// <summary>
        ///     Tries to provide an instance of type <typeparamref name="T" /> and returns null if it
        ///     cannot.
        /// </summary>
        /// <typeparam name="T"> The type to return. </typeparam>
        /// <param name="args"> The arguments. </param>
        /// <returns>
        ///     A new instance if things were successful; otherwise false.
        /// </returns>
        [CanBeNull]
        public static T TryProvideInstance<T>([CanBeNull] params object[] args) where T : class
        {
            return Container.TryProvide<T>(args);
        }

        /// <summary>
        ///     Registers the provided instance as the object to return when <paramref name="type" /> is
        ///     requested.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="instance" /> is
        ///     <see langword="null" />.
        /// </exception>
        /// <param name="type"> The type that will be requested. </param>
        /// <param name="instance"> The instance that will be returned. </param>
        public static void RegisterProvidedInstance([NotNull] Type type, [NotNull] object instance)
        {
            //- Validate
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (instance == null) { throw new ArgumentNullException(nameof(instance)); }

            // Delegate to the container
            Container.RegisterProvidedInstance(type, instance);
        }

        /// <summary>
        ///     Registers the <paramref name="provider"/> as the default provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public static void RegisterDefaultProvider(IObjectProvider provider)
        {
            Container.FallbackProvider = provider;
        }

        /// <summary>
        ///     Provides a collection of the specified type of <see langword="object" /> .
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This functions by calling <see ref="Provide"/> on the ICollection&lt;gt; generic type.
        ///     </para>
        ///     <para>
        ///         It's highly advised to provide a new instance via <see ref="Provide" /> (as opposed
        ///         to using <see ref="RegisterProvidedInstance"/>) on ICollection&lt;gt;
        ///     </para>
        /// </remarks>
        /// <typeparam name="TCollectionItem"> The type of items the collection supports. </typeparam>
        /// <returns>
        ///     The new collection. This will not be <see langword="null" /> .
        /// </returns>
        [NotNull]
        public static ICollection<TCollectionItem> ProvideCollection<TCollectionItem>()
        {
            return Container.ProvideCollection<TCollectionItem>();
        }


        /// <summary>
        ///     Gets or sets the <see cref="Type" /> used when providing collections in
        ///     <see ref="ProvideCollection" />.
        /// </summary>
        /// <value>
        ///     The <see cref="Type" /> to use when providing collections.
        /// </value>
        [NotNull]
        public static Type CollectionType
        {
            get { return Container.CollectionType; }
            set { Container.CollectionType = value; }
        }

    }

}