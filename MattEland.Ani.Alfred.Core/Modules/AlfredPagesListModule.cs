// ---------------------------------------------------------
// AlfredPagesListModule.cs
// 
// Created on:      08/08/2015 at 7:38 PM
// Last Modified:   08/09/2015 at 4:50 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A module that lists installed subsystems
    /// </summary>
    public class AlfredPagesListModule : AlfredModule
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<AlfredWidget> _widgets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredPagesListModule" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredPagesListModule([NotNull] IPlatformProvider platformProvider) : base(platformProvider)
        {
            _widgets = platformProvider.CreateCollection<AlfredWidget>();
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
            foreach (var widget in _widgets)
            {
                var textWidget = widget as AlfredTextWidget;

                if (textWidget == null)
                {
                    continue;
                }

                // Interpret the DataContext and update its text if it's a page.
                // If no page context, it's assumed to be the no items label.
                var page = widget.DataContext as AlfredPage;
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
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(AlfredProvider alfred)
        {
            _widgets.Clear();

            // Read the pages from Alfred
            if (AlfredInstance != null)
            {
                foreach (var page in AlfredInstance.RootPages)
                {
                    var widget = new TextWidget { DataContext = page };
                    UpdateWidgetText(widget, page);

                    _widgets.Add(widget);

                    Register(widget);
                }
            }

            // We'll want to display a fallback for no pages
            if (_widgets.Count == 0)
            {
                var noItemsDetected = Resources.NoPagesDetected.NonNull();

                Log("Pages.Initialize", noItemsDetected, LogLevel.Warning);

                var widget = new TextWidget(noItemsDetected);
                _widgets.Add(widget);

                Register(widget);
            }
        }

        /// <summary>
        ///     Updates the widget's text to that matching the detected page.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="page">The page.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        private static void UpdateWidgetText([NotNull] AlfredTextWidget widget, [NotNull] AlfredPage page)
        {
            if (widget == null)
            {
                throw new ArgumentNullException(nameof(widget));
            }
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

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