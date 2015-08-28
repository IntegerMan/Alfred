﻿// ---------------------------------------------------------
// CommonProviderExtensions.cs
// 
// Created on:      08/27/2015 at 10:32 PM
// Last Modified:   08/27/2015 at 10:40 PM
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
        /// <param name="arguments">The arguments to pass to the constructor.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     baseType or preferredType
        /// </exception>
        public static void RegisterProvider(
            [NotNull] this Type baseType,
            [NotNull] Type preferredType,
            [CanBeNull] params object[] arguments)
        {
            //- Validate
            if (baseType == null) { throw new ArgumentNullException(nameof(baseType)); }
            if (preferredType == null) { throw new ArgumentNullException(nameof(preferredType)); }

            // Register with the common provider
            CommonProvider.Register(baseType, preferredType, arguments);
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
        /// Registers the provider as the default provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public static void RegisterAsDefaultProvider([CanBeNull] this IObjectProvider provider)
        {
            CommonProvider.DefaultProvider = provider;
        }

        /// <summary>
        /// Provides the instance of the type using the <see cref="CommonProvider"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The provided instance.</returns>
        /// <exception cref="NotSupportedException">
        ///     The type is not correctly configured to allow for
        ///     instantiation.
        /// </exception>
        public static object ProvideInstanceOf([NotNull] this Type type)
        {
            return CommonProvider.ProvideInstanceOfType(type);
        }
    }
}