// ---------------------------------------------------------
// AlfredModule.cs
// 
// Created on:      08/11/2015 at 9:44 PM
// Last Modified:   08/12/2015 at 3:43 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Modules
{

    /// <summary>
    ///     Represents a module within Alfred. Modules contain different bits of information to present to the user.
    /// </summary>
    public abstract class AlfredModule : AlfredComponent, IAlfredModule
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<AlfredWidget> _widgets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredModule" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected AlfredModule([NotNull] IPlatformProvider platformProvider)
        {
            if (platformProvider == null)
            {
                throw new ArgumentNullException(nameof(platformProvider));
            }

            _widgets = platformProvider.CreateCollection<AlfredWidget>();
        }

        /// <summary>
        ///     Gets whether or not the module is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the module is visible.</value>
        public override bool IsVisible
        {
            get { return _widgets.Any(w => w.IsVisible); }
        }

        /// <summary>
        ///     Gets the children of this component. Depending on the type of component this is, the children will
        ///     vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { yield break; }
        }

        /// <summary>
        ///     Gets the user interface widgets for the module.
        /// </summary>
        /// <value>The user interface widgets.</value>
        [NotNull]
        public IEnumerable<AlfredWidget> Widgets
        {
            get { return _widgets; }
        }

        /// <summary>
        ///     Clears all child collections
        /// </summary>
        protected override void ClearChildCollections()
        {
            base.ClearChildCollections();

            _widgets.Clear();
            OnPropertyChanged(nameof(Widgets));
        }

        /// <summary>
        ///     Registers a widget for the module.
        /// </summary>
        /// <param name="widget">
        ///     The widget.
        /// </param>
        protected void Register([NotNull] AlfredWidget widget)
        {
            _widgets.AddSafe(widget);

            OnPropertyChanged(nameof(IsVisible));
            OnPropertyChanged(nameof(Widgets));
        }

        /// <summary>
        ///     Registers multiple widgets at once.
        /// </summary>
        /// <param name="widgets">
        ///     The widgets.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="widgets"/> is <see langword="null" />.</exception>
        protected void Register([NotNull] IEnumerable<AlfredWidget> widgets)
        {
            if (widgets == null)
            {
                throw new ArgumentNullException(nameof(widgets));
            }

            foreach (var widget in widgets)
            {
                // ReSharper disable once AssignNullToNotNullAttribute - for testing purposes we'll allow this
                Register(widget);
            }
        }

        /// <summary>
        /// Processes an Alfred Command. If the command is handled, result should be modified accordingly and the method should return true. Returning false will not stop the message from being propogated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The result. If the command was handled, this should be updated.</param>
        /// <returns><c>True</c> if the command was handled; otherwise false.</returns>
        public virtual bool ProcessAlfredCommand(ChatCommand command, AlfredCommandResult result)
        {
            return false;
        }

        /// <summary>
        /// Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        /// Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public override string ItemTypeName
        {
            get { return "Module"; }
        }

        /// <summary>
        /// Gets the property providers.
        /// </summary>
        /// <value>The property providers.</value>
        [NotNull, ItemNotNull]
        public override IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { return Widgets; }
        }

    }
}