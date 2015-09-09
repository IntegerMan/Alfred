// ---------------------------------------------------------
// ActivatorHasContainerObjectProvider.cs
// 
// Created on:      09/09/2015 at 12:44 AM
// Last Modified:   09/09/2015 at 1:08 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace MattEland.Common.Providers
{
    /// <summary>
    ///     An <see cref="IObjectProvider" /> that functions similarly to the
    ///     <see cref="ActivatorObjectProvider" /> in creating objects as needed but will try the
    ///     constructor with a single <see cref="IObjectContainer" /> if it is present.
    /// </summary>
    public sealed class ActivatorHasContainerObjectProvider : ActivatorObjectProvider, IHasContainer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatorObjectProvider" /> class. This
        ///     constructor takes a <paramref name="typeToCreate" /> parameter that allows this instance
        ///     to create using types other than the requested type. If this is null, the
        ///     <see cref="Activator" /> will be invoked on the requested type.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="container" /> is null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="typeToCreate">
        ///     The type to create when <see cref="ActivatorObjectProvider.CreateInstance" /> is called.
        /// </param>
        public ActivatorHasContainerObjectProvider(
            [NotNull] IObjectContainer container,
            [CanBeNull] Type typeToCreate) : base(typeToCreate)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            Container = container;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatorObjectProvider" /> class. This
        ///     constructor sets up the provider to always create the requested type.
        /// </summary>
        /// <param name="container"> The container. </param>
        ///
        /// ### <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="container" /> is null.
        /// </exception>
        public ActivatorHasContainerObjectProvider([NotNull] IObjectContainer container)
            : this(container, null)
        {
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IObjectContainer Container { get; }

        /// <summary>
        ///     Creates instance using activator.
        /// </summary>
        /// <param name="typeToCreate">
        ///     The type to create when <see cref="ActivatorObjectProvider.CreateInstance" /> is called.
        /// </param>
        /// <param name="args"> The arguments. </param>
        /// <returns>
        ///     A new instance of the requested type.
        /// </returns>
        [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
        [SuppressMessage("ReSharper", "ThrowingSystemException")]
        [SuppressMessage("ReSharper", "CatchAllClause")]
        protected override object CreateInstanceUsingActivator(Type typeToCreate, object[] args)
        {
            // If it's not an IHasContainer, just use the standard way
            var interfaces = typeToCreate.GetInterfaces();
            if (!interfaces.Contains(typeof(IHasContainer)))
            {
                return base.CreateInstanceUsingActivator(typeToCreate, args);
            }

            try
            {
                // Add container to the front of args list unless an argument was a Container
                var argsList = args.ToList();
                if (!argsList.Any(a => a is IObjectContainer))
                {
                    argsList.Insert(0, Container);
                }

                // Attempt to create using the "new MyClass(Container)" constructor.
                return Activator.CreateInstance(typeToCreate, argsList.ToArray());
            }
            catch (Exception)
            {
                // We couldn't build it using the desired constructor. Try it the normal way.
                return base.CreateInstanceUsingActivator(typeToCreate, args);
            }
        }
    }
}