using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Tests
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
        internal AlfredTestModule() : this(new SimpleCollectionProvider())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredTestModule" />
        ///     class.
        /// </summary>
        /// <param
        ///     name="collectionProvider">
        ///     The collection provider.
        /// </param>
        /// <exception
        ///     cref="ArgumentNullException">
        /// </exception>
        private AlfredTestModule([NotNull] ICollectionProvider collectionProvider) : base(collectionProvider)
        {
        }

        /// <summary>
        ///     Gets the name and version of the Module.
        /// </summary>
        /// <value>The name and version.</value>
        public override string NameAndVersion { get { return "TestModule"; } }

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
        protected override void InitializeProtected()
        {
            RegisterWidgets(WidgetsToRegisterOnInitialize);
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            RegisterWidgets(WidgetsToRegisterOnShutdown);
        }
    }
}