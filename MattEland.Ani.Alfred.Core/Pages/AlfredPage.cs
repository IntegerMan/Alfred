﻿// ---------------------------------------------------------
// AlfredPage.cs
// 
// Created on:      08/11/2015 at 9:44 PM
// Last Modified:   08/11/2015 at 9:45 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     A page that can be used in Alfred
    /// </summary>
    public abstract class AlfredPage : ComponentBase, IAlfredPage
    {
        [NotNull]
        private readonly string _name;

        private readonly string _id;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredPage" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The ID</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <see langword="null" />.</exception>
        protected AlfredPage([NotNull] string name, [NotNull] string id)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _name = name;
            _id = id;
        }

        /// <summary>
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the component is visible.</value>
        public override bool IsVisible
        {
            get { return Status == AlfredStatus.Online; }
        }

        /// <summary>
        ///     Gets or sets the name of the page.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public override string Name
        {
            [DebuggerStepThrough]
            get
            { return _name; }
        }

        /// <summary>
        /// Gets the page identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id
        {
            [DebuggerStepThrough]
            get
            { return _id; }
        }

        /// <summary>
        ///     Gets a value indicating whether this is a root level page that should show on the navigator.
        /// </summary>
        /// <value><c>true</c> if this page is root level; otherwise, <c>false</c>.</value>
        public bool IsRootLevel
        {
            get { return true; }
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
            get { return "Page"; }
        }

    }

}