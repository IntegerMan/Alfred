//------------------------------------------------------------------------------
// <copyright file="AlfredToolWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

using MattEland.Ani.Alfred.VisualStudio.Properties;

using Microsoft.VisualStudio.Shell;

namespace MattEland.Ani.Alfred.VisualStudio
{

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("24441ce2-233b-4f52-ad5c-a3fe49af3084")]
    public sealed class AlfredToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredToolWindow"/> class.
        /// </summary>
        /// <exception cref="Exception">
        ///     If any exception was encountered during startup, it will be logged and
        ///     rethrown to the Visual Studio Host.
        /// </exception>
        public AlfredToolWindow() : base(null)
        {
            Caption = Resources.AlfredToolWindowCaption;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            Content = new AlfredToolWindowControl();
        }
    }
}
