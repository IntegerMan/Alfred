// ---------------------------------------------------------
// MFDButtonStripControl.xaml.cs
// 
// Created on:      10/17/2015 at 12:43 PM
// Last Modified:   10/17/2015 at 12:53 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    /// Represents a strip of buttons bordering a <see cref="MFDControl"/>
    /// </summary>
    public sealed partial class MFDButtonStripControl : UserControl
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="MFDButtonStripControl" />
        ///     class.
        /// </summary>
        public MFDButtonStripControl()
        {
            InitializeComponent();
        }

    }
}