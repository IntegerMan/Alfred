using System.ComponentModel;

using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Interface defining a user interface widget. Widgets are essentially the View Model of
    ///     user interface components mixed with the blueprints that instruct the client layer to
    ///     build, arrange, and configure the concrete client-specific user interface component(s)
    ///     for any given widget.
    /// </summary>
    public interface IWidget : INotifyPropertyChanged, IPropertyProvider, IHasContainer
    {
        /// <summary>
        ///     Gets or sets whether or not the widget is visible. This defaults to <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if this widget is visible; otherwise, <c>false</c>.</value>
        bool IsVisible { get; set; }

        /// <summary>
        ///     Gets or sets the data context.
        /// </summary>
        /// <remarks>
        ///     The DataContext is used by some controls for data binding and can act as a tag value
        ///     in others allowing the caller to put miscellaneous information related to what the widget
        ///     represents so that the
        ///     widget can be updated later.
        /// </remarks>
        /// <value>The data context.</value>
        object DataContext { get; set; }

    }
}