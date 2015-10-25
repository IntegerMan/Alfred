// ---------------------------------------------------------
// MFDButtonStrip.xaml.cs
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

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    /// Represents a strip of buttons bordering a <see cref="MFDControl"/>
    /// </summary>
    [PublicAPI]
    public sealed class MFDButtonStrip : Control
    {

        /// <summary>
        /// Initializes static members of the <see cref="MFDButtonStrip"/> class.
        /// </summary>
        static MFDButtonStrip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MFDButtonStrip),
                new FrameworkPropertyMetadata(typeof(MFDButtonStrip)));
        }

        /// <summary>
        ///     Defines the <see cref="Orientation"/> dependency property.
        /// </summary>
        /// <remarks>
        ///		Defaults to Horizontal
        /// </remarks>
        [NotNull]
        public static readonly DependencyProperty DockDirectionProperty =
            DependencyProperty.Register(nameof(DockDirection),
                                        typeof(ButtonStripDock),
                                        typeof(MFDButtonStrip),
                                        new PropertyMetadata(ButtonStripDock.Top));

        /// <summary>
        ///     Gets or sets the Dock Direction property using <see cref="DockDirectionProperty"/>.
        /// </summary>
        /// <value>The Orientation.</value>
        public ButtonStripDock DockDirection
        {
            get { return (ButtonStripDock)GetValue(DockDirectionProperty); }
            set { SetValue(DockDirectionProperty, value); }
        }
    }
}