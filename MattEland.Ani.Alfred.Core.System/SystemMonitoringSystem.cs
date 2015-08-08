﻿// ---------------------------------------------------------
// SystemMonitoringSystem.cs
// 
// Created on:      08/07/2015 at 10:12 PM
// Last Modified:   08/07/2015 at 10:36 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     This is a System for the Alfred Framework that allows for monitoring system performance
    ///     and surfacing alerts on critical events.
    /// </summary>
    public class SystemMonitoringSystem : AlfredSubSystem
    {
        [NotNull]
        private readonly UserInterfacePage _page;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubSystem" /> class.
        /// </summary>
        public SystemMonitoringSystem() : this(new SimplePlatformProvider())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubSystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public SystemMonitoringSystem([NotNull] IPlatformProvider provider) : base(provider)
        {
            var cpu = new CpuMonitorModule(provider);
            Register(cpu);

            var memory = new MemoryMonitorModule(provider);
            Register(memory);

            var disk = new DiskMonitorModule(provider);
            Register(disk);

            _page = new UserInterfacePage(provider);
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        protected override void InitializeProtected()
        {
            Register(_page);
        }

        /// <summary>
        ///     Gets the name of the subsystems.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return Resources.SystemMonitoringSystem_Name.NonNull(); }
        }
    }
}