// ---------------------------------------------------------
// AlfredSubSystemListModule.cs
// 
// Created on:      08/07/2015 at 11:56 PM
// Last Modified:   08/07/2015 at 11:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    /// A module that lists installed subsystems
    /// </summary>
    public class AlfredSubSystemListModule : AlfredModule
    {

        [NotNull, ItemNotNull]
        private readonly ICollection<AlfredWidget> _widgets;

        [CanBeNull]
        private AlfredProvider _alfredProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredModule"/> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredSubSystemListModule([NotNull] IPlatformProvider platformProvider) : base(platformProvider)
        {
            _widgets = platformProvider.CreateCollection<AlfredWidget>();
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public override string Name
        {
            get { return "Subsystems"; }
        }

        /// <summary>
        ///     Initializes the module.
        /// </summary>
        /// <param name="alfred">
        ///     The provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Already online when told to initialize.
        /// </exception>
        public override void Initialize(AlfredProvider alfred)
        {
            _alfredProvider = alfred;

            base.Initialize(alfred);
        }

        /// <summary>
        ///     Updates the component
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

                // Interpret the DataContext and update its text if it's a component based on the
                // component status. If no component context, it's assumed to be the no items label.
                var component = widget.DataContext as AlfredComponent;
                if (component != null)
                {
                    UpdateWidgetText(textWidget, component);
                }
                else
                {
                    textWidget.Text = Resources.AlfredSubSystemListModule_NoSubsystemsDetected;
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
        protected override void InitializeProtected()
        {
            _widgets.Clear();

            // Read the subsystems from Alfred
            if (_alfredProvider != null)
            {
                foreach (var subSystem in _alfredProvider.SubSystems)
                {
                    var widget = new TextWidget { DataContext = subSystem };
                    UpdateWidgetText(widget, subSystem);

                    _widgets.Add(widget);

                    Register(widget);
                }
            }

            // We'll want to display a fallback for no subsystems
            if (_widgets.Count == 0)
            {
                var noSubsystemsDetected = Resources.AlfredSubSystemListModule_NoSubsystemsDetected.NonNull();

                Log("SubSystems.Initialize", noSubsystemsDetected, LogLevel.Warning);

                var widget = new TextWidget(noSubsystemsDetected);
                _widgets.Add(widget);

                Register(widget);
            }
        }

        /// <summary>
        /// Updates the widget's text to that matching the detected component.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="component">The component.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        private static void UpdateWidgetText([NotNull] AlfredTextWidget widget, [NotNull] AlfredComponent component)
        {
            if (widget == null)
            {
                throw new ArgumentNullException(nameof(widget));
            }
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            widget.Text = string.Format(CultureInfo.CurrentCulture, "{0}: {1}", component.NameAndVersion, component.Status);
        }
    }
}