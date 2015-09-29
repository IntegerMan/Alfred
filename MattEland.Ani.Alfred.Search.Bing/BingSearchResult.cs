using MattEland.Ani.Alfred.Core.Definitions;
using System;
using JetBrains.Annotations;
using MattEland.Common.Providers;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Search.Bing
{
    /// <summary>
    /// A Bing search result
    /// </summary>
    internal class BingSearchResult : SearchResult, IHasContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BingSearchResult" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="result">The Bing search result.</param>
        public BingSearchResult([NotNull] IObjectContainer container, [NotNull] WebResult result) : base(result.Title)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (result == null) throw new ArgumentNullException(nameof(result));

            // Set Basic Properties
            Description = result.Description;
            LocationText = result.DisplayUrl;
            Url = result.Url;
            Container = container;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult([NotNull] IObjectContainer container, [NotNull] NewsResult result) : base(result.Title)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (result == null) throw new ArgumentNullException(nameof(result));

            // Set Basic Properties
            Description = result.Description;
            LocationText = result.Source; // TODO: Maybe URL?
            Url = result.Url;
            Container = container;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult([NotNull] IObjectContainer container, [NotNull] ImageResult result) : base(result.Title)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (result == null) throw new ArgumentNullException(nameof(result));

            // Set Basic Properties
            Description = result.ContentType;
            LocationText = result.DisplayUrl;
            Url = result.SourceUrl;
            Container = container;
        }


        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; }

        /// <summary>
        /// Builds the more details action.
        /// </summary>
        /// <returns>An action</returns>
        [CanBeNull]
        private Action<ISearchResult> BuildMoreDetailsAction()
        {
            var shell = Container.TryProvide<IShellCommandRecipient>();

            if (shell == null || Url.IsEmpty()) return null;

            // Build out a web request command
            return (r) =>
            {
                var command = new ShellCommand("OpenWebPage", "Browser", Url);

                shell.ProcessShellCommand(command);
            };
        }

        /// <summary>
        /// The action to take on requesting more details
        /// </summary>
        private Action<ISearchResult> _action = null;

        /// <summary>
        ///     Gets the action that is executed when a user wants more information.
        /// </summary>
        /// <value>
        ///     The more details action.
        /// </value>
        public override Action<ISearchResult> MoreDetailsAction
        {
            get
            {
                // Lazy load the action
                if (_action == null)
                {
                    _action = BuildMoreDetailsAction();
                }

                return _action;
            }
        }

        /// <summary>
        ///     Gets the more details text.
        /// </summary>
        /// <value>
        ///     The more details text.
        /// </value>
        public override string MoreDetailsText
        {
            get
            {
                return Url;
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public IObjectContainer Container
        {
            get;
        }
    }

}