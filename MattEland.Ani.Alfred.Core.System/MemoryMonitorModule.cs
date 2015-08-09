// ---------------------------------------------------------
// MemoryMonitorModule.cs
// 
// Created on:      08/04/2015 at 9:50 PM
// Last Modified:   08/04/2015 at 10:05 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     A module that displays information on the system's memory utilization percentage
    /// </summary>
    public class MemoryMonitorModule : SystemMonitorModule
    {
        private const string MemoryCategoryName = "Memory";
        private const string MemoryUtilizationBytesCounterName = "% Committed Bytes in Use";

        [NotNull]
        private readonly PerformanceCounter _memUsedBytesCounter;

        [NotNull]
        private readonly AlfredProgressBarWidget _memWidget;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryMonitorModule" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        public MemoryMonitorModule([NotNull] IPlatformProvider platformProvider) : base(platformProvider)
        {
            _memUsedBytesCounter = new PerformanceCounter(MemoryCategoryName, MemoryUtilizationBytesCounterName, true);

            _memWidget = new AlfredProgressBarWidget
            {
                Minimum = 0,
                Maximum = 100,
                Text = Resources.MemoryMonitorModule_LabelName,
                ValueFormatString = "{0:F2} %"
            };

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
            get { return Resources.MemoryMonitorModule_Name; }
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            _memWidget.Value = 0;
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            Register(_memWidget);
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        protected override void UpdateProtected()
        {
            var usedMemory = GetNextCounterValueSafe(_memUsedBytesCounter);

            _memWidget.Value = usedMemory;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            _memUsedBytesCounter.Dispose();
        }
    }
}