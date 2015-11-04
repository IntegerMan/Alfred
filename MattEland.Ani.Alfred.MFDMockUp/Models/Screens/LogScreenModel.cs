// ---------------------------------------------------------
// LogScreenModel.cs
// 
// Created on:      11/02/2015 at 9:29 PM
// Last Modified:   11/02/2015 at 9:29 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data model for the log screen. This class cannot be inherited.
    /// </summary>
    public sealed class LogScreenModel : ScreenModel
    {
        /// <summary>
        ///     The console.
        /// </summary>
        [NotNull]
        private readonly IConsole _console;

        /// <summary>
        ///     The known events collection.
        /// </summary>
        [NotNull]
        private readonly ObservableCollection<IConsoleEvent> _loggedEvents;

        /// <summary>
        ///     The multifunction display.
        /// </summary>
        [NotNull]
        private readonly MultifunctionDisplay _mfd;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="console"> The console. </param>
        /// <param name="mfd"> The multifunction display. </param>
        public LogScreenModel([NotNull] IConsole console, [NotNull] MultifunctionDisplay mfd) : base("LOG")
        {
            Contract.Requires(console != null);
            Contract.Requires(mfd != null);

            _console = console;
            _loggedEvents = new ObservableCollection<IConsoleEvent>();
            _mfd = mfd;
        }

        /// <summary>
        ///     Contains invariants related to this class.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_mfd != null);
            Contract.Invariant(_console != null);
            Contract.Invariant(_loggedEvents != null);
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor, MFDProcessorResult processorResult)
        {
            // Check for new events
            foreach (var consoleEvent in _console.Events)
            {
                if (!_loggedEvents.Contains(consoleEvent))
                {
                    _loggedEvents.Add(consoleEvent);
                }
            }
        }

        /// <summary>
        ///     Gets the logged events.
        /// </summary>
        /// <value>
        ///     The logged events.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<IConsoleEvent> LoggedEvents
        {
            [DebuggerStepThrough]
            get
            { return _loggedEvents; }
        }

    }
}