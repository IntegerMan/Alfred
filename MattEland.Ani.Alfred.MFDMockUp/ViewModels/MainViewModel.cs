using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MattEland.Ani.Alfred.MFDMockUp.Models;
using Assisticant;

using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    public sealed class MainViewModel
    {

        [NotNull]
        private readonly Workspace _workspace;

        [NotNull]
        private readonly ViewModelLocator _locator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        public MainViewModel([NotNull] ViewModelLocator locator, [NotNull] Workspace workspace)
        {
            _workspace = workspace;
            _locator = locator;
        }

        /// <summary>
        ///     Gets the title for the application
        /// </summary>
        /// <value>
        ///     The application title.
        /// </value>
        public string AppTitle
        {
            get { return _workspace.Name; }
        }

        public IEnumerable<MFDViewModel> MultifunctionDisplays
        {
            get
            {
                return
                    from item in _workspace.MFDs
                    select new MFDViewModel(_locator, item);
            }
        }

        /// <summary>
        ///     Starts the application.
        /// </summary>
        public void StartApplication()
        {
            _workspace.Start();
        }
    }
}
