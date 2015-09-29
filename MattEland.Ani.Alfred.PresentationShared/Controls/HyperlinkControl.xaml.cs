using MattEland.Ani.Alfred.Core.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MattEland.Ani.Alfred.PresentationAvalon.Controls
{
    /// <summary>
    /// Interaction logic for HyperlinkControl.xaml
    /// </summary>
    public partial class HyperlinkControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HyperlinkControl"/> class.
        /// </summary>
        public HyperlinkControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the <see cref="LinkWidget"/> associated with this control.
        /// </summary>
        /// <value>The widget.</value>
        public LinkWidget Widget
        {
            get { return (LinkWidget)GetValue(WidgetProperty); }
            set { SetValue(WidgetProperty, value); }
        }

        /// <summary>
        /// The widget property definition.
        /// </summary>
        public static readonly DependencyProperty WidgetProperty =
            DependencyProperty.Register("Widget",
                typeof(LinkWidget),
                typeof(HyperlinkControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// The text property definition.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                typeof(string),
                typeof(HyperlinkControl),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        /// <summary>
        /// The URL property definition.
        /// </summary>
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url",
                typeof(string),
                typeof(HyperlinkControl),
                new PropertyMetadata(string.Empty));

    }
}
