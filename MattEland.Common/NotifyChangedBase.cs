// ---------------------------------------------------------
// NotifyChangedBase.cs
// 
// Created on:      09/03/2015 at 1:31 PM
// Last Modified:   09/03/2015 at 1:35 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace MattEland.Common
{
    /// <summary>
    /// Provides capabilities for handling <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public abstract class NotifyChangedBase : INotifyPropertyChanged, IExceptionCallbackHandler
    {
        /// <summary>
        ///     Occurs when a property changes.
        /// </summary>
        [CanBeNull]
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Called when a property change event occurs.
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
            catch (Exception exception)
            {
                if (!HandleCallbackException(exception, $"Property Changed: '{propertyName}'"))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    // ReSharper disable once ThrowingSystemException
                    throw;
                }
            }
        }

        /// <summary>
        ///     Handles a callback <see cref="Exception"/>.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <param name="operationName"> Name of the operation that was being performed. </param>
        /// <returns>
        ///     <see langword="true"/> if it the <paramref name="exception"/> was handled and should
        ///     not be thrown again, otherwise false.
        /// </returns>
        public virtual bool HandleCallbackException(
            [NotNull] Exception exception,
            [NotNull] string operationName)
        {
            // By default, don't handle it - inheritors do that as needed

            return false;
        }
    }
}