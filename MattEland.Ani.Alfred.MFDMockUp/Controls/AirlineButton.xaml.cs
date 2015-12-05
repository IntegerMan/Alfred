using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using MattEland.Ani.Alfred.MFDMockUp.Models;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    /// Interaction logic for AirlineButton.xaml
    /// </summary>
    public partial class AirlineButton : UserControl
    {
        /// <summary>
        ///     Initializes a new instance of the AirlineButton class.
        /// </summary>
        public AirlineButton()
        {
            InitializeComponent();

            // Register with instantiation monitor
            InstantiationMonitor.Instance.NotifyItemCreated(this);
        }
    }
}
