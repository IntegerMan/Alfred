using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    public sealed class MFDButton : Button
    {
        static MFDButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MFDButton), new FrameworkPropertyMetadata(typeof(MFDButton)));
        }
    }
}
