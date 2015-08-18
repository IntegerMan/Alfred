// ---------------------------------------------------------
// MemoryMonitorModule.cs
// 
// Created on:      08/04/2015 at 9:50 PM
// Last Modified:   08/18/2015 at 2:06 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     A module that displays information on the system's memory utilization percentage
    /// </summary>
    public sealed class MemoryMonitorModule : SystemMonitorModule, IDisposable
    {
        private const string MemoryCategoryName = "Memory";
        private const string MemoryUtilizationBytesCounterName = "% Committed Bytes in Use";

        [NotNull]
        private readonly MetricProviderBase _memUsedBytesCounter;

        [NotNull]
        private readonly AlfredProgressBarWidget _memWidget;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryMonitorModule" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        /// <param name="factory">The metric provider factory.</param>
        public MemoryMonitorModule([NotNull] IPlatformProvider platformProvider,
                                   [NotNull] IMetricProviderFactory factory)
            : base(platformProvider, factory)
        {
            _memUsedBytesCounter = MetricProvider.Build(MemoryCategoryName,
                                                        MemoryUtilizationBytesCounterName);

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
        ///     Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            _memUsedBytesCounter.Dispose();
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
        /// <param name="alfred">The alfred instance.</param>
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
    }
}