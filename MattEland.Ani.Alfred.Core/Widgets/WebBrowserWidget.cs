using MattEland.Common.Annotations;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     A web browser widget. This class cannot be inherited.
    /// </summary>
    public sealed class WebBrowserWidget : WidgetBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowserWidget"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="parameters"> The creation parameters. </param>
        public WebBrowserWidget([NotNull] WidgetCreationParameters parameters) : base(parameters)
        {

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
                return "Web Browser";
            }
        }

        /// <summary>
        ///     The URL the browser is pointed at
        /// </summary>
        private Uri _url = new Uri("http://www.bing.com/");

        /// <summary>
        ///     An event that occurs when navigation is requested for a new page or a page refresh.
        /// </summary>
        public event EventHandler NavigationRequested;

        /// <summary>
        ///     Raises the <see cref="NavigationRequested"/> event.
        /// </summary>
        private void RaiseNavigationRequested()
        {
            NavigationRequested?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        ///     Gets URL the browser is pointed at.
        /// </summary>
        /// <value>
        ///     The URL.
        /// </value>
        public Uri Url
        {
            get
            {
                return _url;
            }
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged(nameof(Url));

                    RaiseNavigationRequested();
                }
            }
        }
    }
}
