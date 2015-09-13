// ---------------------------------------------------------
// Page.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/11/2015 at 9:35 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     A page that can be used in Alfred
    /// </summary>
    public abstract class Page : ComponentBase, IPage
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="Page" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="name">The name.</param>
        /// <param name="id">The ID</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="container" /> is <see langword="null" /> .
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> .
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="id" /> is <see langword="null" /> .
        /// </exception>
        protected Page(
            [NotNull] IObjectContainer container,
            [NotNull] string name,
            [NotNull] string id) : base(container)
        {
            //- Validation
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            if (id == null) { throw new ArgumentNullException(nameof(id)); }

            Name = name;
            Id = id;

            IsRootLevel = true;
        }

        /// <summary>
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>
        /// Whether or not the component is visible.
        /// </value>
        public override bool IsVisible
        {
            get { return Status == AlfredStatus.Online; }
        }

        /// <summary>
        ///     Gets or sets the name of the page.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [NotNull]
        public override string Name { get; }

        /// <summary>
        ///     Gets the page identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; }

        /// <summary>
        ///     Gets a value indicating whether this is a root level page that should show on the
        ///     navigator.
        /// </summary>
        /// <value>
        /// <c>true</c> if this page is root level; otherwise, <c>false</c> .
        /// </value>
        public bool IsRootLevel { get; set; }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <value>
        /// The item type's name.
        /// </value>
        /// <example>
        ///     Some examples of <see cref="ItemTypeName"/> values might be "Folder", "Application",
        ///     "User", etc.
        /// </example>
        public override string ItemTypeName
        {
            get { return "Page"; }
        }

        /// <summary>
        ///     Type of the layout to use for this page.
        /// </summary>
        private LayoutType _layoutType = LayoutType.VerticalStackPanel;

        /// <summary>
        ///     Gets or sets the type of the layout used for this page.
        /// </summary>
        /// <value>
        ///     The type of the layout.
        /// </value>
        public LayoutType LayoutType
        {
            get { return _layoutType; }
            set
            {
                if (value == _layoutType) return;
                _layoutType = value;
                OnPropertyChanged(nameof(LayoutType));
            }
        }

        /// <summary>
        ///     Processes an Alfred Command. If the <paramref name="command"/> is handled,
        ///     <paramref name="result"/> should be modified accordingly and the method should
        ///     return true. Returning <see langword="false"/> will not stop the message from being
        ///     propagated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">
        /// The result. If the <paramref name="command"/> was handled, this should be updated.
        /// </param>
        /// <returns>
        ///     <c>True</c> if the <paramref name="command"/> was handled; otherwise false.
        /// </returns>
        public virtual bool ProcessAlfredCommand(ChatCommand command, ICommandResult result)
        {
            return false;
        }
    }

}