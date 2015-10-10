//------------------------------------------------------------------------------
// <copyright file="AlfredToolWindowPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Console;

using MattEland.Ani.Alfred.VisualStudio.Properties;
using MattEland.Common.Providers;

using Microsoft.VisualStudio.Shell;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.PresentationAvalon.Commands;
using MattEland.Ani.Alfred.PresentationCommon.Commands;

namespace MattEland.Ani.Alfred.VisualStudio
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideToolWindow(typeof(AlfredToolWindow))]
    [ProvideToolWindow(typeof(AlfredChatWindow))]
    [ProvideToolWindow(typeof(AlfredExplorer))]
    public sealed class AlfredPackage : Package
    {
        [CanBeNull]
        private static ApplicationManager _app;

        /// <summary>
        /// <see cref="AlfredPackage"/> GUID string.
        /// </summary>
        public const string PackageGuidString = "f2ff3af0-b1ad-4d94-ba2f-04a180fb9de4";

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredToolWindow"/> class.
        /// </summary>
        public AlfredPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.

            EnsureAlfredInstance();
        }

        /// <summary>
        ///     Ensures the Alfred instance exists, creating it if it does not.
        /// </summary>
        /// <returns>The <see cref="ApplicationManager"/>.</returns>
        [NotNull]
        internal static ApplicationManager EnsureAlfredInstance()
        {
            if (_app == null)
            {
                var options = new ApplicationManagerOptions()
                {
                    IsSpeechEnabled = true,
                    ShowMindExplorerPage = false
                };

                // Build out the app manager
                var container = AlfredContainerHelper.ProvideContainer();
                _app = new ApplicationManager(container, options);

                _app.Console?.Log(Resources.AlfredPackageInstantiatingAlfredLogHeader, Resources.AlfredPackageInstantiatingAlfredLogMessage, LogLevel.Verbose);

                // Auto Start
                Debug.Assert(Settings.Default != null);
                if (Settings.Default.AutoStartAlfred)
                {
                    _app.Console?.Log(Resources.AlfredPackageInstantiatingAlfredLogHeader, Resources.AlfredPackageEnsureAlfredInstanceAutoStartingLogMessage, LogLevel.Verbose);
                    _app.Start();
                }
            }

            return _app;
        }

        /// <summary>
        ///     Gets the Alfred <see cref="ApplicationManager"/> instance.
        /// </summary>
        /// <value>
        /// The Alfred instance.
        /// </value>
        [NotNull]
        public static ApplicationManager AlfredInstance
        {
            get
            {
                return _app = EnsureAlfredInstance();
            }
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by Visual Studio.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();

            // Let's get Alfred ready before we add the controls
            EnsureAlfredInstance();

            // Add tool window display commands
            AlfredToolWindowCommand.Initialize(this);
            AlfredChatWindowCommand.Initialize(this);
            AlfredExplorerCommand.Initialize(this);
        }

        #endregion
    }
}
