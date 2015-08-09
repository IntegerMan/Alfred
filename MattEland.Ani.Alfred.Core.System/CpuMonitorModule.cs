// ---------------------------------------------------------
// CpuMonitorModule.cs
// 
// Created on:      08/03/2015 at 8:51 PM
// Last Modified:   08/04/2015 at 10:06 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{

    /// <summary>
    ///     A module that displays information on the system's processor utilization percentages
    /// </summary>
    public class CpuMonitorModule : SystemMonitorModule
    {
        private const string CpuCategoryName = "Processor";
        private const string CpuUsageCounterName = "% Processor Time";

        // ReSharper disable once AssignNullToNotNullAttribute
        [NotNull]
        private readonly string _cpuMonitorLabel = Resources.CpuMonitorModule_Cpu_Label_Format;

        [NotNull]
        [ItemNotNull]
        private readonly List<AlfredProgressBarWidget> _cpuWidgets = new List<AlfredProgressBarWidget>();

        [NotNull]
        [ItemNotNull]
        private readonly List<PerformanceCounter> _processorCounters = new List<PerformanceCounter>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="CpuMonitorModule" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        public CpuMonitorModule([NotNull] IPlatformProvider platformProvider) : base(platformProvider)
        {
            var cpuCategory = new PerformanceCounterCategory(CpuCategoryName);

            var cpuInstanceNames = cpuCategory.GetInstanceNames();

            // Add counters for each CPU instance we're using
            foreach (var instance in cpuInstanceNames)
            {
                _processorCounters.Add(
                                       new PerformanceCounter(
                                           CpuCategoryName,
                                           CpuUsageCounterName,
                                           instance,
                                           true));
            }
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>
        ///     The name of the module.
        /// </value>
        public override string Name
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            get { return Resources.CpuMonitorModule_Name; }
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            _cpuWidgets.Clear();

            // _processorCounters is not cleared since it's only populated at startup and its
            // values are used during initialize. Fields will be disposed during Dispose.
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            _cpuWidgets.Clear();

            var core = 1;

            foreach (var counter in _processorCounters.OrderBy(c => c.InstanceName))
            {
                // Don't add a core indicator for the total
                Debug.Assert(counter != null);
                if (counter.InstanceName == TotalInstanceName)
                {
                    continue;
                }

                // Create a widget for the counter
                // Store the counter as the widget's data context for easier updating later on
                var widget = new AlfredProgressBarWidget { DataContext = counter, Minimum = 0, Maximum = 100 };

                // Get the first value of the widget and have the label applied to the widget
                var label = string.Format(CultureInfo.CurrentCulture, _cpuMonitorLabel, core);
                UpdateCpuWidget(widget, counter, label);

                _cpuWidgets.Add(widget);

                Register(widget);

                core++;
            }
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        protected override void UpdateProtected()
        {
            var core = 1;
            foreach (var widget in _cpuWidgets)
            {
                var counter = widget.DataContext as PerformanceCounter;

                // Update the widget using our arbitrary number instead of the instance name
                var label = string.Format(CultureInfo.CurrentCulture, _cpuMonitorLabel, core);
                UpdateCpuWidget(widget, counter, label);

                core++;
            }
        }

        /// <summary>
        ///     Updates the cpu widget with the next value from its counter or an error message if no counter was provided or
        ///     an error occurred reading the value.
        /// </summary>
        /// <param name="widget"> The display widget. </param>
        /// <param name="counter"> The performance counter. </param>
        /// <param name="label"> The label to add before the counter value. </param>
        private static void UpdateCpuWidget(
            [NotNull] AlfredProgressBarWidget widget,
            [CanBeNull] PerformanceCounter counter,
            [CanBeNull] string label)
        {

            widget.Text = string.Format(CultureInfo.CurrentCulture, "{0}:", label);

            widget.Value = GetNextCounterValueSafe(counter, 0);
            widget.ValueFormatString = "{0:F2} %";
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            foreach (var counter in _processorCounters)
            {
                counter.Dispose();
            }
        }
    }
}