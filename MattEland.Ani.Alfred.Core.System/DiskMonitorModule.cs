// ---------------------------------------------------------
// DiskMonitorModule.cs
// 
// Created on:      08/05/2015 at 1:03 AM
// Last Modified:   08/05/2015 at 3:10 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     A module that displays information on the system's disk input / output operations
    /// </summary>
    public class DiskMonitorModule : SystemMonitorModule
    {
        private const string DiskCategoryName = "PhysicalDisk";
        private const string DiskReadCounterName = "% Disk Read Time";
        private const string DiskWriteCounterName = "% Disk Write Time";

        [NotNull]
        private readonly PerformanceCounter _diskReadCounter;

        [NotNull]
        private readonly AlfredProgressBarWidget _diskReadWidget;

        [NotNull]
        private readonly PerformanceCounter _diskWriteCounter;

        [NotNull]
        private readonly AlfredProgressBarWidget _diskWriteWidget;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryMonitorModule" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        public DiskMonitorModule([NotNull] IPlatformProvider platformProvider) : base(platformProvider)
        {
            _diskReadCounter = new PerformanceCounter(DiskCategoryName, DiskReadCounterName, TotalInstanceName, true);
            _diskWriteCounter = new PerformanceCounter(DiskCategoryName, DiskWriteCounterName, TotalInstanceName, true);

            _diskReadWidget = CreatePercentWidget();
            _diskReadWidget.Text = Resources.DiskReadLabel;

            _diskWriteWidget = CreatePercentWidget();
            _diskWriteWidget.Text = Resources.DiskWriteLabel;

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
            get { return Resources.DiskMonitorName; }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            _diskReadCounter.Dispose();
            _diskWriteCounter.Dispose();
        }

        /// <summary>
        ///     Creates a percentage progress bar widget.
        /// </summary>
        /// <returns>A percentage progress bar widget</returns>
        [NotNull]
        private static AlfredProgressBarWidget CreatePercentWidget()
        {
            var widget = new AlfredProgressBarWidget
            {
                Minimum = 0,
                Maximum = 100,
                ValueFormatString = "{0:F2} %"
            };

            return widget;
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            _diskReadWidget.Value = 0;
            _diskWriteWidget.Value = 0;
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            Register(_diskReadWidget);
            Register(_diskWriteWidget);
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        protected override void UpdateProtected()
        {
            var readPercent = GetNextCounterValueSafe(_diskReadCounter);
            var writePercent = GetNextCounterValueSafe(_diskWriteCounter);

            _diskReadWidget.Value = readPercent;
            _diskWriteWidget.Value = writePercent;
        }
    }
}