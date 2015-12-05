using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    ///     A sensor of interest indicator. This class cannot be inherited.
    /// </summary>
    public sealed class SensorOfInterestIndicator : Control
    {
        /// <summary>
        ///     Initializes static members of the SensorOfInterestIndicator class.
        /// </summary>
        static SensorOfInterestIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SensorOfInterestIndicator),
                new FrameworkPropertyMetadata(typeof(SensorOfInterestIndicator)));
        }

        /// <summary>
        ///     Defines the <see cref="IsSensorOfInterest"/> dependency property.
        /// </summary>
        /// <remarks>
        ///		Defaults to false
        /// </remarks>
        [NotNull]
        public static readonly DependencyProperty IsSensorOfInterestProperty = DependencyProperty.Register(nameof(IsSensorOfInterest),
                                                                         typeof(bool),
                                                                         typeof(SensorOfInterestIndicator),
                                                                         new PropertyMetadata(false));

        /// <summary>
        ///     Gets or sets the IsSensorOfInterest property using <see cref="IsSensorOfInterestProperty"/>.
        /// </summary>
        /// <value>The IsSensorOfInterest.</value>
        public bool IsSensorOfInterest
        {
            get { return (bool)GetValue(IsSensorOfInterestProperty); }
            set { SetValue(IsSensorOfInterestProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Controls.Control"/> class. 
        /// </summary>
        public SensorOfInterestIndicator()
        {
            // Register with instantiation monitor
            InstantiationMonitor.Instance.NotifyItemCreated(this);
        }
    }
}
