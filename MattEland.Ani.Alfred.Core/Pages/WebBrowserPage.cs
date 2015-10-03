using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;

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
        public WebBrowserPage([NotNull] IAlfredContainer container)
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

        /// <summary>
        /// Processes an Alfred Command. If the <paramref name="command"/> is handled,
        /// <paramref name="result"/> should be modified accordingly and the method should
        /// return true. Returning <see langword="false"/> will not stop the message from being
        /// propagated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">
        /// The result. If the <paramref name="command"/> was handled, this should be updated.
        /// </param>
        /// <returns>
        /// <c>True</c> if the <paramref name="command"/> was handled; otherwise false.
        /// </returns>
        public override bool ProcessAlfredCommand(ChatCommand command, ICommandResult result)
        {
            if (base.ProcessAlfredCommand(command, result)) return true;

            // Ensure the command is one we can handle
            if (command.Name.Matches("Browse") && command.Data.HasText())
            {
                // Browse
                Browser.Url = new Uri(command.Data.Trim());

                // Navigate to the web browser as needed
                var shell = AlfredInstance?.ShellCommandHandler;
                if (shell != null)
                {
                    var shellCommand = new ShellCommand("Navigate", "Browser", command.Data);
                    shell.ProcessShellCommand(shellCommand);
                }

                return true;
            }

            return false;
        }
    }
}
