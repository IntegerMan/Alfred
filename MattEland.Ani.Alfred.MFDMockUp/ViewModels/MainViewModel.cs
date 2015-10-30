using System.Collections.Generic;
using System.Linq;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     The main application view model. This class cannot be inherited.
    /// </summary>
    [PublicAPI]
    public sealed class MainViewModel
    {

        [NotNull]
        private readonly Workspace _workspace;

        [NotNull]
        private readonly ViewModelLocator _locator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="locator"> The locator. </param>
        /// <param name="workspace"> The workspace. </param>
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

        /// <summary>
        ///     Gets the multifunction displays.
        /// </summary>
        /// <value>
        ///     The multifunction displays.
        /// </value>
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
