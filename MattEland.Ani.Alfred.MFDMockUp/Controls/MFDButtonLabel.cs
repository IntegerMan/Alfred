using MattEland.Common.Annotations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MattEland.Ani.Alfred.MFDMockUp.Controls
{
    /// <summary>
    ///     A <see cref="Control"/> used to render textual content to a multifunction display as a
    ///     label for a MFD Button.
    /// </summary>
    [ContentProperty(nameof(Text))]
    public sealed class MFDButtonLabel : Control
    {
        /// <summary>
        /// Defines the <see cref="IsSelected"/> dependency property. 
        /// </summary>
        /// <remarks> Defaults to false </remarks>
        [NotNull]
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected),
                                                                         typeof(bool),
                                                                         typeof(MFDButtonLabel),
                                                                         new PropertyMetadata(false));

        /// <summary>
        ///     Initializes static members of the MFDButtonLabel class.
        /// </summary>
        static MFDButtonLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MFDButtonLabel),
                new FrameworkPropertyMetadata(typeof(MFDButtonLabel)));
        }

        /// <summary>
        /// Gets or sets the IsSelected property using <see cref="IsSelectedProperty"/>. 
        /// </summary>
        /// <value> The IsSelected. </value>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        ///     Defines the <see cref="Text"/> dependency property.
        /// </summary>
        /// <remarks>
        ///		Defaults to null
        /// </remarks>
        [NotNull]
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
                                                                         typeof(string),
                                                                         typeof(MFDButtonLabel),
                                                                         new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets the Text property using <see cref="TextProperty"/>. This is the default
        ///     content property for <see cref="MFDButtonLabel"/>.
        /// </summary>
        /// <value>
        ///     The text of the label.
        /// </value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}