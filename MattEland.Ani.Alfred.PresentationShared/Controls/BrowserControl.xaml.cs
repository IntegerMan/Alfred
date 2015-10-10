using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.Core.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MattEland.Ani.Alfred.PresentationAvalon.Controls
{
    /// <summary>
    /// Interaction logic for BrowserControl.xaml
    /// </summary>
    public partial class BrowserControl : UserControl
    {
        /// <summary>
        ///     Initializes a new instance of the BrowserControl class.
        /// </summary>
        public BrowserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the widget.
        /// </summary>
        /// <value>
        ///     The widget.
        /// </value>
        public WebBrowserWidget Widget
        {
            get { return (WebBrowserWidget)GetValue(WidgetProperty); }
            set { SetValue(WidgetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Widget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetProperty =
            DependencyProperty.Register("Widget", typeof(WebBrowserWidget), typeof(BrowserControl), new PropertyMetadata(null, OnWidgetChanged));

        /// <summary>
        ///     Updates the browser's source based on the Widget's Url.
        /// </summary>
        private void UpdateBrowser()
        {
            var widget = Widget;
            if (widget != null)
            {
                browser.Navigate(widget.Url);
            }
            else
            {
                browser.Source = null;
            }
        }
        /// <summary>
        ///     Handles the widget's navigation requested event.
        /// </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        private void OnNavigationRequested(object sender, EventArgs e)
        {
            UpdateBrowser();
        }

        /// <summary>
        ///     Updates the widget event bindings after the current widget changes.
        /// </summary>
        /// <param name="oldWidget"> The old widget. </param>
        /// <param name="newWidget"> The new widget. </param>
        private void UpdateWidgetBindings([CanBeNull] WebBrowserWidget oldWidget,
            [CanBeNull] WebBrowserWidget newWidget)
        {
            // Unsubscribe from old notifications
            if (oldWidget != null)
            {
                oldWidget.NavigationRequested -= OnNavigationRequested;
            }

            // Subscribe to new notifications
            if (newWidget != null)
            {
                newWidget.NavigationRequested += OnNavigationRequested;
            }

            // Refresh the browser
            UpdateBrowser();
        }

        /// <summary>
        ///     Handles when a BrowserControl's Widget changes.
        /// </summary>
        /// <param name="d"> The DependencyObject to process. </param>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        private static void OnWidgetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var Control = (BrowserControl)d;

            Control.UpdateWidgetBindings(e.OldValue as WebBrowserWidget, e.NewValue as WebBrowserWidget);
        }
    }
}
