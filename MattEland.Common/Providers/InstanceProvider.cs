// ---------------------------------------------------------
// InstanceProvider.cs
// 
// Created on:      08/27/2015 at 11:12 PM
// Last Modified:   08/28/2015 at 12:18 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;
using System.Diagnostics.Contracts;

namespace MattEland.Common.Providers
{
    /// <summary>
    ///     An <see cref="IObjectProvider" /> that provides pre-configured instances for types in its
    ///     <see cref="Mappings" />. Note that it will provide the actual instances and will not
    ///     instantiate new types. This class is also a decorator and will use the decorated provider (if
    ///     one is present) as a fallback.
    /// </summary>
    [PublicAPI]
    public sealed class InstanceProvider : IObjectProvider
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public InstanceProvider()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProvider"/> class.
        /// </summary>
        /// <param name="fallbackProvider">The fallback provider.</param>
        public InstanceProvider([CanBeNull] IObjectProvider fallbackProvider)
        {
            FallbackProvider = fallbackProvider;
        }

        /// <summary>
        ///     Gets the mappings Dictionary containing a keyed mapping of requested type to concrete
        ///     instances.
        /// </summary>
        /// <value>The mappings dictionary.</value>
        [NotNull]
        [ItemNotNull]
        private IDictionary<Type, object> Mappings { get; } = new Dictionary<Type, object>();

        /// <summary>
        ///     Gets or sets the fallback provider that is used when there is no mapping. If this is
        ///     <see langword="null" /> , a <see cref="NotSupportedException" /> will be thrown in
        ///     <see cref="InstanceProvider.CreateInstance" /> if no mapping is found.
        /// </summary>
        /// <value>
        ///     The fallback provider.
        /// </value>
        [CanBeNull]
        public IObjectProvider FallbackProvider { get; set; }

        /// <summary>
        ///     Creates an instance of the requested type using the predefined mappings. If no mapping
        ///     is found, the <see cref="FallbackProvider" /> will be used. If there is no mapping and no
        ///     <see cref="FallbackProvider" />, this will return null.
        /// </summary>
        /// <param name="requestedType"> The type that was requested. </param>
        /// <param name="args"> The arguments. </param>
        /// <returns>
        ///     A new instance of the requested type or null.
        /// </returns>
        [CanBeNull]
        public object CreateInstance([NotNull] Type requestedType, [CanBeNull] params object[] args)
        {
            /* Grab from our mappings if present, otherwise defer to the fallback
               provider if present. If one isn't, send back null. The system will 
               have to deal with null and throw exceptions as needed. */

            return Mappings.ContainsKey(requestedType)
                       ? Mappings[requestedType]
                       : FallbackProvider?.CreateInstance(requestedType, args);
        }

        /// <summary>
        ///     Adds the instance as the value provided when <paramref name="type" /> is requested in
        ///     <see cref="CreateInstance" />.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type" /> or <paramref name="instance" /> is <see langword="null" />.
        /// </exception>
        public void Register([NotNull] Type type, [NotNull] object instance)
        {
            //- Validate
            Contract.Requires(type != null, "type is null.");
            Contract.Requires(instance != null, "instance is null.");

            Mappings[type] = instance;
        }

        /// <summary>
        ///     Clears all pre-defined mappings.
        /// </summary>
        public void ClearMappings()
        {
            Mappings.Clear();
        }
    }
}