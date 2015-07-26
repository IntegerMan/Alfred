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

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            _alfred.Initialize();
        }
    }
}