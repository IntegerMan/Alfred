// ---------------------------------------------------------
// AlfredWidget.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:33 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Widgets
{

    /// <summary>
    ///     Any sort of user interface widget for representing a module.
    ///     Widgets can range from simple text to interactive components.
    ///     Widgets do not contain user interface elements but tell the
    ///     client what user interface elements to create.
    /// </summary>
    public abstract class WidgetBase : IWidget
    {

        [CanBeNull]
        private object _dataContext;

        private bool _isVisible = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WidgetBase" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="parameters"> The creation parameters. </param>
        protected WidgetBase([NotNull] WidgetCreationParameters parameters)
        {
            if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }

            Name = parameters.Name;
            Console = parameters.Console;
            Locale = parameters.Locale;
        }

        /// <summary>
        ///     Gets the logging console.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets the locale.
        /// </summary>
        /// <value>The locale.</value>
        public CultureInfo Locale
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets or sets whether or not the widget is visible. This defaults to <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if this widget is visible; otherwise, <c>false</c>.</value>
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible) { return; }
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
                if (Equals(value, _dataContext)) { return; }
                _dataContext = value;
                OnPropertyChanged(nameof(DataContext));
            }
        }

        /// <summary>
        ///     Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Gets the display name for use in the user interface.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        ///     Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public abstract string ItemTypeName { get; }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     Gets a list of properties provided by this item.
        /// </summary>
        /// <returns>The properties</returns>
        public virtual IEnumerable<IPropertyItem> Properties
        {
            get
            {
                yield return new AlfredProperty("Visible", IsVisible);
            }
        }

        /// <summary>
        ///     Gets the property providers.
        /// </summary>
        /// <value>The property providers.</value>
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { yield break; }
        }

        /// <summary>
        ///     Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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
                var message = string.Format(Locale,
                                            "Error encountered changing property '{0}' :{1}",
                                            propertyName,
                                            ex.GetBaseException());

                Error("Widget.PropertyChanged", message);
            }
        }

        /// <summary>
        ///     Handles a widget error by logging it to the console.
        /// </summary>
        /// <param name="header">The error header.</param>
        /// <param name="message">The error message.</param>
        protected void Error([NotNull] string header, [NotNull] string message)
        {
            Console?.Log(header, message, LogLevel.Error);
        }

        /// <summary>
        ///     Logs a message to the console if a console is configured.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level. Defaults to Verbose.</param>
        protected void Log(
            [NotNull] string header,
            [NotNull] string message,
            LogLevel level = LogLevel.Verbose)
        {
            Console?.Log(header, message, level);
        }
    }
}