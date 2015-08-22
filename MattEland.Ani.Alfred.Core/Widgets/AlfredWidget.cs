// ---------------------------------------------------------
// AlfredWidget.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/22/2015 at 12:48 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     Any sort of user interface widget for representing a module.
    ///     Widgets can range from simple text to interactive components.
    ///     Widgets do not contain user interface elements but tell the
    ///     client what user interface elements to create.
    /// </summary>
    public abstract class AlfredWidget : INotifyPropertyChanged
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
        ///     in others allowing the caller to put miscellaneous information related to what the widget
        ///     represents so that the
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

        /// <summary>
        ///     Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        [SuppressMessage("ReSharper", "CatchAllClause")]
        protected void OnPropertyChanged([CanBeNull] string propertyName)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                var message = string.Format(CultureInfo.CurrentCulture,
                                            "Error encountered changing property '{0}' :{1}",
                                            propertyName,
                                            ex.GetBaseException());

                Error("Widget.PropertyChanged", message);
            }
        }

        /// <summary>
        ///     Handles a widget error.
        /// </summary>
        /// <param name="header">The error header.</param>
        /// <param name="message">The error message.</param>
        protected void Error([NotNull] string header, [NotNull] string message)
        {
            // TODO: It'd be very good to get this to Alfred's console

            var format = string.Format(CultureInfo.CurrentCulture, "{0}: {1}", header, message);
            Debug.WriteLine(format);

            Debugger.Break();
        }
    }
}