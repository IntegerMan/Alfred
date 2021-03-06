﻿// ---------------------------------------------------------
// AlfredTimeModule.cs
// 
// Created on:      07/26/2015 at 1:46 PM
// Last Modified:   08/10/2015 at 10:52 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A module that keeps track of time and date information and presents it to the user.
    /// </summary>
    public sealed class AlfredTimeModule : AlfredModule
    {
        /// <summary>
        ///     The hour changed event's title message used for logging purposes.
        /// </summary>
        public const string HourAlertEventTitle = "Time.HourAlert";

        /// <summary>
        ///     The half hour alert event title log message
        /// </summary>
        public const string HalfHourAlertEventTitle = "Time.HalfHourAlert";

        private DateTime _time;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="AlfredModule" />
        ///     class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        public AlfredTimeModule([NotNull] IAlfredContainer container) : base(container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            CurrentDateWidget = new TextWidget(BuildWidgetParameters(@"lblCurrentDate"));
            CurrentTimeWidget = new TextWidget(BuildWidgetParameters(@"lblCurrentTime"));

            AlertWidget = new WarningWidget(BuildWidgetParameters(@"warnAlert"))
            {
                IsVisible = false,
                Text = Resources.AlfredTimeModule_AlfredTimeModule_BedtimeNagMessage
            };

            AlertHour = 21;
            AlertMinute = 30;
            AlertDurationInHours = 4;
            IsAlertEnabled = true;

            CurrentTime = DateTime.MinValue;
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public override string Name
        {
            get { return Resources.AlfredTimeModule_Name.NonNull(); }
        }

        /// <summary>
        ///     Gets the current time user interface widget.
        /// </summary>
        /// <value>The current time user interface widget.</value>
        [NotNull]
        public TextWidget CurrentTimeWidget { get; }

        /// <summary>
        ///     Gets the current date user interface widget.
        /// </summary>
        /// <value>The current date user interface widget.</value>
        [NotNull]
        public TextWidget CurrentDateWidget { get; }

        /// <summary>
        ///     Gets the bedtime alert widget.
        ///     This widget alerts users to go to bed after a certain time
        ///     and will be hidden until the user needs to go to bed.
        /// </summary>
        /// <value>The bedtime alert widget.</value>
        [NotNull]
        public WarningWidget AlertWidget { get; }

        /// <summary>
        ///     Gets or sets the bedtime hour.  Defaults to 9 PM
        /// </summary>
        /// <value>The bedtime hour.</value>
        public int AlertHour { get; set; }

        /// <summary>
        ///     Gets or sets the bedtime minute. Defaults to 30 minutes past the hour.
        /// </summary>
        /// <value>The bedtime minute.</value>
        public int AlertMinute { get; set; }

        /// <summary>
        ///     Gets or sets the alert duration in hours.
        /// </summary>
        /// <value>The alert duration in hours.</value>
        public int AlertDurationInHours { get; set; }

        /// <summary>
        ///     Gets or sets whether the alert is enabled.
        /// </summary>
        /// <value>The alert is enabled.</value>
        public bool IsAlertEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the last time recorded
        /// </summary>
        /// <value>The current time.</value>
        private DateTime CurrentTime
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            Register(CurrentDateWidget);
            Register(CurrentTimeWidget);
            Register(AlertWidget);

            // Ensure it has some initial values so it doesn't "blink" or lag on start
            ClearLastTimeRun();
            Update(DateTime.Now);
        }

        /// <summary>
        ///     Handles updating the module
        /// </summary>
        protected override void UpdateProtected()
        {
            // Call a new method with the current time. This helps this module be testable
            Update(DateTime.Now);
        }

        /// <summary>
        ///     Updates the module given the specified time.
        /// </summary>
        /// <param name="time">
        ///     The time. Date portions of this will be ignored.
        /// </param>
        public void Update(DateTime time)
        {
            // Always update the date
            CurrentDateWidget.Text = time.ToString("D", CultureInfo.CurrentCulture); // Thursday April 10, 2008

            // Always update the time
            var timeFormat = Resources.AlfredTimeModule_Update_CurrentTimeDisplayString.NonNull();
            var timeText = string.Format(CultureInfo.CurrentCulture, timeFormat, time);
            CurrentTimeWidget.Text = timeText;

            // Update the visibility of the alert widget based on time of day
            UpdateBedtimeAlertVisibility(time);

            // Check to see if it's now a new hour and, if so, log it to the console
            if (CurrentTime > DateTime.MinValue)
            {
                if (time.Minute == 0 && CurrentTime.Hour != time.Hour)
                {
                    // Let the user know it's now X
                    timeText.Log(HourAlertEventTitle, LogLevel.ChatNotification, Container);
                }
                else if (time.Minute == 30 && CurrentTime.Minute != 30)
                {
                    // Let the user know it's now half after X
                    timeText.Log(HalfHourAlertEventTitle, LogLevel.ChatNotification, Container);
                }
            }

            // Store the last time we processed so it can be referenced next iteration
            CurrentTime = time;
        }

        /// <summary>
        ///     Updates the bedtime alert visibility based on the given time.
        /// </summary>
        /// <param name="time">
        ///     The time.
        /// </param>
        private void UpdateBedtimeAlertVisibility(DateTime time)
        {
            if (!IsAlertEnabled)
            {
                // Easy call; if it's disabled, don't show it
                AlertWidget.IsVisible = false;
            }
            else
            {
                // Farm off the more complicated math to a dedicated class
                AlertWidget.IsVisible = AlertVisibilityCalculator.CalculateAlertVisibility(time.Hour,
                                                                                           time.Minute,
                                                                                           AlertHour,
                                                                                           AlertMinute,
                                                                                           AlertDurationInHours);
            }
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            CurrentTimeWidget.Text = null;
            ClearLastTimeRun();
        }

        /// <summary>
        ///     Clears the last time the module was run.
        /// </summary>
        /// <remarks>
        ///     This is intended for use in time-sensitive unit testing
        ///     scenarios and is not intended for any normal usage
        /// </remarks>
        public void ClearLastTimeRun()
        {
            CurrentTime = DateTime.MinValue;
        }
    }
}