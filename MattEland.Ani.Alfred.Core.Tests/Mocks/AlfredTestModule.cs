using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Tests.Mocks
{
    /// <summary>
    ///     An <see cref="AlfredModule"/> used explicitly for testing purposes
    /// </summary>
    [Obsolete("Use Moq instead")]
    internal sealed class AlfredTestModule : AlfredModule
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="AlfredTestModule" />
        ///     class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        internal AlfredTestModule([NotNull] IObjectContainer container) : base(container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            WidgetsToRegisterOnShutdown = new List<IWidget>();
            WidgetsToRegisterOnInitialize = new List<IWidget>();
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public override string Name { get { return "Test"; } }

        /// <summary>
        /// Gets the widgets to register on initialize.
        /// </summary>
        /// <value>The widgets to register on initialize.</value>
        [NotNull]
        internal ICollection<IWidget> WidgetsToRegisterOnInitialize { get; }

        /// <summary>
        /// Gets the widgets to register on shutdown.
        /// </summary>
        /// <value>The widgets to register on shutdown.</value>
        [NotNull]
        internal ICollection<IWidget> WidgetsToRegisterOnShutdown { get; }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            Register(WidgetsToRegisterOnInitialize);
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            Register(WidgetsToRegisterOnShutdown);
        }
    }
}