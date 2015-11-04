// ---------------------------------------------------------
// LogEntryViewModel.cs
// 
// Created on:      11/02/2015 at 9:58 PM
// Last Modified:   11/03/2015 at 1:47 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     A ViewModel for a log entry. This class cannot be inherited.
    /// </summary>
    [ViewModelFor(typeof(IConsoleEvent))]
    public sealed class LogEntryViewModel
    {
        /// <summary>
        ///     The console event.
        /// </summary>
        [NotNull]
        private readonly IConsoleEvent _event;

        /// <summary>
        ///     Initializes a new instance of the LogEntryViewModel class.
        /// </summary>
        /// <param name="consoleEvent"> The console event. </param>
        [UsedImplicitly]
        public LogEntryViewModel([NotNull] IConsoleEvent consoleEvent)
        {
            Contract.Requires(consoleEvent != null);
            Contract.Ensures(_event != null);

            _event = consoleEvent;
        }

        /// <summary>
        ///     Gets the title of the event.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        public string Title
        {
            [DebuggerStepThrough]
            get
            {
                return _event.Title;
            }
        }

        /// <summary>
        ///     Gets the event's message.
        /// </summary>
        /// <value>
        ///     The message.
        /// </value>
        public string Message
        {
            [DebuggerStepThrough]
            get
            {
                return _event.Message;
            }
        }

        /// <summary>
        ///     Gets the date/time that this event was created.
        /// </summary>
        /// <value>
        ///     The date/time that this event was created.
        /// </value>
        public DateTime CreatedDateTime
        {
            [DebuggerStepThrough]
            get
            {
                return _event.Time;
            }
        }

        /// <summary>
        ///     Gets the log level.
        /// </summary>
        /// <value>
        ///     The log level.
        /// </value>
        public LogLevel LogLevel
        {
            [DebuggerStepThrough]
            get
            {
                return _event.Level;
            }
        }

        /// <summary>
        ///     Gets the display string.
        /// </summary>
        /// <value>
        ///     The display string.
        /// </value>
        [NotNull]
        public string DisplayString
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return string.Format(Culture,
                                     "{0}: {1} - {2} ({3})",
                                     _event.Time.ToShortTimeString(),
                                     Title,
                                     Message,
                                     LogLevel);
            }
        }

        /// <summary>
        ///     Gets the current culture.
        /// </summary>
        /// <value>
        ///     The culture.
        /// </value>
        [NotNull]
        private static CultureInfo Culture
        {
            get
            {
                Contract.Ensures(Contract.Result<CultureInfo>() != null);

                return CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        ///     Gets the header string for use in a headered content control.
        /// </summary>
        /// <value>
        ///     The header string.
        /// </value>
        public string HeaderString
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return string.Format(Culture,
                                     "{0}: {1} ({2})",
                                     CreatedDateTime.ToShortTimeString(),
                                     Title,
                                     LogLevel);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return DisplayString;
        }
    }
}