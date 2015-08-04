// ---------------------------------------------------------
// SystemMonitorModule.cs
// 
// Created on:      08/03/2015 at 8:51 PM
// Last Modified:   08/04/2015 at 2:53 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary> A module that displays information on the system </summary>
    public class SystemMonitorModule : AlfredModule
    {
        private const string CpuCategoryName = "Processor";
        private const string CpuUsageCounterName = "% Processor Time";
        private const string TotalInstanceName = "_Total";

        [NotNull]
        [ItemNotNull]
        private readonly List<TextWidget> _cpuWidgets = new List<TextWidget>();

        [NotNull]
        [ItemNotNull]
        private readonly List<PerformanceCounter> _processorCounters =
            new List<PerformanceCounter>();

        /// <summary> Initializes a new instance of the
        ///     <see cref="AlfredModule" />
        ///     class. </summary>
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
        }

        /// <summary> Handles module initialization events </summary>
        protected override void InitializeProtected()
        {
            _cpuWidgets.Clear();

            foreach (var counter in _processorCounters)
            {
                // Don't add a core indicator for the total
                if (counter.InstanceName == TotalInstanceName)
                {
                    continue;
                }

                var widget = new TextWidget("CPU " + counter.InstanceName + ": Initializing...");
                _cpuWidgets.Add(widget);

                RegisterWidget(widget);
            }
        }

        /// <summary> Handles updating the module as needed </summary>
        protected override void UpdateProtected()
        {
            var cpuIndex = 0;
            foreach (var widget in _cpuWidgets)
            {
                var counter = _processorCounters[cpuIndex];

                // Increment now for ease of use in display
                cpuIndex++;

                // Display counter value or an error if we somehow didn't have a counter
                UpdateCpuWidget(widget, counter, cpuIndex.ToString());
            }
        }

        /// <summary>
        ///     Updates the cpu widget with the next value from its counter or an error message if no counter was provided or
        ///     an error occurred reading the value.
        /// </summary>
        /// <param name="widget"> The display widget. </param>
        /// <param name="counter"> The performance counter. </param>
        /// <param name="instance"> The instance name. </param>
        private static void UpdateCpuWidget(
            [NotNull] AlfredTextWidget widget,
            [CanBeNull] PerformanceCounter counter,
            [CanBeNull] string instance)
        {
            const string NotFoundMessage = "Error - Not Found";

            widget.Text = counter != null
                              ? $"CPU {instance}: {GetCpuPercentString(counter)}"
                              : $"CPU {instance}: {NotFoundMessage}";
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