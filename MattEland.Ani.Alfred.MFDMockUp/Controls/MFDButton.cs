using System.Windows;
using System.Windows.Controls;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    ///     A multifunction display button. This class cannot be inherited.
    /// </summary>
    public sealed class MFDButton : Button
    {
        /// <summary>
        ///     Initializes static members of the MFDButton class.
        /// </summary>
        static MFDButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MFDButton),
                new FrameworkPropertyMetadata(typeof(MFDButton)));
        }
    }
}
