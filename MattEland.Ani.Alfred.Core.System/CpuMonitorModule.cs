// ---------------------------------------------------------
// CpuMonitorModule.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{

    /// <summary>
    ///     A module that displays information on the system's processor utilization percentages
    /// </summary>
    public sealed class CpuMonitorModule : SystemMonitorModule, IDisposable
    {
        /// <summary>
        ///     The performance counter CPU category name
        /// </summary>
        public const string ProcessorCategoryName = "Processor";

        private const string ProcessorUsageCounterName = "% Processor Time";

        // ReSharper disable once AssignNullToNotNullAttribute
        [NotNull]
        private readonly string _cpuMonitorLabel = Resources.CpuMonitorModule_Cpu_Label_Format;

        [NotNull]
        [ItemNotNull]
        private readonly List<AlfredProgressBarWidget> _cpuWidgets =
            new List<AlfredProgressBarWidget>();

        [NotNull]
        [ItemNotNull]
        private readonly List<MetricProviderBase> _processorCounters =
            new List<MetricProviderBase>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="CpuMonitorModule" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        /// <param name="factory">The metric provider factory.</param>
        public CpuMonitorModule(
            [NotNull] IPlatformProvider platformProvider,
            [NotNull] IMetricProviderFactory factory) : base(platformProvider, factory)
        {
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>
        ///     The name of the module.
        /// </value>
        [NotNull]
        public override string Name
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            get { return Resources.CpuMonitorModule_Name; }
        }

        /// <summary>
        ///     Gets the number of CPU cores detected.
        /// </summary>
        /// <value>The number of CPU cores.</value>
        public int NumberOfCores
        {
            get
            {
                // Don't include the aggregate counter
                return _processorCounters.Count(c => c.Name != TotalInstanceName);
            }
        }

        /// <summary>
        ///     Gets the average CPU utilization.
        /// </summary>
        /// <value>The average CPU utilization.</value>
        public float AverageProcessorUtilization
        {
            get
            {
                // Grab the total counter and get its value
                var aggregateCounter =
                    _processorCounters.FirstOrDefault(c => c.Name == TotalInstanceName);
                return aggregateCounter?.NextValue() ?? 0;
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        ///     resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var counter in _processorCounters) { counter.TryDispose(); }
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            // Clear out the counters, bearing in mind that they're disposable
            foreach (var counter in _processorCounters) { counter.TryDispose(); }
            _processorCounters.Clear();

            _cpuWidgets.Clear();
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            BuildCounters();

            _cpuWidgets.Clear();

            var core = 1;

            foreach (var counter in _processorCounters.OrderBy(c => c.Name))
            {
                // Don't add a core indicator for the total
                Debug.Assert(counter != null);
                if (counter.Name == TotalInstanceName) { continue; }

                // Create a widget for the counter
                // Store the counter as the widget's data context for easier updating later on
                var id = string.Format(Locale, "progProcessor{0}", counter.Name);
                var widget = new AlfredProgressBarWidget(BuildWidgetParameters(id));
                widget.DataContext = counter;
                widget.Minimum = 0;
                widget.Maximum = 100;

                // Get the first value of the widget and have the label applied to the widget
                var label = string.Format(CultureInfo.CurrentCulture, _cpuMonitorLabel, core);
                UpdateCpuWidget(widget, counter, label);

                _cpuWidgets.Add(widget);

                Register(widget);

                core++;
            }
        }

        /// <summary>
        ///     Builds the list of processor counters
        /// </summary>
        private void BuildCounters()
        {
            var cpuInstanceNames = MetricProvider.GetCategoryInstanceNames(ProcessorCategoryName);

            // Add counters for each CPU instance we're using
            foreach (var instance in cpuInstanceNames)
            {
                var provider = MetricProvider.Build(ProcessorCategoryName,
                                                    ProcessorUsageCounterName,
                                                    instance);
                _processorCounters.Add(provider);
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
                var counter = widget.DataContext as MetricProviderBase;

                // Update the widget using our arbitrary number instead of the instance name
                var label = string.Format(CultureInfo.CurrentCulture, _cpuMonitorLabel, core);
                UpdateCpuWidget(widget, counter, label);

                core++;
            }
        }

        /// <summary>
        ///     Updates the cpu widget with the next value from its counter or an error message if no counter
        ///     was provided or
        ///     an error occurred reading the value.
        /// </summary>
        /// <param name="widget"> The display widget. </param>
        /// <param name="counter"> The performance counter. </param>
        /// <param name="label"> The label to add before the counter value. </param>
        private static void UpdateCpuWidget(
            [NotNull] AlfredProgressBarWidget widget,
            [CanBeNull] MetricProviderBase counter,
            [CanBeNull] string label)
        {
            widget.Text = string.Format(CultureInfo.CurrentCulture, "{0}:", label);

            widget.Value = GetNextCounterValueSafe(counter, 0);
            widget.ValueFormatString = "{0:F2} %";
        }
    }
}