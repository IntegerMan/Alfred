// ---------------------------------------------------------
// IContainer.cs
// 
// Created on:      08/28/2015 at 10:31 PM
// Last Modified:   08/28/2015 at 10:35 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

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
        ///     Gets or sets the <see cref="Type" /> used when providing collections in
        ///     <see ref="ProvideCollection" />.
        /// </summary>
        /// <value>
        ///     The <see cref="Type" /> to use when providing collections.
        /// </value>
        [NotNull]
        Type CollectionType { get; set; }

        /// <summary>
        ///     Gets or sets the name of the Container.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        string Name { get; set; }

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
        ICollection<TCollectionItem> ProvideCollection<TCollectionItem>();

        /// <summary>
        ///     Provides an instance of the requested type.
        /// </summary>
        /// <typeparam name="TRequested">The type that was requested to be provided.</typeparam>
        /// <param name="args">The arguments.</param>
        /// <exception cref="NotSupportedException">
        /// The type is not correctly configured to allow for instantiation.
        /// </exception>
        /// <returns>An instance of the requested type.</returns>
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
        ///     Removes the mapping for providing a value of the specified <paramref name="type"/> if a
        ///     mapping was present in this container.
        /// </summary>
        /// <param name="type"> The type in this container. </param>
        void RemoveMapping([NotNull] Type type);

        /// <summary>
        ///     Registers a mapping if no mapping for the specified <paramref name="type"/> already
        ///     exists.
        /// </summary>
        /// <param name="type"> The type to register. </param>
        /// <param name="preferredType">
        ///     The preferred type if there is no mapping already present.
        /// </param>
        /// <returns>
        ///     true if it succeeds, false if it was already registered.
        /// </returns>
        bool TryRegister([NotNull] Type type, [NotNull] Type preferredType);

        /// <summary>
        ///     Registers an activator function responsible for instantiating the desired type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="activator">The activator function.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="activator" /> is <see langword="null" />.
        /// </exception>
        void Register(
            [NotNull] Type type,
            [NotNull] Delegate activator);

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
        void Register(
            [NotNull] Type baseType,
            [NotNull] Type preferredType);

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
        void ClearMappings();

        /// <summary>
        ///     Provides an instance of the requested <paramref name="type" /> .
        /// </summary>
        /// <param name="type">The <see cref="Type" /> that was requested to be provided.</param>
        /// <param name="args">The arguments to use when providing or creating the <paramref name="type"/>.</param>
        /// <exception cref="NotSupportedException">
        /// The <paramref name="type"/> is not correctly configured to allow for instantiation.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> is <see langword="null" /> .
        /// </exception>
        /// <returns>An instance of the requested <paramref name="type"/></returns>
        [NotNull]
        object ProvideType([NotNull] Type type, params object[] args);

        /// <summary>
        ///     Attempts to provide an instance of the requested <paramref name="type"/>, returning
        ///     <see langword="null"/> if the <paramref name="type"/> could not be provided.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> that was requested to be provided.</param>
        /// <param name="args">The arguments to use when providing or creating the type.</param>
        /// <returns>
        ///     An instance of the requested <paramref name="type"/> or <see langword="null"/> if
        ///     the <paramref name="type"/> could not be provided.
        /// </returns>
        [CanBeNull]
        object TryProvideType([NotNull] Type type, params object[] args);

        /// <summary>
        ///     Tries to provide an instance of type <typeparamref name="T" /> and returns null if it cannot.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns>A new instance if things were successful; otherwise false.</returns>
        [CanBeNull]
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
        ///     Gets the <see cref="IObjectProvider"/> for the requested <paramref name="type"/>.
        /// </summary>
        /// <remarks>
        ///     This will search not only this container's mappings but also will search its
        ///     <see cref="IObjectContainer.Parent" /> and any additional
        ///     ancestors.
        /// </remarks>
        /// <param name="type">Type that was requested.</param>
        /// <returns>The object provider.</returns>
        [NotNull]
        IObjectProvider GetObjectProvider([NotNull] Type type);
    }

}