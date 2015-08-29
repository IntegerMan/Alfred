// ---------------------------------------------------------
// AlfredSubSystemListModule.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/25/2015 at 5:41 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A module that lists installed subsystems
    /// </summary>
    public sealed class AlfredSubsystemListModule : AlfredModule
    {

        [NotNull]
        [ItemNotNull]
        private readonly ICollection<WidgetBase> _widgets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystemListModule" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        internal AlfredSubsystemListModule([NotNull] IObjectContainer container)
            : base(container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            _widgets = container.ProvideCollection<WidgetBase>();
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
        ///     Updates the component
        /// </summary>
        protected override void UpdateProtected()
        {
            foreach (var widget in _widgets)
            {
                var textWidget = widget as AlfredTextWidget;

                if (textWidget == null) { continue; }

                // Interpret the DataContext and update its text if it's a component based on the
                // component status. If no component context, it's assumed to be the no items label.
                var component = widget.DataContext as IAlfredComponent;
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
        /// <param name="alfred"></param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            _widgets.Clear();

            // Read the subsystems from Alfred
            if (AlfredInstance != null)
            {
                foreach (var item in AlfredInstance.Subsystems)
                {
                    var widget = BuildSubsystemWidget(item);

                    _widgets.Add(widget);

                    Register(widget);
                }
            }

            // We'll want to display a fallback for no subsystems
            if (_widgets.Count == 0)
            {
                var noSubsystemsDetected =
                    Resources.AlfredSubSystemListModule_NoSubsystemsDetected.NonNull();

                Log("Subsystems.Initialize", noSubsystemsDetected, LogLevel.Warning);

                var widget = new TextWidget(noSubsystemsDetected,
                                            BuildWidgetParameters(@"lblNoSubsystems"));
                _widgets.Add(widget);

                Register(widget);
            }
        }

        /// <summary>
        ///     Builds a widget for the subsystem.
        /// </summary>
        /// <param name="subsystem">The item.</param>
        /// <returns>TextWidget.</returns>
        [NotNull]
        private TextWidget BuildSubsystemWidget([NotNull] IAlfredSubsystem subsystem)
        {
            var id = string.Format(Locale, @"lblSubsystem{0}", subsystem.Id);
            var widget = new TextWidget(BuildWidgetParameters(id)) { DataContext = subsystem };
            UpdateWidgetText(widget, subsystem);

            return widget;
        }

        /// <summary>
        ///     Updates the widget's text to that matching the detected component.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="component">The component.</param>
        /// <exception cref="ArgumentNullException" />
        private static void UpdateWidgetText(
            [NotNull] AlfredTextWidget widget,
            [NotNull] IAlfredComponent component)
        {
            if (widget == null) { throw new ArgumentNullException(nameof(widget)); }
            if (component == null) { throw new ArgumentNullException(nameof(component)); }

            widget.Text = string.Format(CultureInfo.CurrentCulture,
                                        @"{0}: {1}",
                                        component.NameAndVersion,
                                        component.Status);
        }
    }

}