using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    /// Represents a cockpit-style multifunction display with buttons bordering a central display.
    /// </summary>
    [UsedImplicitly]
    public sealed partial class MFDControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MFDControl"/> class.
        /// </summary>
        public MFDControl()
        {
            InitializeComponent();
        }
    }
}
