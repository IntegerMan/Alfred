using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Annotations;
using System.Windows;
using System.Windows.Controls;

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
        }

        /// <summary>
        /// Gets or sets the LayoutType property. 
        /// </summary>
        /// <value> The LayoutType. </value>
        public LayoutType LayoutType { get; set; } = LayoutType.HorizontalAutoSpacePanel;
    }
}