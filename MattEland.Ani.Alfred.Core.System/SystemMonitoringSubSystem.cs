// ---------------------------------------------------------
// SystemMonitoringSubsystem.cs
// 
// Created on:      08/07/2015 at 10:12 PM
// Last Modified:   08/07/2015 at 10:36 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

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
        /// <param name="provider">The provider.</param>
        /// <param name="factory"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="Win32Exception">A call to an underlying system API failed.</exception>
        /// <exception cref="UnauthorizedAccessException">Code that is executing without administrative privileges attempted to read a performance counter.</exception>
        public SystemMonitoringSubsystem([NotNull] IPlatformProvider provider, [NotNull] IMetricProviderFactory factory) : base(provider)
        {
            _cpuModule = new CpuMonitorModule(provider, factory);
            _memoryModule = new MemoryMonitorModule(provider, factory);
            _diskModule = new DiskMonitorModule(provider, factory);

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
        public override string Name
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
        /// Processes an Alfred Command. If the command is handled, result should be modified accordingly and the method should return true. Returning false will not stop the message from being propogated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The result. If the command was handled, this should be updated.</param>
        /// <returns><c>True</c> if the command was handled; otherwise false.</returns>
        public override bool ProcessAlfredCommand(ChatCommand command, AlfredCommandResult result)
        {
            if (command.IsFor(this) && command.Name.Matches("Status"))
            {
                result.Output = GetStatusText(command.Data);
                return true;
            }

            return base.ProcessAlfredCommand(command, result);
        }

        private string GetStatusText(string data)
        {

            var alfred = AlfredInstance;
            if (alfred == null)
            {
                return "No Alfred integration is detected. The system may be offline.";
            }

            var sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.CurrentCulture,
                                 "The System is {0} with a total of {1} Subsystems Present. ",
                                 alfred.Status,
                                 alfred.Subsystems.Count());

            // TODO: Add modules from other areas


            return sb.ToString();
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