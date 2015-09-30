using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    /// Represents a hyperlink control in the user interface
    /// </summary>
    public sealed class LinkWidget : TextWidgetBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkWidget" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public LinkWidget(WidgetCreationParameters parameters) : base(parameters)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkWidget" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        public LinkWidget(string text, WidgetCreationParameters parameters) : base(text, parameters)
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LinkWidget" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="url">The URL.</param>
        /// <param name="parameters">The parameters.</param>
        public LinkWidget(string text, string url, WidgetCreationParameters parameters) : base(text, parameters)
        {
            _url = url;
        }

        /// <summary>
        /// The command that is executed when the link is clicked.
        /// </summary>
        [CanBeNull]
        public IAlfredCommand Command
        {
            get { return _command; }
            set
            {
                if (_command != value)
                {
                    _command = value;

                    OnPropertyChanged(nameof(Command));
                }
            }
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
            get
            {
                return "Hyperlink";
            }
        }

        /// <summary>
        /// The command
        /// </summary>
        [NotNull]
        private IAlfredCommand _command;

        /// <summary>
        /// The URL
        /// </summary>
        [CanBeNull]
        private string _url = string.Empty;

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [CanBeNull]
        public string Url
        {
            get { return _url; }
            set
            {
                if (_url != value)
                {
                    _url = value;

                    OnPropertyChanged(nameof(Url));
                }
            }
        }
    }
}
