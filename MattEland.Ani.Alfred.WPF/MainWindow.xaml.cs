using System.ComponentModel;
using System.Windows;

using MattEland.Ani.Alfred.Core;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AlfredProvider _alfred;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _alfred = new AlfredProvider
            {
                Console = new SimpleConsole()
            };

            this.DataContext = _alfred;
        }

        /// <summary>
        /// Handles the <see cref="E:WindowLoaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Tell Alfred that everything is configured and ready to go. Doing this after window loaded (vs in constructor) will help reduce UI latency
            _alfred.Initialize();
        }

        private void OnExitClicked(object sender, RoutedEventArgs e)
        {
            // TODO: A command based model would be better
            this.Close();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // TODO: Shut down Alfred
        }
    }
}