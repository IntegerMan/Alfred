using System.Windows;
using System.Windows.Controls;

using MattEland.Ani.Alfred.MFDMockUp.Models;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MFDButton"/> class.
        /// </summary>
        public MFDButton()
        {
            // Register with instantiation monitor
            InstantiationMonitor.Instance.NotifyItemCreated(this);
        }
    }
}
