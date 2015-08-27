// ---------------------------------------------------------
// CommonProvider.cs
// 
// Created on:      08/27/2015 at 2:55 PM
// Last Modified:   08/27/2015 at 3:20 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Common.Providers
{
    /// <summary>
    ///     A dependency injection container used to quickly instantiate classes without coupling ourselves
    ///     to that implementation.
    /// </summary>
    public static class CommonProvider
    {

        /// <summary>
        ///     Registers the preferred type as the type to instantiate when the base type is requested.
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        public static void Register([NotNull] Type baseType) { Register(baseType, baseType); }

        /// <summary>
        ///     Registers the preferred type as the type to instantiate when the base type is requested.
        /// </summary>
        /// <param name="baseType">The type that will be requested.</param>
        /// <param name="preferredType">
        ///     The type that should be created when <see cref="baseType" /> is
        ///     requested.
        /// </param>
        public static void Register([NotNull] Type baseType, [NotNull] Type preferredType)
        {
            //- Validate
            if (baseType == null) { throw new ArgumentNullException(nameof(baseType)); }
            if (preferredType == null) { throw new ArgumentNullException(nameof(preferredType)); }

            // TODO: Register
        }

        /// <summary>
        ///     Creates an instance of the requested type.
        /// </summary>
        /// <typeparam name="TRequested">The type that was requested to be created.</typeparam>
        /// <returns>An instance of the requested type</returns>
        /// <exception cref="TypeInitializationException">The type could not be initialized.</exception>
        [NotNull]
        public static TRequested Create<TRequested>()
        {
            // Determine which type to create
            var instantiateType = typeof(TRequested);

            // Create and return an instance of the requested type
            var instance = CreateInstance(instantiateType);

            return (TRequested)instance;
        }

        /// <summary>
        ///     Creates an instance of the requested type.
        /// </summary>
        /// <param name="instantiateType">Type to instantiate.</param>
        /// <returns>A new instance of the requested type.</returns>
        /// <exception cref="TypeInitializationException">The type could not be initialized.</exception>
        [NotNull]
        private static object CreateInstance([NotNull] Type instantiateType)
        {
            // Use the activator to create and return an instance of the requested type
            var instance = Activator.CreateInstance(instantiateType);
            if (instance != null) { return instance; }

            // Shouldn't be null here (should error out), but just in case throw the exception
            throw new TypeInitializationException(instantiateType.FullName, null);
        }
    }
}