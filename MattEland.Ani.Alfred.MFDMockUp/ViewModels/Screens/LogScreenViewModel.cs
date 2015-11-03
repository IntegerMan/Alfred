// ---------------------------------------------------------
// LogScreenViewModel.cs
// 
// Created on:      11/02/2015 at 9:41 PM
// Last Modified:   11/02/2015 at 9:41 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the log screen. This class cannot be inherited.
    /// </summary>
    [ViewModelFor(typeof(LogScreenModel))]
    public sealed class LogScreenViewModel
    {
        [NotNull]
        private readonly LogScreenModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public LogScreenViewModel([NotNull] LogScreenModel model)
        {
            Contract.Requires(model != null);

            _model = model;
        }

        /// <summary>
        ///     Gets the log entries.
        /// </summary>
        /// <value>
        ///     The log entries.
        /// </value>
        public IEnumerable<LogEntryViewModel> LogEntries
        {
            get { yield break; }
        }
    }

}