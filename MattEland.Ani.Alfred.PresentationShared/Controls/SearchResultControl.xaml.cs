using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common.Providers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MattEland.Ani.Alfred.PresentationAvalon.Controls
{
    /// <summary>
    /// Interaction logic for SearchResultControl.xaml
    /// </summary>
    public partial class SearchResultControl : UserControl, IHasContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultControl"/> class.
        /// </summary>
        public SearchResultControl() : this(CommonProvider.Container, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultControl"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="result">The result.</param>
        public SearchResultControl(IObjectContainer container, ISearchResult result)
        {
            // Validate
            if (container == null) throw new ArgumentNullException(nameof(container));

            // Set Properties
            Container = container;
            Result = result;

            // Initialize
            InitializeComponent();
        }

        /// <summary>
        /// Builds the link widget.
        /// </summary>
        private void BuildLinkWidget()
        {
            if (Result != null && Result.MoreDetailsAction != null)
            {
                string controlName = "linkResult" + Result.GetHashCode();
                var parameters = new WidgetCreationParameters(controlName, Container);

                LinkWidget = new LinkWidget(Result.MoreDetailsText, parameters);

                // TODO: add in a command
            }
            else
            {
                LinkWidget = null;
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public IObjectContainer Container { get; }

        /// <summary>
        /// Gets or sets the search result.
        /// </summary>
        /// <value>The result.</value>
        public ISearchResult Result
        {
            get { return (ISearchResult)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        /// <summary>
        /// The result property definition.
        /// </summary>
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result",
                typeof(ISearchResult),
                typeof(SearchResultControl),
                new PropertyMetadata(null, OnResultChanged));

        /// <summary>
        /// Gets or sets the link widget.
        /// </summary>
        /// <value>The link widget.</value>
        public LinkWidget LinkWidget
        {
            get { return (LinkWidget)GetValue(LinkWidgetProperty); }
            private set { SetValue(LinkWidgetProperty, value); }
        }

        /// <summary>
        /// Responds to the <see cref="Result"/> changed event by rebuilding the link.
        /// </summary>
        /// <param name="obj">The control.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnResultChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = (SearchResultControl)obj;
            control.BuildLinkWidget();
        }

        /// <summary>
        /// The link widget property definition
        /// </summary>
        public static readonly DependencyProperty LinkWidgetProperty =
            DependencyProperty.Register("LinkWidget",
                typeof(LinkWidget),
                typeof(SearchResultControl),
                new PropertyMetadata(null));

    }
}
