﻿// ---------------------------------------------------------
// CommonProvider.cs
// 
// Created on:      08/27/2015 at 2:55 PM
// Last Modified:   08/27/2015 at 4:08 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
        ///     Gets the type mappings dictionary used to instantiate instances of requested types. The keys
        ///     are the class that will be requested and the values are the types that should be instantiated
        ///     in that case.
        /// </summary>
        /// <value>The type mappings.</value>
        [NotNull]
        [ItemNotNull]
        private static IDictionary<Type, Type> TypeMappings { get; } = new Dictionary<Type, Type>();

        /// <summary>
        ///     Gets the function mappings dictionary used to instantiate instances of requested types. The keys
        ///     are the class that will be requested and the values are activator functions that will invoke and return the desired class
        ///     in that case.
        /// </summary>
        /// <value>The function mappings.</value>
        [NotNull, ItemNotNull]
        private static IDictionary<Type, Func<object>> FunctionMappings { get; } = new Dictionary<Type, Func<object>>();

        /// <summary>
        ///     Registers the preferred type as the type to instantiate when the base type is requested.
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="baseType" /> is <see langword="null" />.
        /// </exception>
        public static void Register([NotNull] Type baseType)
        {
            Register(baseType, baseType);
        }

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
        public static void Register([NotNull] Type baseType, [NotNull] Type preferredType)
        {
            //- Validate
            if (baseType == null) { throw new ArgumentNullException(nameof(baseType)); }
            if (preferredType == null) { throw new ArgumentNullException(nameof(preferredType)); }

            /* Ensure that preferredType is something we can create. 
               This will throw an InvalidOperationException if the class is something 
               that cannot be created */

            ValidateInstantiationType(preferredType);

            // Register the type mapping
            TypeMappings.Add(baseType, preferredType);
        }

        /// <summary>
        ///     Validates that <see cref="preferredType" /> is something that can be created.
        /// </summary>
        /// <param name="preferredType">Type of the preferred.</param>
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private static void ValidateInstantiationType([NotNull] Type preferredType)
        {
            if (preferredType.IsAbstract)
            {
                throw new InvalidOperationException("Cannot create an abstract type");
            }
        }

        /// <summary>
        ///     Creates an instance of the requested type.
        /// </summary>
        /// <typeparam name="TRequested">The type that was requested to be created.</typeparam>
        /// <returns>An instance of the requested type</returns>
        /// <exception cref="InvalidOperationException">
        ///     The type is not correctly configured to allow for
        ///     instantiation.
        /// </exception>
        [NotNull]
        public static TRequested Create<TRequested>()
        {
            var requestedType = typeof(TRequested);

            // Determine which type to create
            var activator = GetTypeActivatorFunction(requestedType);

            try
            {
                /* Create and return an instance of the requested type using the type 
                   determined earlier. This can throw many exceptions which will be
                   wrapped into more user-friendly exceptions with easier error handling. */

                // ReSharper disable once EventExceptionNotDocumented
                var instance = activator.Invoke();

                if (instance == null)
                {
                    throw new InvalidOperationException("The activator function for creating " + requestedType.FullName + " returned a null value.");
                }

                return (TRequested)instance;
            }
            catch (MissingMemberException ex)
            {
                // Improve the thrown exception with more information.
                string message =
                    $"Could not instantiate {requestedType.FullName} due to missing member exception: '{ex.Message}'";

                throw new InvalidOperationException(message, ex);
            }
        }

        /// <summary>
        ///     Calculates the type to instantiate given the type requested.
        ///     This is influenced by the Register methods.
        /// </summary>
        /// <param name="requestedType">Type that was requested.</param>
        /// <returns>An activator function to instantiate the requested type.</returns>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private static Func<object> GetTypeActivatorFunction([NotNull] Type requestedType)
        {
            if (TypeMappings.ContainsKey(requestedType))
            {
                return () => CreateInstance(TypeMappings[requestedType]);
            }

            if (FunctionMappings.ContainsKey(requestedType))
            {
                return FunctionMappings[requestedType];
            }

            return () => CreateInstance(requestedType);
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

        /// <summary>
        /// Registers an activator function responsible for instantiating the desired type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="activator">The activator.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="type" /> or <paramref name="activator" /> is <see langword="null" />.</exception>
        public static void Register([NotNull] Type type, [NotNull] Func<object> activator)
        {
            //- Validate
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (activator == null) { throw new ArgumentNullException(nameof(activator)); }

            FunctionMappings.Add(type, activator);
        }

        /// <summary>
        /// Clears all mappings for creating types.
        /// </summary>
        /// <remarks>
        /// This is useful for unit testing for cleaning up before invoking each time
        /// </remarks>
        public static void ClearMappings()
        {
            FunctionMappings.Clear();
            TypeMappings.Clear();
        }
    }
}