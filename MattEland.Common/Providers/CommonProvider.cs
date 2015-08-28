﻿// ---------------------------------------------------------
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
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="baseType" /> or
        ///     <paramref name="preferredType" /> is <see langword="null" />.
        /// </exception>
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
        /// <typeparam name="TRequested">The type that was requested to be provided.</typeparam>
        /// <returns>An instance of the requested type</returns>
        /// <exception cref="NotSupportedException">
        ///     The type is not correctly configured to allow for
        ///     instantiation.
        /// </exception>
        [NotNull]
        public static TRequested Provide<TRequested>(params object[] args)
        {
            var type = typeof(TRequested);

            var instance = ProvideType(type, true, args);
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
        private static object ProvideType([NotNull] Type type, bool errorOnNoInstance, params object[] args)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            return Container.ProvideType(type, errorOnNoInstance, args);
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
            return ProvideType(type, true);
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

            Container.Register(type, activator, arguments);
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
        /// Tries to provide an instance of type <typeparamref name="T" /> and returns null if it cannot.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns>A new instance if things were successful; otherwise false.</returns>
        [CanBeNull]
        public static T TryProvideInstance<T>([CanBeNull] params object[] args) where T : class
        {
            return Container.TryProvideInstance<T>(args);
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