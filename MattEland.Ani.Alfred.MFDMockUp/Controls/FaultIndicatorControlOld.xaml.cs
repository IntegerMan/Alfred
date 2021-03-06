﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using MattEland.Ani.Alfred.MFDMockUp.Models;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    /// Interaction logic for FaultIndicatorControl.xaml
    /// </summary>
    public sealed partial class FaultIndicatorControlOld : UserControl
    {
        /// <summary>
        ///     Initializes a new instance of the FaultIndicatorControl class.
        /// </summary>
        public FaultIndicatorControlOld()
        {
            InitializeComponent();

            // Register with instantiation monitor
            InstantiationMonitor.Instance.NotifyItemCreated(this);
        }
    }
}
