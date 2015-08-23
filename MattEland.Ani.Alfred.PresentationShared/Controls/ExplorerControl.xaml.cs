using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.PresentationShared.Controls
{
    /// <summary>
    /// Interaction logic for ExplorerControl.xaml
    /// </summary>
    public partial class ExplorerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplorerControl"/> class.
        /// </summary>
        public ExplorerControl()
        {
            InitializeComponent();
        }

        public ExplorerControl(IEnumerable<IPropertyProvider> rootNodes) : this()
        {
            RootNodes = rootNodes;
        }

        [CanBeNull]
        private IEnumerable<IPropertyProvider> _rootNodes;

        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        /// <value>The root node.</value>
        [CanBeNull]
        public IEnumerable<IPropertyProvider> RootNodes
        {
            get { return _rootNodes; }
            set
            {
                _rootNodes = value;
                Debug.Assert(TreeHierarchy != null);
                TreeHierarchy.ItemsSource = value;
            }
        }

        /// <summary>
        /// Simulates the page's loaded event.
        /// </summary>
        public void SimulateLoadedEvent()
        {
        }
    }
}
