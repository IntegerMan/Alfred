// ---------------------------------------------------------
// DesignDataProvider.cs
// 
// Created on:      11/24/2015 at 10:51 PM
// Last Modified:   11/24/2015 at 10:51 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Linq;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes;
using MattEland.Ani.Alfred.MFDMockUp.ViewModels;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A design data provider. This class cannot be inherited.
    /// </summary>
    [PublicAPI]
    public sealed class DesignDataProvider
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public DesignDataProvider()
        {
            Container = new AlfredContainer();
            var locator = new ViewModelLocator(Container);
            ViewModelLocator = locator;

            Workspace = locator.Workspace;
            MultifunctionDisplay = InitializeMFD();
        }

        /// <summary>
        ///     Initializes the multifunction display.
        /// </summary>
        /// <returns>
        ///     The MFD
        /// </returns>
        [NotNull]
        private MultifunctionDisplay InitializeMFD()
        {
            var mfd = Workspace.MFDs.FirstOrDefault()
                                   ?? new MultifunctionDisplay(Container, Workspace, "Design MFD");

            mfd.MasterMode = new SystemMasterMode(MultifunctionDisplay);

            // Build out some basic data
            mfd.Update();

            return mfd;
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public AlfredContainer Container { get; }

        /// <summary>
        ///     Gets the view model locator.
        /// </summary>
        /// <value>
        ///     The view model locator.
        /// </value>
        [NotNull]
        public ViewModelLocator ViewModelLocator { get; }

        /// <summary>
        ///     Gets the multifunction display.
        /// </summary>
        /// <value>
        ///     The multifunction display.
        /// </value>
        [NotNull]
        public MultifunctionDisplay MultifunctionDisplay { get; }

        /// <summary>
        ///     Gets the workspace.
        /// </summary>
        /// <value>
        ///     The workspace.
        /// </value>
        [NotNull]
        public Workspace Workspace { get; }
    }
}