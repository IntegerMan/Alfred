// ---------------------------------------------------------
// AlfredWidget.cs
// 
// Created on:      07/28/2015 at 12:18 PM
// Last Modified:   08/04/2015 at 3:04 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     Any sort of user interface widget for representing a module.
    ///     Widgets can range from simple text to interactive components.
    ///     Widgets do not contain user interface elements but tell the
    ///     client what user interface elements to create.
    /// </summary>
    public abstract class AlfredWidget : NotifyPropertyChangedBase
    {
        [CanBeNull]
        private object _dataContext;

        private bool _isVisible = true;

        /// <summary>
        ///     Gets or sets whether or not the widget is visible. This defaults to <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if this widget is visible; otherwise, <c>false</c>.</value>
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible)
                {
                    return;
                }
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        /// <summary>
        ///     Gets or sets the data context.
        /// </summary>
        /// <remarks>
        ///     The DataContext is used by some controls for data binding and can act as a tag value
        ///     in others allowing the caller to put miscellaneous information related to what the widget represents so that the
        ///     widget can be updated later.
        /// </remarks>
        /// <value>The data context.</value>
        [CanBeNull]
        public object DataContext
        {
            get { return _dataContext; }
            set
            {
                if (Equals(value, _dataContext))
                {
                    return;
                }
                _dataContext = value;
                OnPropertyChanged(nameof(DataContext));
            }
        }
    }
}