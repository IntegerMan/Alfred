using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    ///     A status label control containing a textual area and a status label.
    /// </summary>
    [PublicAPI]
    public sealed class StatusLabel : HeaderedContentControl
    {
        /// <summary>
        ///     Initializes static members of the StatusLabel class.
        /// </summary>
        static StatusLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusLabel),
                new FrameworkPropertyMetadata(typeof(StatusLabel)));
        }

        /// <summary>
        ///     Defines the <see cref="StatusForeground"/> dependency property.
        /// </summary>
        /// <remarks>
        ///		Defaults to Brushes.White
        /// </remarks>
        [NotNull]
        public static readonly DependencyProperty StatusForegroundProperty =
            DependencyProperty.Register(nameof(StatusForeground),
                                        typeof(Brush),
                                        typeof(StatusLabel),
                                        new PropertyMetadata(Brushes.White));

        /// <summary>
        ///     Gets or sets the StatusForeground property using <see cref="StatusForegroundProperty"/>.
        /// </summary>
        /// <value>The StatusForeground.</value>
        public Brush StatusForeground
        {
            get { return (Brush)GetValue(StatusForegroundProperty); }
            set { SetValue(StatusForegroundProperty, value); }
        }

    }
}
