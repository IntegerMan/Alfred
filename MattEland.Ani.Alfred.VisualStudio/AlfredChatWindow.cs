//------------------------------------------------------------------------------
// <copyright file="AlfredChatWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Runtime.InteropServices;

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
    [Guid("b068e922-fdc8-4e9b-9951-af36ca688f7c")]
    public sealed class AlfredChatWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredChatWindow"/> class.
        /// </summary>
        public AlfredChatWindow() : base(null)
        {
            Caption = "AlfredChatWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            Content = new AlfredChatWindowControl();
        }
    }
}
