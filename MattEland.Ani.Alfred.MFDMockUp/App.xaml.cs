using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using MattEland.Common;
using System.Diagnostics.CodeAnalysis;

namespace MattEland.Ani.Alfred.MFDMockUp
{
    /// <summary>
    /// The main Alfred MFD Application
    /// </summary>
    public sealed partial class App : Application
    {
        /// <summary>
        ///     Event handler. Called when an unhandled exception occurs in the application.
        /// </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e"> Dispatcher unhandled exception event information. </param>
        [SuppressMessage("Design", "CC0091:Use static method", Justification = "XAML Event Handler")]
        private void App_OnDispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            //- Build a user-readible message
            var message = string.Format("An error occurred. Do you want to continue?\n\n{0}",
                e.Exception.BuildDetailsMessage());

            // Let the user know something went down
            var result = MessageBox.Show(message,
                                         "Application Exception Encountered",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Error);

            // Continue only if the user says so.
            e.Handled = result == MessageBoxResult.Yes;
        }
    }
}
