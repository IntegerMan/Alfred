using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data model for the home screen. This class cannot be inherited.
    /// </summary>
    public sealed class HomeScreenModel : ScreenModel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="faultManager"> The faultIndicator manager. </param>
        public HomeScreenModel([NotNull] FaultManager faultManager) : base("HOME")
        {
            Contract.Requires(faultManager != null);

            FaultManager = faultManager;
        }

        /// <summary>
        ///     Gets the manager for faultIndicator indicators.
        /// </summary>
        /// <value>
        ///     The faultIndicator manager.
        /// </value>
        [NotNull]
        public FaultManager FaultManager { get; }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor, MFDProcessorResult processorResult)
        {
            // Do nothing
        }

        /// <summary>
        ///     Gets the version display string for the entry assembly.
        /// </summary>
        /// <value>
        ///     The version display string.
        /// </value>
        public string VersionString
        {
            get
            {
                var assembly = GetEntryAssembly();

                var version = assembly.GetName().Version;

                return string.Format("Version {0}", version);
            }
        }

        /// <summary>
        ///     Gets the application author string for display purposes.
        /// </summary>
        /// <value>
        ///     The author string.
        /// </value>
        public string AuthorString
        {
            get
            {
                var versionInfo = GetEntryAssemblyFileVersionInfo();

                return string.Format("Developed by {0}", versionInfo.CompanyName);
            }
        }

        /// <summary>
        ///     Gets the application name display string.
        /// </summary>
        /// <value>
        ///     The application name string.
        /// </value>
        public string ApplicationNameString
        {
            get
            {
                var versionInfo = GetEntryAssemblyFileVersionInfo();

                return versionInfo.ProductName;
            }
        }

        /// <summary>
        ///     Gets the application copyright string.
        /// </summary>
        /// <value>
        ///     The application copyright string.
        /// </value>
        public string CopyrightString
        {
            get
            {
                var versionInfo = GetEntryAssemblyFileVersionInfo();

                return versionInfo.LegalCopyright;
            }
        }

        /// <summary>
        ///     Gets the faultIndicator indicators.
        /// </summary>
        /// <value>
        ///     The faultIndicator indicators.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<FaultIndicatorModel> FaultIndicators
        {
            get { return FaultManager.FaultIndicators; }
        }

        /// <summary>
        ///     Gets entry assembly's file version information.
        /// </summary>
        /// <returns>
        ///     The entry assembly's file version information.
        /// </returns>
        [NotNull]
        private static FileVersionInfo GetEntryAssemblyFileVersionInfo()
        {
            var entryAssembly = GetEntryAssembly();

            var versionInfo = FileVersionInfo.GetVersionInfo(entryAssembly.Location);
            return versionInfo;
        }

        /// <summary>
        ///     Gets entry assembly.
        /// </summary>
        /// <returns>
        ///     The entry assembly.
        /// </returns>
        [NotNull]
        private static Assembly GetEntryAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            Debug.Assert(entryAssembly != null, "entryAssembly != null");

            return entryAssembly;
        }

    }
}
