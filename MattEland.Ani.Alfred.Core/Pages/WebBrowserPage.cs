using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     A web browser page. This class cannot be inherited.
    /// </summary>
    public sealed class WebBrowserPage : AlfredPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WebBrowserPage" />
        ///     class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public WebBrowserPage([NotNull] IObjectContainer container)
            : base(container, "Web Browser", "Browser")
        {
            _browserWidget = new WebBrowserWidget(new WidgetCreationParameters("browser", container));
        }

        /// <summary>
        ///     Gets or sets URL the browser is currently browsing.
        /// </summary>
        /// <value>
        ///     The URL.
        /// </value>
        [NotNull]
        public Uri Url
        {
            get
            {
                return _browserWidget.Url;
            }
            set
            {
                _browserWidget.Url = value;
            }
        }

        /// <summary>
        /// Gets the children of the component. Depending on the type of component this is, the children
        /// will vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get
            {
                yield break;
            }
        }

        /// <summary>
        ///     The web browser widget.
        /// </summary>
        [NotNull]
        private WebBrowserWidget _browserWidget;

        /// <summary>
        ///     Gets the browser widget.
        /// </summary>
        /// <value>
        ///     The browser.
        /// </value>
        [NotNull]
        public WebBrowserWidget Browser
        {
            get
            {
                return _browserWidget;
            }
        }
    }
}
