using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Annotations;
using System.Windows;
using System.Windows.Controls;

using MattEland.Ani.Alfred.MFDMockUp.Models;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    /// Interaction logic for MFDButtonLabelStripControl.xaml 
    /// </summary>
    [UsedImplicitly]
    public sealed partial class MFDButtonLabelStripControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the MFDButtonLabelStripControl class. 
        /// </summary>
        public MFDButtonLabelStripControl()
        {
            InitializeComponent();

            // Register with instantiation monitor
            InstantiationMonitor.Instance.NotifyItemCreated(this);
        }

        /// <summary>
        /// Gets or sets the LayoutType property. 
        /// </summary>
        /// <value> The LayoutType. </value>
        public LayoutType LayoutType { get; set; } = LayoutType.HorizontalAutoSpacePanel;
    }
}