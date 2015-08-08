// ---------------------------------------------------------
// SystemMonitoringSystem.cs
// 
// Created on:      08/07/2015 at 10:12 PM
// Last Modified:   08/07/2015 at 10:12 PM
// Original author: Matt Eland
// ---------------------------------------------------------
namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    /// This is a System for the Alfred Framework that allows for monitoring system performance
    /// and surfacing alerts on critical events.
    /// </summary>
    public class SystemMonitoringSystem : AlfredSubSystem
    {
        /// <summary>
        /// Gets the name of the subsystems.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return Resources.SystemMonitoringSystem_Name.NonNull(); }
        }
    }
}