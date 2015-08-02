// ---------------------------------------------------------
// AlfredCoreModule.cs
// 
// Created on:      08/02/2015 at 4:56 PM
// Last Modified:   08/02/2015 at 5:06 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    /// A module intended for the control and monitoring of Alfred
    /// </summary>
    public sealed class AlfredCoreModule : AlfredModule
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredCoreModule" />
        ///     class.
        /// </summary>
        /// <exception
        ///     cref="ArgumentNullException">
        /// </exception>
        /// <param
        ///     name="collectionProvider">
        ///     The collection provider.
        /// </param>
        public AlfredCoreModule([NotNull] ICollectionProvider collectionProvider) : base(collectionProvider)
        {
        }

        /// <summary>
        ///     Gets the name and version of the Module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public override string NameAndVersion
        {
            get { return "Alfred Core 1.0"; }
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        protected override void InitializeProtected()
        {
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        protected override void UpdateProtected()
        {
        }
    }
}