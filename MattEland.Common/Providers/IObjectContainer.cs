// ---------------------------------------------------------
// IContainer.cs
// 
// Created on:      08/28/2015 at 10:31 PM
// Last Modified:   08/28/2015 at 10:35 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Common.Providers
{
    /// <summary>
    ///     Represents an inversion of control container for dependency injection.
    /// </summary>
    [PublicAPI]
    public interface IObjectContainer : IObjectProvider
    {
        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        IObjectContainer Parent { get; set; }

        /// <summary>
        ///     Gets the <see cref="IObjectProvider" /> to use when no provider is found.
        /// </summary>
        /// <value>The default object provider.</value>
        IObjectProvider FallbackProvider
        {
            [NotNull]
            get;
            [CanBeNull]
            set;
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
        TRequested Provide<TRequested>(params object[] args);

        /// <summary>
        ///     Registers a custom <see cref="IObjectProvider" /> as a source for future requests for
        ///     <paramref name="type" /> in this container
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="provider">The object provider.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="provider" /> is <see langword="null" />.
        /// </exception>
        void Register([NotNull] Type type, [NotNull] IObjectProvider provider);

        /// <summary>
        ///     Registers an activator function responsible for instantiating the desired type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="activator">The activator function.</param>
        /// <param name="arguments">The arguments to pass in to the activator function.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="activator" /> is <see langword="null" />.
        /// </exception>
        void Register(
            [NotNull] Type type,
            [NotNull] Delegate activator,
            [CanBeNull] params object[] arguments);

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
        void Register(
            [NotNull] Type baseType,
            [NotNull] Type preferredType,
            [CanBeNull] params object[] arguments);

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
        void RegisterProvidedInstance([NotNull] Type type, [NotNull] object instance);

        /// <summary>
        ///     Resets all mappings for creating types to their default values.
        /// </summary>
        /// <remarks>
        ///     This is useful for unit testing for cleaning up before invoking each time
        /// </remarks>
        void ResetMappings();

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
        object ProvideType([NotNull] Type type, bool errorOnNoInstance, params object[] args);

        /// <summary>
        ///     Tries to provide an instance of type <typeparamref name="T" /> and returns null if it cannot.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns>A new instance if things were successful; otherwise false.</returns>
        T TryProvide<T>([CanBeNull] params object[] args) where T : class;

        /// <summary>
        ///     Determines whether the specified type has a mapping in this container or any of its parents.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type has mapping; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="type" /> is <see langword="null" />.</exception>
        bool HasMapping([NotNull] Type type);

        /// <summary>
        ///     Gets the object provider for the requested type.
        /// </summary>
        /// <remarks>
        ///     This will search not only this container's mappings but also will search its
        ///     <see cref="Parent" /> and any additional ancestors.
        /// </remarks>
        /// <param name="type">Type that was requested.</param>
        /// <returns>The object provider.</returns>
        [NotNull]
        IObjectProvider GetObjectProvider([NotNull] Type type);
    }
}