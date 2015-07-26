using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// Handles the <see cref="E:UnhandledException" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Let the user know what happened and give them a chance to let the app crash or continue on.
            var result = MessageBox.Show(
                $"An unhandled exception occurred:\r\r{e.Exception.Message}\r\rDo you want to continue running this application?",
                "Unhandled Error",
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
