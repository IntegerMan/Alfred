using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Modules;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MattEland.Ani.Alfred.Win8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const bool AutoInitialize = false;

        [NotNull]
        private readonly AlfredProvider _alfred;

        public MainPage()
        {

            this.InitializeComponent();

            // Create the console first to log what we're doing
            var console = new Win8ClientConsole();
            console.Log("MetroClient.Initialize", "Console is now online.");

            // Create Alfred. It won't be online and running yet, but create it.
            _alfred = new AlfredProvider(new Win8ClientPlatformProvider())
            {
                Console = console
            };

            StandardModuleProvider.AddStandardModules(_alfred);

            console.Log("MetroClient.Initialize", "Alfred instantiated");

            // Data bindings in the UI rely on Alfred
            this.DataContext = _alfred;

            // Set up the update timer
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += OnTimerTick;
            timer.Start();

            if (AutoInitialize)
            {
                _alfred.Initialize();
            }

        }

        /// <summary>
        /// Handles the <see cref="E:TimerTick" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnTimerTick(object sender, object e)
        {
            // If Alfred is online, ask it to update its modules
            if (_alfred.Status == AlfredStatus.Online)
            {
                _alfred.Update();
            }
        }

    }
}
