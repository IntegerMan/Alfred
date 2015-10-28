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
        /// Defines the <see cref="LayoutType"/> dependency property. 
        /// </summary>
        /// <remarks> Defaults to LayoutType. </remarks>
        [NotNull]
        public static readonly DependencyProperty LayoutTypeProperty =
            DependencyProperty.Register(nameof(LayoutType), typeof(LayoutType), typeof(MFDButtonLabelStripControl),
                                                                         new PropertyMetadata(LayoutType.HorizontalAutoSpacePanel));

        /// <summary>
        /// Initializes a new instance of the MFDButtonLabelStripControl class. 
        /// </summary>
        public MFDButtonLabelStripControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the LayoutType property using <see cref="LayoutTypeProperty"/>. 
        /// </summary>
        /// <value> The LayoutType. </value>
        public LayoutType LayoutType
        {
            get { return (LayoutType)GetValue(LayoutTypeProperty); }
            set { SetValue(LayoutTypeProperty, value); }
        }
    }
}