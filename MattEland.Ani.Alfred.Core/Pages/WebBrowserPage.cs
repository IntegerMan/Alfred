using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

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
    }
}
