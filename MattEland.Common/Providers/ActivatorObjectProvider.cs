// ---------------------------------------------------------
// ActivatorObjectProvider.cs
// 
// Created on:      08/27/2015 at 5:49 PM
// Last Modified:   08/27/2015 at 9:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Common.Providers
{
    /// <summary>
    ///     An <see cref="IObjectProvider" /> that provides the requested type via using the
    ///     <see cref="Activator" />. Instances can be configured to create types other than the requested
    ///     type via the <see cref="ActivatorObjectProvider(System.Type)" /> constructor.
    /// </summary>
    [PublicAPI]
    public class ActivatorObjectProvider : IObjectProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatorObjectProvider" /> class.
        ///     This constructor sets up the provider to always create the requested type.
        /// </summary>
        public ActivatorObjectProvider() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatorObjectProvider" /> class.
        ///     This constructor takes a <paramref name="typeToCreate" /> parameter that allows
        ///     this instance to create using types other than the requested type. If this is
        ///     null, the <see cref="Activator" /> will be invoked on the requested type.
        /// </summary>
        /// <param name="typeToCreate">The type to create when <see cref="CreateInstance" /> is called.</param>
        public ActivatorObjectProvider([CanBeNull] Type typeToCreate)
        {
            TypeToCreate = typeToCreate;
        }


        /// <summary>
        ///     Gets the type to create when <see cref="CreateInstance" /> is called.
        /// </summary>
        /// <value>The type to create.</value>
        private Type TypeToCreate { get; }

        /// <summary>
        ///     Creates an instance of the requested type.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     Thrown when the requested operation is not supported.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// <param name="requestedType"> The type that was requested. </param>
        /// <param name="args"> The arguments. </param>
        /// <returns>
        ///     A new instance of the requested type.
        /// </returns>
        public object CreateInstance(Type requestedType, [CanBeNull] params object[] args)
        {
            if (requestedType == null) { throw new ArgumentNullException(nameof(requestedType)); }

            /* If we were set up to create using a specific type, use that type instead of 
            the requested type. This allows us to set up this class to create instances of
            subclasses or those that implement interfaces*/

            var typeToCreate = TypeToCreate ?? requestedType;

            // Validate against interfaces
            if (typeToCreate.IsInterface)
            {
                throw new NotSupportedException($"Cannot create interface type: {typeToCreate.Name}");
            }

            // Validate against abstract classes
            if (typeToCreate.IsAbstract)
            {
                throw new NotSupportedException("Cannot create an abstract class");
            }

            var instance = CreateInstanceUsingActivator(typeToCreate, args);

            if (instance == null)
            {
                string message = $"Activator did not create instance for {typeToCreate.FullName} when {requestedType.FullName} was requested.";

                throw new InvalidOperationException(message);
            }

            return instance;
        }

        /// <summary>
        ///     Creates instance using activator.
        /// </summary>
        /// <param name="typeToCreate">
        ///     The type to create when <see cref="CreateInstance" /> is called.
        /// </param>
        /// <param name="args"> The arguments. </param>
        /// <returns>
        ///     A new instance of the requested type.
        /// </returns>
        [CanBeNull]
        protected virtual object CreateInstanceUsingActivator(Type typeToCreate, object[] args)
        {
            return Activator.CreateInstance(typeToCreate, args);
        }

    }

}