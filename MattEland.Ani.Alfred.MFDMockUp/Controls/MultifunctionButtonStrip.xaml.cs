// ---------------------------------------------------------
// MultifunctionButtonStrip.xaml.cs
// 
// Created on:      10/17/2015 at 12:43 PM
// Last Modified:   10/17/2015 at 12:53 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    /// Represents a strip of buttons bordering a <see cref="MultifunctionDisplay"/>
    /// </summary>
    public sealed partial class MultifunctionButtonStrip : UserControl
    {

        /// <summary>
        ///     Defines the <see cref="Orientation"/> dependency property.
        /// </summary>
        /// <remarks>
        ///		Defaults to Horizontal
        /// </remarks>
        [NotNull]
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation),
                                        typeof(Orientation),
                                        typeof(MultifunctionButtonStrip),
                                        new PropertyMetadata(Orientation.Horizontal));

        /// <summary>
        ///     Initializes a new instance of the <see cref="MultifunctionButtonStrip" />
        ///     class.
        /// </summary>
        public MultifunctionButtonStrip()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the Orientation property using <see cref="OrientationProperty"/>.
        /// </summary>
        /// <value>The Orientation.</value>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
    }
}