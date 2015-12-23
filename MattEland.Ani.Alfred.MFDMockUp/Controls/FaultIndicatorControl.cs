using System.Windows;
using System.Windows.Controls;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    ///     A fault indicator control that displays the status of an alert via text and color.
    /// </summary>
    public sealed class FaultIndicatorControl : Control
    {
        /// <summary>
        ///     Initializes static members of the MFDButton class.
        /// </summary>
        static FaultIndicatorControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FaultIndicatorControl),
                new FrameworkPropertyMetadata(typeof(FaultIndicatorControl)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MFDButton"/> class.
        /// </summary>
        public FaultIndicatorControl()
        {
            // Register with instantiation monitor
            InstantiationMonitor.Instance.NotifyItemCreated(this);
        }

        /// <summary>
        ///     Defines the <see cref="Status"/> dependency property.
        /// </summary>
        /// <remarks>
        ///		Defaults to FaultIndicatorStatus.Inactive
        /// </remarks>
        [NotNull]
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof(Status),
                                                                         typeof(FaultIndicatorStatus),
                                                                         typeof(FaultIndicatorControl),
                                                                         new PropertyMetadata(FaultIndicatorStatus.Inactive));

        /// <summary>
        ///     Gets or sets the Status property using <see cref="StatusProperty"/>.
        /// </summary>
        /// <value>The Status.</value>
        public FaultIndicatorStatus Status
        {
            get { return (FaultIndicatorStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        /// <summary>
        ///     Defines the <see cref="Label"/> dependency property.
        /// </summary>
        /// <remarks>
        ///		Defaults to null
        /// </remarks>
        [NotNull]
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label",
                                                                         typeof(string),
                                                                         typeof(FaultIndicatorControl),
                                                                         new PropertyMetadata(string.Empty));

        /// <summary>
        ///     Gets or sets the Label property using <see cref="LabelProperty"/>.
        /// </summary>
        /// <value>The Name.</value>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
    }
}
