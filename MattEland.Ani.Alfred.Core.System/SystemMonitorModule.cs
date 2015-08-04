// ---------------------------------------------------------
// SystemMonitorModule.cs
// 
// Created on:      08/03/2015 at 8:51 PM
// Last Modified:   08/04/2015 at 3:20 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     A module that displays information on the system
    /// </summary>
    public class SystemMonitorModule : AlfredModule
    {
        private const string CpuCategoryName = "Processor";
        private const string CpuUsageCounterName = "% Processor Time";
        private const string TotalInstanceName = "_Total";
        private const string CpuMonitorLabel = "CPU {0}";

        [NotNull]
        [ItemNotNull]
        private readonly List<TextWidget> _cpuWidgets = new List<TextWidget>();

        [NotNull]
        [ItemNotNull]
        private readonly List<PerformanceCounter> _processorCounters =
            new List<PerformanceCounter>();

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="AlfredModule" />
        ///     class.
        /// </summary>
        /// <param name="platformProvider"> The platform provider. </param>
        /// <exception cref="ArgumentNullException"> </exception>
        public SystemMonitorModule([NotNull] IPlatformProvider platformProvider) : base(platformProvider)
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

        /// <summary> Gets the name of the module. </summary>
        /// <value> The name of the module. </value>
        public override string Name
        {
            get { return "System Monitor"; }
        }

        /// <summary> Handles module shutdown events </summary>
        protected override void ShutdownProtected()
        {
            _cpuWidgets.Clear();

            // _processorCounters is not cleared since it's only populated at startup and its values are used during initialize.
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        protected override void InitializeProtected()
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
                var widget = new TextWidget { DataContext = counter };

                // Get the first value of the widget and have the label applied to the widget
                var label = string.Format(CultureInfo.CurrentCulture, CpuMonitorLabel, core);
                UpdateCpuWidget(widget, counter, label);

                _cpuWidgets.Add(widget);

                RegisterWidget(widget);

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
                var label = string.Format(CultureInfo.CurrentCulture, CpuMonitorLabel, core);
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
            [NotNull] AlfredTextWidget widget,
            [CanBeNull] PerformanceCounter counter,
            [CanBeNull] string label)
        {
            const string NotFoundMessage = "Error - Not Found";

            widget.Text = counter != null
                              ? $"{label}: {GetCpuPercentString(counter)}"
                              : $"{label}: {NotFoundMessage}";
        }

        /// <summary>
        ///     Gets a processor utilization string as a percentage for a given performance counter, safely handling all
        ///     documented exceptions for getting the next value.
        /// </summary>
        /// <param name="counter"> The counter. </param>
        /// <returns> System.String. </returns>
        [NotNull]
        private static string GetCpuPercentString([NotNull] PerformanceCounter counter)
        {
            if (counter == null)
            {
                throw new ArgumentNullException(nameof(counter));
            }

            try // Catch each exception thrown by counter.NextValue() instead of catching Exception.
            {
                // If you try to interpret it as a % in the format string the value will be multiplied * 100.
                return counter.NextValue().ToString("0.00", CultureInfo.CurrentCulture) + " %";
            }
            catch (Win32Exception ex)
            {
                return ex.Message;
            }
            catch (PlatformNotSupportedException ex)
            {
                return ex.Message;
            }
            catch (UnauthorizedAccessException ex)
            {
                return ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
        }
    }
}