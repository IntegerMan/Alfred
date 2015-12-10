// ---------------------------------------------------------
// SystemMonitoringSubSystem.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   11/05/2015 at 4:12 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     This is a subsystem for the Alfred Framework that allows for monitoring system
    ///     performance and surfacing alerts on critical events.
    /// </summary>
    public sealed class SystemMonitoringSubsystem : AlfredSubsystem, IDisposable
    {
        /// <summary>
        ///     The identifier for the subsystem. This value will be set for the instance's
        ///     <see cref="Id"/>.
        /// </summary>
        [NotNull]
        public const string InstanceId = "Perf";

        [NotNull]
        private readonly CpuMonitorModule _cpuModule;

        [NotNull]
        private readonly DiskMonitorModule _diskModule;

        [NotNull]
        private readonly MemoryMonitorModule _memoryModule;

        [NotNull]
        private readonly ModuleListPage _page;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        public SystemMonitoringSubsystem([NotNull] IAlfredContainer container) : base(container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            // Grab a factory from the container. This can be counter-based or a test factory.
            var factory = Container.Provide<IMetricProviderFactory>();

            // Set up modules
            _cpuModule = new CpuMonitorModule(container, factory);
            _memoryModule = new MemoryMonitorModule(container, factory);
            _diskModule = new DiskMonitorModule(container, factory);

            // Set up the containing page
            _page = new ModuleListPage(container,
                                       Resources.SystemMonitoringSystem_Name.NonNull(),
                                       InstanceId);
        }

        /// <summary>
        ///     Gets the CPU monitoring module.
        /// </summary>
        /// <value>
        ///     The CPU monitoring module.
        /// </value>
        [NotNull]
        public CpuMonitorModule CpuModule
        {
            [DebuggerStepThrough]
            get
            {
                return _cpuModule;
            }
        }

        /// <summary>
        ///     Gets the disk monitoring module.
        /// </summary>
        /// <value>
        ///     The disk module.
        /// </value>
        [NotNull]
        public DiskMonitorModule DiskModule
        {
            [DebuggerStepThrough]
            get
            {
                return _diskModule;
            }
        }

        /// <summary>
        ///     Gets the memory monitoring module.
        /// </summary>
        /// <value>
        ///     The memory module.
        /// </value>
        [NotNull]
        public MemoryMonitorModule MemoryModule
        {
            [DebuggerStepThrough]
            get
            {
                return _memoryModule;
            }
        }

        /// <summary>
        ///     Gets the system modules associated with this subsystem.
        /// </summary>
        /// <value>
        ///     The system modules.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<SystemMonitorModule> SystemModules
        {
            get
            {
                yield return MemoryModule;
                yield return CpuModule;
                yield return DiskModule;
            }
        }

        /// <summary>
        ///     Gets the name of the subsystems.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [NotNull]
        public override string Name
        {
            get { return Resources.SystemMonitoringSystem_Name.NonNull(); }
        }

        /// <summary>
        ///     Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>
        /// The identifier for the subsystem.
        /// </value>
        public override string Id
        {
            get { return "Sys"; }
        }

        /// <summary>
        ///     Disposes of allocated resources
        /// </summary>
        public void Dispose()
        {
            _cpuModule.Dispose();
            _memoryModule.Dispose();
            _diskModule.Dispose();
        }

        /// <summary>
        ///     Registers the controls for this component.
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
        ///     Updates the component
        /// </summary>
        protected override void UpdateProtected()
        {
            var lastEx = LastError;
            var lastExTime = LastErrorTime;

            foreach (var sys in SystemModules)
            {
                if (!sys.HasError) continue;
                if (sys.LastErrorTime <= lastExTime) continue;

                lastEx = sys.LastError;
                lastExTime = sys.LastErrorTime;
            }

            // Update the aggregate last error to the latest one
            if (lastEx != null && lastEx != LastError)
            {
                LastError = lastEx;
            }
        }

        /// <summary>
        ///     Acknowledges the <see cref="M:LastError"/> error and clears out the exception.
        /// </summary>
        public override void AcknowledgeError()
        {
            // Clear out child errors
            foreach (var sys in SystemModules)
            {
                sys.AcknowledgeError();
            }

            // Acknowledge the error
            base.AcknowledgeError();
        }

        /// <summary>
        ///     Processes an Alfred Command. If the <paramref name="command" /> is handled,
        ///     <paramref name="result" /> should be modified accordingly and the method should
        ///     return true. Returning <see langword="false" /> will not stop the message from being
        ///     propagated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">
        /// The result. If the <paramref name="command" /> was handled, this should be updated.
        /// </param>
        /// <returns>
        ///     <c>True</c> if the <paramref name="command" /> was handled; otherwise false.
        /// </returns>
        public override bool ProcessAlfredCommand(ChatCommand command,
                                                  [CanBeNull] ICommandResult result)
        {
            if (result == null)
            {
                return false;
            }

            if (command.IsFor(this) && command.Name.Matches("Status"))
            {
                result.Output = GetStatusText(command.Data);
                return true;
            }

            return base.ProcessAlfredCommand(command, result);
        }

        /// <summary>
        ///     Gets the status text for the given <paramref name="data"/> parameter.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The status text.</returns>
        private string GetStatusText([CanBeNull] string data)
        {
            var alfred = AlfredInstance;
            if (alfred == null)
            {
                return "No Alfred integration is detected. The system may be offline.";
            }

            var sb = new StringBuilder();

            // Alfred status command
            if (data.IsEmpty() || data.Matches("Alfred"))
            {
                sb.AppendFormat(CultureInfo.CurrentCulture,
                                "The system is {0} with a total of {1} {2} Present. ",
                                Status.ToString().ToLowerInvariant(),
                                alfred.Subsystems.Count(),
                                alfred.Subsystems.Pluralize("Subsystem", "Subsystems"));
            }

            // CPU status command
            if (data.IsEmpty() || data.Matches("CPU"))
            {
                sb.AppendFormat(CultureInfo.CurrentCulture,
                                "There {3} {0} CPU {1} with an average of {2:F1} % utilization. ",
                                _cpuModule.NumberOfCores,
                                _cpuModule.NumberOfCores.Pluralize("core", "cores"),
                                _cpuModule.AverageProcessorUtilization,
                                _cpuModule.NumberOfCores.Pluralize("is", "are"));
            }

            // Memory Status Command
            if (data.IsEmpty() || data.Matches("Memory"))
            {
                sb.AppendFormat(CultureInfo.CurrentCulture,
                                "The system is currently utilizing {0:F1} % of all available memory. ",
                                _memoryModule.MemoryUtilization);
            }

            // Disk Status Command
            if (data.IsEmpty() || data.Matches("Disk"))
            {
                sb.AppendFormat(CultureInfo.CurrentCulture,
                                "Disk read speed is currently utilized at {0:F1} % and disk write utilization is at {1:F1} %. ",
                                _diskModule.ReadUtilization,
                                _diskModule.WriteUtilization);
            }

            return sb.ToString();
        }
    }
}