// ---------------------------------------------------------
// CommonProviderExtensions.cs
// 
// Created on:      08/27/2015 at 10:32 PM
// Last Modified:   08/28/2015 at 12:42 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace MattEland.Common.Providers
{
    /// <summary>
    ///     Contains extension methods related to <see cref="CommonProvider" /> dependency injection
    ///     containers.
    /// </summary>
    [PublicAPI]
    public static class CommonProviderExtensions
    {
        /// <summary>
        ///     Registers a preferred type as the type to provide when the base type is requested.
        /// </summary>
        /// <param name="baseType">Type that will be requested in the future.</param>
        /// <param name="preferredType">Type to instantiate instead of the base type.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     baseType or preferredType
        /// </exception>
        public static void RegisterProvider(
            [NotNull] this Type baseType,
            [NotNull] Type preferredType)
        {
            //- Validate
            if (baseType == null) { throw new ArgumentNullException(nameof(baseType)); }
            if (preferredType == null) { throw new ArgumentNullException(nameof(preferredType)); }

            // Register with the common provider
            CommonProvider.Register(baseType, preferredType);
        }

        /// <summary>
        ///     Registers a delegate to invoke to provide an instance when the base type is requested.
        /// </summary>
        /// <param name="baseType">Type that will be requested in the future.</param>
        /// <param name="activationDelegate">The activation delegate to invoke.</param>
        /// <param name="arguments">The arguments to pass to the delegate.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     baseType or activationDelegate
        /// </exception>
        public static void RegisterProvider(
            [NotNull] this Type baseType,
            [NotNull] Delegate activationDelegate,
            [CanBeNull] params object[] arguments)
        {
            //- Validate
            if (baseType == null) { throw new ArgumentNullException(nameof(baseType)); }
            if (activationDelegate == null)
            {
                throw new ArgumentNullException(nameof(activationDelegate));
            }

            // Register with the common provider
            CommonProvider.Register(baseType, activationDelegate, arguments);
        }

        /// <summary>
        ///     Registers the provider as the default provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public static void RegisterAsDefaultProvider([CanBeNull] this IObjectProvider provider)
        {
            CommonProvider.RegisterDefaultProvider(provider);
        }

        /// <summary>
        /// Provides the instance of the type using the <see cref="CommonProvider" />.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The provided instance.</returns>
        /// <exception cref="NotSupportedException">The type is not correctly configured to allow for
        /// instantiation.</exception>
        public static object ProvideInstanceOf([NotNull] this Type type)
        {
            return CommonProvider.ProvideType(type);
        }

        /// <summary>
        ///     Registers the instance as the instance that will be provided when
        ///     <see cref="CommonProvider.Provide{TRequested}" /> or
        ///     <see cref="CommonProvider.ProvideType" /> is called for type <paramref name="type" />
        ///     .
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="type">The type that will be requested.</param>
        /// <exception cref="System.ArgumentNullException">type, instance</exception>
        public static void RegisterAsProvidedInstance(
            [NotNull] this object instance,
            [NotNull] Type type)
        {
            //- Validate
            if (instance == null) { throw new ArgumentNullException(nameof(instance)); }
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            // Register
            CommonProvider.RegisterProvidedInstance(type, instance);
        }
    }
}