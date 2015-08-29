// ---------------------------------------------------------
// CommonContainer.cs
// 
// Created on:      08/28/2015 at 12:53 AM
// Last Modified:   08/29/2015 at 4:39 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

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
    ///     one can have different mappings that are entirely separate from each other. Containers can have
    ///     a parent container which is deferred to if a mapping wasn't found in this container.
    /// </remarks>
    [PublicAPI]
    public class CommonContainer : IObjectContainer
    {
        /// <summary>The <see cref="Type" /> used when providing collections.</summary>
        [NotNull]
        private Type _collectionType = typeof(List<>);

        /// <summary>The backing field for <see cref="FallbackProvider" />.</summary>
        [CanBeNull]
        private IObjectProvider _fallbackProvider;

        /// <summary>The backing field for <see cref="Parent" />
        /// </summary>
        [CanBeNull]
        private IObjectContainer _parent;

        /// <summary>
        ///     Gets the mapping dictionary that provides a map from a requested <see cref="Type" /> to an
        ///     <see cref="IObjectProvider" /> capable of providing an instance of that type. The key is the
        ///     <see cref="Type" /> requested and the value is the <see cref="IObjectProvider" />.
        /// </summary>
        /// <value>The provider mappings dictionary.</value>
        [NotNull]
        [ItemNotNull]
        private IDictionary<Type, IObjectProvider> Mappings { get; }

        /// <summary>Gets the instance provider that is used as the provider when
        ///     <see cref="RegisterProvidedInstance" /> is called.</summary>
        /// <value>The instance provider.</value>
        [NotNull]
        private InstanceProvider InstanceProvider { get; }

        /// <summary>Gets or sets the <see cref="Type" /> used when providing collections in
        ///     <see ref="ProvideCollection" />.</summary>
        /// <exception cref="ArgumentException"> Exception must  </exception>
        /// <value>The <see cref="Type" /> to use when providing collections.</value>
        [NotNull]
        public Type CollectionType
        {
            get { return _collectionType; }
            set
            {
                // Validate
                if (value == null || !value.IsGenericType)
                {
                    throw new ArgumentException(
                        Resources.CommonContainerCollectionTypeMustBeGeneric,
                        nameof(value));
                }

                _collectionType = value;
            }
        }

        /// <summary>Gets the <see cref="IObjectProvider" /> to use when no provider is found.</summary>
        /// <value>The default object provider.</value>
        public IObjectProvider FallbackProvider
        {
            [NotNull]
            get
            {
                var provider = _fallbackProvider;

                if (provider != null) { return provider; }

                /* Here we're going to use the container to try to create an instance to use
                    for the default provider. If none is found, we'll use the default activator type. */

                if (Mappings.ContainsKey(typeof(IObjectProvider)))
                {
                    provider = Provide<IObjectProvider>();
                }
                else
                {
                    provider = new ActivatorObjectProvider();
                }

                _fallbackProvider = provider;

                return provider;
            }
            [CanBeNull]
            set
            {
                // Null is allowable since the next time get is called it will reset itself

                _fallbackProvider = value;
            }
        }

        /// <summary>Gets the <see cref="IObjectProvider" /> for the requested <paramref name="type" />.</summary>
        /// <remarks>This will search not only this container's <see cref="Mappings" /> and its
        ///     <see cref="Parent" /> container and any additional ancestor containers.</remarks>
        /// <param name="type">The <see cref="Type" /> that was requested.</param>
        /// <returns>The object provider.</returns>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public IObjectProvider GetObjectProvider([NotNull] Type type)
        {
            //- TODO: It might be nice to have an option to disable defaulting and throw an exception instead

            // Grab our registered mapping. If we don't have one, then use our default provider
            if (Mappings.ContainsKey(type)) { return Mappings[type]; }

            if (Parent != null && Parent.HasMapping(type))
            {
                return Parent.GetObjectProvider(type);
            }

            return FallbackProvider;
        }

        /// <summary>Determines whether the specified <paramref name="type" /> has a mapping in this
        ///     <see cref="IObjectContainer" /> or any of its <see cref="Parent" /> containers.</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type has mapping; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="type" /> is <see langword="null" />.</exception>
        public bool HasMapping([NotNull] Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            return Mappings.ContainsKey(type);
        }

        /// <summary>Gets or sets the parent container.</summary>
        /// <value>The parent.</value>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="InvalidOperationException">Cannot set a container to have itself as a parent</exception>
        public IObjectContainer Parent
        {
            get { return _parent; }
            set
            {
                // Exit early if null to avoid parent checks
                if (value == null)
                {
                    _parent = null;
                    return;
                }

                /* Check to see that we're not assigning this object as a parent or 
                   assigning to a chain where an item would have this container as its
                   parent (a circular relationship) */

                var p = value;
                while (p != null)
                {
                    if (p == this)
                    {
                        const string Message =
                            "Cannot set a container to have itself as a parent or grandparent";
                        throw new InvalidOperationException(Message);
                    }

                    // Move to next item in the chain
                    p = p.Parent;
                }

                _parent = value;
            }
        }

        /// <summary>Provides an instance of the requested type.</summary>
        /// <typeparam name="TRequested">The type that was requested to be provided.</typeparam>
        /// <returns>An instance of the requested type</returns>
        /// <exception cref="NotSupportedException">
        ///     The type is not correctly configured to allow for
        ///     instantiation.
        /// </exception>
        [NotNull]
        public TRequested Provide<TRequested>(params object[] args)
        {
            var type = typeof(TRequested);

            var instance = ProvideTypePrivate(type, true, args);
            Debug.Assert(instance != null);

            return (TRequested)instance;
        }

        /// <summary>Provides a collection of the specified type of <see langword="object" /> .</summary>
        /// <remarks>
        ///     <para>This functions by calling <see ref="Provide" /> on the ICollection&lt;gt; generic type.</para>
        ///     <para>
        ///         It's highly advised to provide a new instance via <see ref="Provide" /> (as opposed to
        ///         using <see ref="RegisterProvidedInstance" />) on ICollection&lt;gt;
        ///     </para>
        /// </remarks>
        /// <typeparam name="TCollectionItem"> The type of items the collection supports. </typeparam>
        /// <returns>The new collection. This will not be <see langword="null" /> .</returns>
        public ICollection<TCollectionItem> ProvideCollection<TCollectionItem>()
        {
            var genericType = typeof(TCollectionItem);
            var tConcreteCollection = CollectionType.MakeGenericType(genericType);

            var genericCollection = ProvideType(tConcreteCollection);
            Debug.Assert(genericCollection != null);

            return (ICollection<TCollectionItem>)genericCollection;
        }

        /// <summary>Provides an instance of the requested <paramref name="type" />.</summary>
        /// <paramref name="type">The <see cref="Type" /> that was requested to be provided.</paramref>
        /// <returns>An instance of the requested type</returns>
        /// <exception cref="NotSupportedException">
        ///     The type is not correctly configured to allow for
        ///     instantiation.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="type" /> is <see langword="null" />.</exception>
        [NotNull]
        public object ProvideType([NotNull] Type type, params object[] args)
        {
            var value = ProvideTypePrivate(type, true, args);
            Debug.Assert(value != null);

            return value;
        }

        /// <summary>Registers a custom <see cref="IObjectProvider" /> as a source for future requests for
        ///     <paramref name="type" /> in this container</summary>
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

            Mappings[type] = provider;
        }

        /// <summary>
        ///     Registers an <paramref name="activator" /> function responsible for instantiating the
        ///     desired type.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="activator" /> is <see langword="null" />.
        /// </exception>
        /// <param name="type"> The type. </param>
        /// <param name="activator"> The activator function. </param>
        /// <param name="arguments"> The arguments to pass in to the activator function. </param>
        public void Register(
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
        ///     Registers the preferred type as the type to instantiate when the
        ///     <paramref name="baseType" /> is requested.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="baseType" /> or <paramref name="preferredType" /> is
        ///     <see langword="null" /> .
        /// </exception>
        /// <param name="baseType"> The type that will be requested. </param>
        /// <param name="preferredType">
        ///     The type that should be created when <see cref="baseType" /> is
        ///     requested.
        /// </param>
        /// <param name="arguments"> The arguments (if any) to pass to the class's constructor. </param>
        public void Register(
            [NotNull] Type baseType,
            [NotNull] Type preferredType,
            [CanBeNull] params object[] arguments)
        {
            //- Validate
            if (baseType == null) { throw new ArgumentNullException(nameof(baseType)); }
            if (preferredType == null) { throw new ArgumentNullException(nameof(preferredType)); }

            ValidateTypeRegistration(baseType, preferredType);

            // Register the type mapping using the default Activator-based model.
            var provider = new ActivatorObjectProvider(preferredType);
            Register(baseType, provider);
        }

        /// <summary>
        ///     Registers the provided <paramref name="instance" /> as the <see langword="object" /> to
        ///     return when <paramref name="type" /> is requested.
        /// </summary>
        /// <param name="type">The type that will be requested.</param>
        /// <param name="instance">The instance that will be returned.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="instance" /> is <see langword="null" /> .
        /// </exception>
        public void RegisterProvidedInstance([NotNull] Type type, [NotNull] object instance)
        {
            //- Validate
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (instance == null) { throw new ArgumentNullException(nameof(instance)); }

            // Put this instance inside the instance provider
            InstanceProvider.Register(type, instance);

            /* Tell the system to read from the Instance Provider when getting values for this. 
               The InstanceProvider provides all values registered this way using an internal dictionary.
               This operation will handle both add to dictionary and update existing entry. */

            Mappings[type] = InstanceProvider;
        }

        /// <summary>Resets all mappings for creating types to their default values.</summary>
        /// <remarks>This is useful for unit testing for cleaning up before invoking each time</remarks>
        public void ResetMappings()
        {
            // Clear out all usages
            Mappings.Clear();

            // Reset the default provider as well
            _fallbackProvider = null;
        }

        /// <summary>
        ///     Tries to provide an instance of type <typeparamref name="T" /> and returns null if it
        ///     cannot.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns>A new instance if things were successful; otherwise false.</returns>
        [CanBeNull]
        [SuppressMessage("ReSharper", "CatchAllClause")]
        [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
        [SuppressMessage("ReSharper", "ThrowingSystemException")]
        public T TryProvide<T>([CanBeNull] params object[] args) where T : class
        {
            try
            {
                var instance = ProvideTypePrivate(typeof(T), false, args);

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

                // Throw anything else we caught in this block
                throw;
            }
        }

        /// <summary>Provides an instance of the requested <paramref name="type" />.</summary>
        /// <paramref name="type">The <see cref="Type" /> that was requested to be provided.</paramref>
        /// <returns>An instance of the requested type</returns>
        /// <exception cref="NotSupportedException">
        ///     The type is not correctly configured to allow for
        ///     instantiation.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="type" /> is <see langword="null" />.</exception>
        [CanBeNull]
        public object TryProvideType([NotNull] Type type, params object[] args)
        {
            return ProvideTypePrivate(type, false, args);
        }

        /// <summary>Creates an instance of the requested type.</summary>
        /// <param name="requestedType">The type that was requested.</param>
        /// <param name="args">The arguments</param>
        /// <returns>A new instance of the requested type</returns>
        object IObjectProvider.CreateInstance(Type requestedType, params object[] args)
        {
            return ProvideType(requestedType, args);
        }

        /// <summary>Adds default mappings to the container.</summary>
        public void AddDefaultMappings()
        {
        }

        /// <summary>
        ///     Provides an instance of the requested <paramref name="type" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The type is not correctly configured to allow for instantiation.
        /// </exception>
        /// <param name="type"> The <see cref="Type" /> that was requested to be provided. </param>
        /// <param name="errorOnNoInstance"> true to error on no instance. </param>
        /// <param name="args"> The arguments. </param>
        /// <returns>
        ///     An instance of the requested type.
        /// </returns>
        [CanBeNull]
        public object ProvideTypePrivate(
            [NotNull] Type type,
            bool errorOnNoInstance,
            params object[] args)
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

                var instance = provider.CreateInstance(type, args);

                // Some callers want exceptions on not found; others don't
                if (instance == null && errorOnNoInstance)
                {
                    string typeName = type.FullName;

                    var arguments = GetArgumentsString(args);

                    var message = $"The {provider.GetType().FullName} activator for creating {typeName} returned a null value with arguments: {arguments}";
                    throw new NotSupportedException(message);
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
        ///     Gets an arguments string for the specified <paramref name="arguments"/>.
        /// </summary>
        /// <param name="arguments"> The arguments to pass in to the activator function. </param>
        /// <returns>
        ///     The arguments string.
        /// </returns>
        private static string GetArgumentsString(object[] arguments)
        {
            if (arguments == null || !arguments.Any())
            {
                return "[No Arguments]";
            }

            var sb = new StringBuilder();
            foreach (var argument in arguments)
            {
                sb.AppendFormat("['{0}'] ", argument.AsNonNullString());
            }
            return sb.ToString();
        }

        /// <summary>Validates that <see cref="preferredType" /> is something that can be created.</summary>
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
        private void ValidateTypeRegistration([NotNull] Type baseType, [NotNull] Type preferredType)
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

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="CommonContainer" /> class.</summary>
        public CommonContainer() : this(true)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommonContainer" /> class.</summary>
        /// <param name="addDefaultMappings"> Whether or not to add default mappings. </param>
        public CommonContainer(bool addDefaultMappings) : this(addDefaultMappings, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommonContainer" /> class.</summary>
        /// <param name="parent">The parent container.</param>
        public CommonContainer([CanBeNull] IObjectContainer parent) : this(true, parent)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommonContainer" /> class.</summary>
        /// <param name="addDefaultMappings"> Whether or not to add default mappings. </param>
        /// <param name="parent">The parent container.</param>
        public CommonContainer(bool addDefaultMappings, [CanBeNull] IObjectContainer parent)
        {
            Mappings = new Dictionary<Type, IObjectProvider>();
            InstanceProvider = new InstanceProvider();

            Parent = parent;

            if (addDefaultMappings) { AddDefaultMappings(); }
        }

        #endregion
    }
}