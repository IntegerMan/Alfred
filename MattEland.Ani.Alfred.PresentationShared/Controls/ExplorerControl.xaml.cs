﻿// ---------------------------------------------------------
// ExplorerControl.xaml.cs
// 
// Created on:      08/23/2015 at 12:48 AM
// Last Modified:   08/24/2015 at 6:03 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.PresentationAvalon.Controls
{

    /// <summary>
    ///     Interaction logic for ExplorerControl.xaml
    /// </summary>
    public partial class ExplorerControl : IUserInterfaceTestable
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExplorerControl" /> class.
        /// </summary>
        public ExplorerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExplorerControl" /> class.
        /// </summary>
        /// <param name="rootNodes">The root nodes.</param>
        public ExplorerControl(IEnumerable<IPropertyProvider> rootNodes) : this()
        {
            RootNodes = rootNodes;
        }

        /// <summary>
        ///     The root nodes dependency property.
        ///     
        ///     TODO: Look into this - the NotifyPropertyChanged didn't seem to go through.
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty RootNodesProperty =
            DependencyProperty.Register("RootNodes",
                                        typeof(IEnumerable<IPropertyProvider>),
                                        typeof(ExplorerControl),
                                        new PropertyMetadata(
                                            default(IEnumerable<IPropertyProvider>),
                                            OnRootNodesChanged));

        /// <summary>
        ///     Handles the <see cref="E:RootNodesChanged" /> event.
        /// </summary>
        /// <param name="d">The dependence object.</param>
        /// <param name="e">
        ///     The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance
        ///     containing the event data.
        /// </param>
        private static void OnRootNodesChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as ExplorerControl;

            control?.HandleRootNodesChanged();
        }

        /// <summary>
        ///     Gets or sets the root node.
        /// </summary>
        /// <value>The root node.</value>
        [CanBeNull]
        [ItemNotNull]
        public IEnumerable<IPropertyProvider> RootNodes
        {
            get { return (IEnumerable<IPropertyProvider>)GetValue(RootNodesProperty); }
            set { SetValue(RootNodesProperty, value); }
        }

        /// <summary>
        ///     Handles the root nodes changed.
        /// </summary>
        private void HandleRootNodesChanged()
        {
            Debug.Assert(TreeHierarchy != null);
            TreeHierarchy.ItemsSource = RootNodes;
        }

        /// <summary>
        ///     Simulates the page's loaded event.
        /// </summary>
        public void SimulateLoadedEvent()
        {
        }
    }
}