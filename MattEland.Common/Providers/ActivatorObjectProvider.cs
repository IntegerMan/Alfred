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
    ///     type via the <see cref="ActivatorObjectProvider(System.Type,object[])" /> constructor.
    /// </summary>
    [PublicAPI]
    public sealed class ActivatorObjectProvider : IObjectProvider
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
        /// <param name="arguments">Constructor arguments</param>
        public ActivatorObjectProvider([CanBeNull] Type typeToCreate, params object[] arguments)
        {
            TypeToCreate = typeToCreate;
            Arguments = arguments;
        }

        /// <summary>
        ///     Gets the arguments to pass in to the class constructor on instantiation.
        /// </summary>
        /// <value>The arguments.</value>
        public object[] Arguments { get; }

        /// <summary>
        ///     Gets the type to create when <see cref="CreateInstance" /> is called.
        /// </summary>
        /// <value>The type to create.</value>
        private Type TypeToCreate { get; }

        /// <summary>
        ///     Creates an instance of the requested type.
        /// </summary>
        /// <param name="requestedType">The type that was requested.</param>
        /// <returns>A new instance of the requested type</returns>
        public object CreateInstance(Type requestedType)
        {
            /* If we were set up to create using a specific type, use that type instead of 
            the requested type. This allows us to set up this class to create instances of
            subclasses or those that implement interfaces*/

            var typeToCreate = TypeToCreate ?? requestedType;

            return Activator.CreateInstance(typeToCreate, Arguments);
        }

    }
}