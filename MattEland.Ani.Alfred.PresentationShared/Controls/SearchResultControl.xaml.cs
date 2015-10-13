using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using System;
using System.Windows;
using System.Windows.Controls;

using MattEland.Common.Definitions.Search;

namespace MattEland.Ani.Alfred.PresentationAvalon.Controls
{
    /// <summary>
    /// Interaction logic for SearchResultControl.xaml
    /// </summary>
    public partial class SearchResultControl : UserControl, IHasContainer<IAlfredContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultControl"/> class.
        /// </summary>
        public SearchResultControl() : this(AlfredContainerHelper.ProvideContainer(), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultControl"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="result">The result.</param>
        public SearchResultControl(IAlfredContainer container, ISearchResult result)
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
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public IAlfredContainer Container { get; }

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
                new PropertyMetadata(null));

    }
}
