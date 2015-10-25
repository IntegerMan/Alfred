using System.Collections.Generic;
using System.Diagnostics;
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
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HomeScreenModel() : base("HOME")
        {
        }
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
                var versionInfo = GetEntryAssemblyFileVersionInfo();

                var version = versionInfo.FileVersion;

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

        [NotNull]
        private static Assembly GetEntryAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            Debug.Assert(entryAssembly != null, "entryAssembly != null");

            return entryAssembly;
        }

    }
}
