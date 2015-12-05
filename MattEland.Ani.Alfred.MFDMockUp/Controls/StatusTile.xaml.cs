using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using MattEland.Ani.Alfred.MFDMockUp.Models;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <content>
    ///     The StatusTile is a special type of HeaderedContentControl that presents as a tile in
    ///     format believable on an MFD screen.
    /// </content>
    public sealed partial class StatusTile
    {
        /// <summary>
        ///     Initializes a new instance of the StatusTile class.
        /// </summary>
        public StatusTile()
        {
            InitializeComponent();

            // Register with instantiation monitor
            InstantiationMonitor.Instance.NotifyItemCreated(this);
        }

    }
}
