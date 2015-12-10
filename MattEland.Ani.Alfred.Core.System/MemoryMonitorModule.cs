// ---------------------------------------------------------
// MemoryMonitorModule.cs
// 
// Created on:      08/04/2015 at 9:50 PM
// Last Modified:   08/18/2015 at 2:06 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;
using MattEland.Common.Providers;

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
        private readonly MetricProviderBase _usedBytesCounter;

        [NotNull]
        private readonly ProgressBarWidget _widget;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryMonitorModule" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="factory"> The metric provider factory. </param>
        internal MemoryMonitorModule([NotNull] IAlfredContainer container,
                                   [NotNull] IMetricProviderFactory factory)
            : base(container, factory)
        {
            try
            {
                _usedBytesCounter = MetricProvider.Build(MemoryCategoryName,
                                                         MemoryUtilizationBytesCounterName);
            }
            catch (InvalidOperationException ioex)
            {
                var instance = container.HandleException(ioex, "MEMMON-01", "Bytes counter creation failure");
                LastErrorInstance = instance;
            }

            _widget = new ProgressBarWidget(BuildWidgetParameters(@"progMemoryUsed"))
            {
                Minimum = 0,
                Maximum = 100,
                IsSoftMaximum = true,
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
        /// Gets the percent of memory that is utilized.
        /// </summary>
        /// <value>The memory utilization percentage.</value>
        internal float MemoryUtilization
        {
            get { return _usedBytesCounter.NextValue(); }
        }

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            _usedBytesCounter.TryDispose();
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            _widget.Value = 0;
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            Register(_widget);
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        protected override void UpdateProtected()
        {
            var usedMemory = GetNextCounterValueSafe(_usedBytesCounter);

            _widget.Value = usedMemory;
        }
    }
}