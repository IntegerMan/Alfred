using System;
using System.Collections.Generic;
using System.Linq;
using Assisticant;
using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     The view model locator that helps views find appropriate view models.
    /// </summary>
    public class ViewModelLocator : ViewModelLocatorBase
    {
        /// <summary>
        ///     The default number of MFDs present.
        /// </summary>
        private const int DefaultMFDCount = 4;

        [NotNull]
        private readonly Workspace _workspace;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelLocator"/> class.
        /// </summary>
        public ViewModelLocator()
        {
            _workspace = LoadWorkspace();
        }

        /// <summary>
        ///     Gets the main view model.
        /// </summary>
        /// <value>
        ///     The main view model.
        /// </value>
        [NotNull]
        public MainViewModel Main
        {
            get { return ViewModel(() => new MainViewModel(_workspace)); }
        }

        [NotNull]
        private Workspace LoadWorkspace()
        {
            // TODO: Load your document here.
            var workspace = new Workspace();

            for (int index = 0; index < DefaultMFDCount; index++)
            {
                var mfd = workspace.NewMFD();

                if (DesignMode)
                {
                    ConfigureDesignMFD(mfd, index);
                }
                else
                {
                    ConfigureMFD(mfd, index);
                }
            }

            return workspace;
        }

        /// <summary>
        ///     Configures a multifunction display.
        /// </summary>
        /// <param name="mfd"> The mfd. </param>
        /// <param name="index"> The zero-based index of the multifunction display. </param>
        private void ConfigureMFD([NotNull] MultifunctionDisplay mfd, int index)
        {
            mfd.Name = string.Format("MFD {0}", index + 1);
        }

        /// <summary>
        ///     Configures a multifunction display.
        /// </summary>
        /// <param name="mfd"> The mfd. </param>
        /// <param name="index"> The zero-based index of the multifunction display. </param>
        private void ConfigureDesignMFD([NotNull] MultifunctionDisplay mfd, int index)
        {
            mfd.Name = string.Format("MFD {0}", index + 1);
        }

    }
}
