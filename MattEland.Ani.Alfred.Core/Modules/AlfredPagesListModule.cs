// ---------------------------------------------------------
// AlfredPagesListModule.cs
// 
// Created on:      08/08/2015 at 7:38 PM
// Last Modified:   08/09/2015 at 4:50 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A module that lists installed subsystems
    /// </summary>
    public sealed class AlfredPagesListModule : AlfredModule
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<WidgetBase> _widgets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredPagesListModule" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        internal AlfredPagesListModule([NotNull] IObjectContainer container) : base(container)
        {
            _widgets = container.ProvideCollection<WidgetBase>();
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public override string Name
        {
            get { return "Pages"; }
        }

        /// <summary>
        ///     Updates the page
        /// </summary>
        protected override void UpdateProtected()
        {
            var textWidgets = _widgets.Cast<TextWidget>();
            foreach (var textWidget in textWidgets)
            {
                Debug.Assert(textWidget != null);

                // Interpret the DataContext and update its text if it's a page.
                // If no page context, it's assumed to be the no items label.
                var page = textWidget.DataContext as AlfredPage;

                if (page != null)
                {
                    UpdateWidgetText(textWidget, page);
                }
                else
                {
                    textWidget.Text = Resources.NoPagesDetected;
                }
            }
        }

        /// <summary>
        ///     Handles shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            _widgets.Clear();
        }

        /// <summary>
        ///     Gets whether or not the module is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the module is visible.</value>
        public override bool IsVisible
        {
            get
            {
                return AlfredInstance != null && (base.IsVisible && AlfredInstance.IsOnline);
            }
        }

        /// <summary>
        /// Allows components to define controls
        /// </summary>
        protected override void RegisterControls()
        {
            _widgets.Clear();

            // Read the pages from Alfred
            if (AlfredInstance != null)
            {
                foreach (var subsystem in AlfredInstance.Subsystems)
                {
                    foreach (var page in subsystem.Pages)
                    {
                        AddPageWidget(page);
                    }
                }
            }

            // We'll want to display a fallback for no pages
            if (_widgets.Count == 0)
            {
                AddNoItemsWidget();
            }

        }

        /// <summary>
        ///     Adds a widget for an <see cref="IAlfredPage"/>.
        /// </summary>
        /// <param name="page"> The page. </param>
        private void AddPageWidget([NotNull] IAlfredPage page)
        {
            var lblId = string.Format(Locale, @"lblPage{0}", page.Id);
            var widget = new TextWidget(BuildWidgetParameters(lblId)) { DataContext = page };

            UpdateWidgetText(widget, page);

            _widgets.Add(widget);

            Register(widget);
        }

        /// <summary>
        ///     Adds a no items detected <see cref="TextWidget"/>.
        /// </summary>
        private void AddNoItemsWidget()
        {
            var noItemsDetected = Resources.NoPagesDetected.NonNull();

            Log("Pages.Initialize", noItemsDetected, LogLevel.Warning);

            var widget = new TextWidget(noItemsDetected, BuildWidgetParameters(@"lblNoItems"));
            _widgets.Add(widget);

            Register(widget);
        }

        /// <summary>
        ///     Updates the widget's text to that matching the detected page.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="page">The page.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        private static void UpdateWidgetText([NotNull] AlfredTextWidget widget, [NotNull] IAlfredPage page)
        {
            widget.Text = page.Name;
        }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnInitializationCompleted()
        {
            // Re-initialize in case other pages popped up
            InitializeProtected(AlfredInstance);
        }
    }
}