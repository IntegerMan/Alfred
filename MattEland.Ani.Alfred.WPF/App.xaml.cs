// ---------------------------------------------------------
// App.xaml.cs
// 
// Created on:      07/25/2015 at 11:55 PM
// Last Modified:   08/04/2015 at 2:25 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Threading;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App
    {
        /// <summary>
        ///     Handles the
        ///     <see
        ///         cref="E:UnhandledException" />
        ///     event.
        /// </summary>
        /// <param
        ///     name="sender">
        ///     The sender.
        /// </param>
        /// <param
        ///     name="e">
        ///     The
        ///     <see
        ///         cref="DispatcherUnhandledExceptionEventArgs" />
        ///     instance containing the event data.
        /// </param>
        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Let the user know what happened and give them a chance to let the app crash or continue on.
            var message = WPF.Properties.Resources.App_OnUnhandledException_Message;
            if (e == null || message == null || e.Exception == null)
            {
                return;
            }

            message = string.Format(CultureInfo.CurrentCulture, message, e.Exception.Message);

            var caption = WPF.Properties.Resources.App_OnUnhandledException_Unhandled_Error;

            var result = MessageBox.Show(
                                         message,
                                         caption,
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Error);

            // Mark it as handled so the app can keep on going.
            if (result == MessageBoxResult.Yes)
            {
                e.Handled = true;
            }
        }
    }
}