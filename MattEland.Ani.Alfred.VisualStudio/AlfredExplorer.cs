//------------------------------------------------------------------------------
// <copyright file="AlfredExplorer.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace MattEland.Ani.Alfred.VisualStudio
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    ///     This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     In Visual Studio tool windows are composed of a frame (implemented by the shell) and a
    ///     pane, usually implemented by the package implementer.</para>
    ///     <para>
    ///     This class derives from the ToolWindowPane class provided from the MPF in order to use
    ///     its implementation of the IVsUIElementPane interface.</para>
    /// </remarks>
    [Guid("d9fe28aa-928f-41e6-8e8d-64d316c882a9")]
    public sealed class AlfredExplorer : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredExplorer"/> class.
        /// </summary>
        public AlfredExplorer() : base(null)
        {
            // Start Alfred
            AlfredPackage.EnsureAlfredInstance();

            Caption = "Alfred Explorer";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            Content = new AlfredExplorerControl();
        }
    }
}
