using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using JetBrains.Annotations;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace MattEland.Ani.Alfred.Win8
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched([NotNull] LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            var rootFrame = Window.Current.Content as Frame ?? CreateRootFrame(e);

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        [NotNull]
        private Frame CreateRootFrame([NotNull] LaunchActivatedEventArgs e)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            var rootFrame = new Frame();

            // Set the default language
            rootFrame.Language = ApplicationLanguages.Languages[0];

            rootFrame.NavigationFailed += OnNavigationFailed;

            /*
            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }
            */

            // Place the frame in the current Window
            var window = Window.Current;
            if (window != null)
            {
                window.Content = rootFrame;
            }

            return rootFrame;
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, [NotNull] NavigationFailedEventArgs e)
        {
            var pageType = e.SourcePageType;

            var exception = e.Exception;

            Debug.Assert(exception != null, "exception != null");

            if (pageType != null)
            {
                throw new InvalidOperationException($"Failed to load Page {pageType.FullName}: {exception.Message}");
            }

            throw new InvalidOperationException($"Unknown navigation failure: {exception.Message}");
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, [NotNull] SuspendingEventArgs e)
        {
            var operation = e.SuspendingOperation;
            var deferral = operation?.GetDeferral();

            //TODO: Save application state and stop any background activity

            deferral?.Complete();
        }
    }
}
