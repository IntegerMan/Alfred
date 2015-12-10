// ---------------------------------------------------------
// ErrorCode.cs
// 
// Created on:      12/09/2015 at 9:36 PM
// Last Modified:   12/09/2015 at 9:36 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Common.Annotations;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Definitions
{

    /// <summary>
    ///     An error code and all associated instances of this error.
    /// </summary>
    public sealed class ErrorCode : IHasContainer<IAlfredContainer>
    {
        [NotNull, ItemNotNull]
        private readonly ICollection<ErrorInstance> _instances;

        /// <summary>
        ///     Initializes a new instance of the ErrorCode class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public ErrorCode([NotNull] IAlfredContainer container)
        {
            Contract.Requires(container != null);
            Contract.Ensures(Container != null);
            Contract.Ensures(Container == container);

            Container = container;

            _instances = container.ProvideCollection<ErrorInstance>();
        }

        /// <summary>
        ///     Gets a value indicating whether this instance has errors associated with it.
        /// </summary>
        /// <value>
        ///     true if this instance has errors, false if not.
        /// </value>
        public bool HasErrors { get { return _instances.Any(); } }

        /// <summary>
        ///     Gets a value indicating whether this instance has errors associated with it.
        /// </summary>
        /// <value>
        ///     true if this instance has errors, false if not.
        /// </value>
        public bool HasUnacknowledgedErrors { get { return _instances.Any(e => !e.IsAcknowledged); } }

        /// <summary>
        ///     Gets the last instance associated with this error code.
        /// </summary>
        /// <value>
        ///     The last instance.
        /// </value>
        [CanBeNull]
        public ErrorInstance LastInstance { get { return _instances.LastOrDefault(); } }

        /// <summary>
        ///     Gets or sets the error code identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public string Identifier { get; set; } = "UNKN";

        /// <summary>
        ///     Adds an error to the collection.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="instance"/> is <lang keyword="null" />.
        /// </exception>
        /// <param name="instance"> The instance. </param>
        public void AddError([NotNull] ErrorInstance instance)
        {
            Contract.Requires(instance != null);
            Contract.Ensures(Instances.Contains(instance));

            if (instance == null) throw new ArgumentNullException(nameof(instance));

            _instances.Add(instance);

            instance.ErrorCode = this;
        }

        /// <summary>
        ///     Gets the instances associated with this error code.
        /// </summary>
        /// <value>
        ///     The instances.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<ErrorInstance> Instances
        {
            get { return _instances; }
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IAlfredContainer Container { get; }

        /// <summary>
        ///     Determine if this error code should include the specified instance.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are <lang keyword="null" />.
        /// </exception>
        /// <param name="instance"> The instance. </param>
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        public bool ShouldInclude([NotNull] ErrorInstance instance)
        {

            //- Validate
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var ex = instance.Exception;
            var exType = ex.GetType();

            // Check for any mathing stack traces and exception types.
            return _instances.Any(i => i.Exception.StackTrace == ex.StackTrace
                                       && i.Exception.GetType() == exType);
        }
    }

}