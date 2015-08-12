// ---------------------------------------------------------
// SystemMonitoringSubsystem.cs
// 
// Created on:      08/07/2015 at 10:12 PM
// Last Modified:   08/07/2015 at 10:36 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     This is a subsystem for the Alfred Framework that allows for monitoring system performance
    ///     and surfacing alerts on critical events.
    /// </summary>
    public sealed class SystemMonitoringSubsystem : AlfredSubsystem, IDisposable
    {
        [NotNull]
        private readonly AlfredModuleListPage _page;

        [NotNull]
        private readonly CpuMonitorModule _cpuModule;

        [NotNull]
        private readonly MemoryMonitorModule _memoryModule;

        [NotNull]
        private readonly DiskMonitorModule _diskModule;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        public SystemMonitoringSubsystem() : this(new SimplePlatformProvider())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public SystemMonitoringSubsystem([NotNull] IPlatformProvider provider) : base(provider)
        {
            _cpuModule = new CpuMonitorModule(provider);
            _memoryModule = new MemoryMonitorModule(provider);
            _diskModule = new DiskMonitorModule(provider);

            _page = new AlfredModuleListPage(provider, Resources.SystemMonitoringSystem_Name.NonNull());
        }

        /// <summary>
        /// Registers the controls for this component.
        /// </summary>
        protected override void RegisterControls()
        {
            Register(_page);

            _page.ClearModules();
            _page.Register(_cpuModule);
            _page.Register(_memoryModule);
            _page.Register(_diskModule);
        }

        /// <summary>
        ///     Gets the name of the subsystems.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public override sealed string Name
        {
            get { return Resources.SystemMonitoringSystem_Name.NonNull(); }
        }

        /// <summary>
        /// Disposes of allocated resources
        /// </summary>
        public void Dispose()
        {
            _cpuModule.Dispose();
            _memoryModule.Dispose();
            _diskModule.Dispose();
        }

        /// <summary>
        ///     Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        public override string Id
        {
            get { return "Sys"; }
        }
    }
}