using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A simple module that only takes in a name. This class cannot be inherited.
    /// </summary>
    public sealed class SimpleModule : AlfredModule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredModule" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="name"> The name of the module. </param>
        public SimpleModule([NotNull] IAlfredContainer container, [NotNull] string name) : base(container)
        {
            if (name.IsEmpty()) { throw new ArgumentNullException(nameof(name)); }

            Name = name;

            // Set up collections
            WidgetsToRegisterOnInitialize = new List<IWidget>();
            WidgetsToRegisterOnShutdown = new List<IWidget>();
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public override string Name { get; }

        /// <summary>
        /// Gets the widgets to register on initialize.
        /// </summary>
        /// <value>The widgets to register on initialize.</value>
        [NotNull]
        public ICollection<IWidget> WidgetsToRegisterOnInitialize { get; }

        /// <summary>
        /// Gets the widgets to register on shutdown.
        /// </summary>
        /// <value>The widgets to register on shutdown.</value>
        [NotNull]
        public ICollection<IWidget> WidgetsToRegisterOnShutdown { get; }

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