// ---------------------------------------------------------
// AlfredToolWindowControl.xaml.cs
// 
// Created on:      08/20/2015 at 9:45 PM
// Last Modified:   08/20/2015 at 10:51 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Windows;
using System.Windows.Markup;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Common;

namespace MattEland.Ani.Alfred.VisualStudio
{

    /// <summary>
    ///     Interaction logic for AlfredToolWindowControl.
    /// </summary>
    public partial class AlfredToolWindowControl : IUserInterfaceDirector
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredToolWindowControl" /> class.
        /// </summary>
        /// <exception cref="XamlParseException">
        ///     If a XamlParseException was encountered, it will be logged and
        ///     rethrown to the Visual Studio Host.
        /// </exception>
        public AlfredToolWindowControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (XamlParseException xex)
            {
                // Give some clue as to what went wrong
                MessageBox.Show("XAML Parse Exception",
                                xex.BuildExceptionDetailsMessage(),
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                // We shouldn't load the page in a bad state. Crash the application
                throw;
            }
        }

        /// <summary>
        ///     Handles the page navigation command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Whether or not the command was handled</returns>
        public bool HandlePageNavigationCommand(ShellCommand command)
        {
            if (command.Data.HasText() && tabPages?.Items != null)
            {
                foreach (var item in tabPages.Items)
                {
                    var page = item as IAlfredPage;

                    if (page != null && page.Id.Matches(command.Data))
                    {
                        tabPages.SelectedItem = page;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}