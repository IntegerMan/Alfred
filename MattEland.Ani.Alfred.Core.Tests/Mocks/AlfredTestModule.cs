using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Tests.Mocks
{
    /// <summary>
    /// An AlfredModule used explicitly for testing purposes
    /// </summary>
    internal sealed class AlfredTestModule : AlfredModule
    {
        [NotNull]
        private readonly ICollection<AlfredWidget> _widgetsToAddOnInit = new List<AlfredWidget>();

        [NotNull]
        private readonly ICollection<AlfredWidget> _widgetsToAddOnShutdown = new List<AlfredWidget>();

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredTestModule" />
        ///     class.
        /// </summary>
        internal AlfredTestModule() : this(new SimplePlatformProvider())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredTestModule" />
        ///     class.
        /// </summary>
        /// <param
        ///     name="platformProvider">
        ///     The collection provider.
        /// </param>
        /// <exception
        ///     cref="ArgumentNullException">
        /// </exception>
        private AlfredTestModule([NotNull] IPlatformProvider platformProvider) : base(platformProvider)
        {
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
        internal ICollection<AlfredWidget> WidgetsToRegisterOnInitialize { get { return _widgetsToAddOnInit; } }

        [NotNull]
        internal ICollection<AlfredWidget> WidgetsToRegisterOnShutdown { get { return _widgetsToAddOnShutdown; } }

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