using System;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A module that keeps track of time and date information and presents it to the user.
    /// </summary>
    public sealed class AlfredTimeModule : AlfredModule
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredModule" />
        ///     class.
        /// </summary>
        /// <param
        ///     name="collectionProvider">
        ///     The collection provider.
        /// </param>
        public AlfredTimeModule([NotNull] ICollectionProvider collectionProvider) : base(collectionProvider)
        {
            CurrentDateWidget = new TextWidget();
            CurrentTimeWidget = new TextWidget();
            BedtimeAlertWidget = new WarningWidget { IsVisible = false, Text = "Shouldn't we be heading to bed soon?" };
            BedtimeHour = 21;
            BedtimeMinute = 30;
            AlertDurationInHours = 4;
            IsAlertEnabled = true;
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public override string Name { get { return "Time Module"; } }

        /// <summary>
        ///     Gets the current time user interface widget.
        /// </summary>
        /// <value>The current time user interface widget.</value>
        [NotNull]
        public TextWidget CurrentTimeWidget { get; }


        /// <summary>
        /// Gets the current date user interface widget.
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
        public WarningWidget BedtimeAlertWidget { get; }

        protected override void InitializeProtected()
        {
            RegisterWidget(CurrentDateWidget);
            RegisterWidget(CurrentTimeWidget);
            RegisterWidget(BedtimeAlertWidget);
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
        /// Updates the module given the specified time.
        /// </summary>
        /// <param name="time">The time. Date portions of this will be ignored.</param>
        public void Update(DateTime time)
        {
            // Always update the date
            CurrentDateWidget.Text = time.ToString("D", CultureInfo.CurrentCulture); // Thursday April 10, 2008

            // Always update the time
            CurrentTimeWidget.Text = $"The time is now {time.ToString("t", CultureInfo.CurrentCulture)}";

            // Update the visibility of the alert widget based on time of day
            UpdateBedtimeAlertVisbility(time);
        }

        /// <summary>
        /// Updates the bedtime alert visibility based on the given time.
        /// </summary>
        /// <param name="time">The time.</param>
        private void UpdateBedtimeAlertVisbility(DateTime time)
        {

            var alertVisible = false;

            // Only do alert visibility calculations if the thing is even enabled.
            if (IsAlertEnabled)
            {

                // Figure out when the alarm display should end. Accept values >= 24 for now.
                // We'll adjust this in a few blocks when checking for early morning circumstances.
                var bedtimeAlertEndHour = BedtimeHour + AlertDurationInHours;

                if (time.Hour == BedtimeHour)
                {
                    if (time.Minute >= BedtimeMinute)
                    {
                        alertVisible = true;
                    }
                }
                else if (time.Hour > BedtimeHour)
                {

                    // Check for when we're on the hour the alert will expire
                    if (time.Hour == bedtimeAlertEndHour && time.Minute < BedtimeMinute)
                    {
                        alertVisible = true;
                    }
                    else if (time.Hour < bedtimeAlertEndHour)
                    {
                        alertVisible = true;
                    }
                }

                // Next we'll check early morning carry over for late alerts so 
                // make sure the end hour is representable on a 24 hour clock.
                if (bedtimeAlertEndHour >= 24)
                {
                    bedtimeAlertEndHour -= 24;
                }

                // Support scenarios of a 9 PM bedtime but it's 12:30 AM.
                if (time.Hour <= bedtimeAlertEndHour)
                {
                    alertVisible = true;
                }
            }

            // Finally stick the value in the widget
            BedtimeAlertWidget.IsVisible = alertVisible;
        }

        /// <summary>
        /// Gets or sets the bedtime hour.  Defaults to 9 PM
        /// </summary>
        /// <value>The bedtime hour.</value>
        public int BedtimeHour { get; set; }

        /// <summary>
        /// Gets or sets the bedtime minute. Defaults to 30 minutes past the hour.
        /// </summary>
        /// <value>The bedtime minute.</value>
        public int BedtimeMinute { get; set; }

        /// <summary>
        /// Gets or sets the alert duration in hours.
        /// </summary>
        /// <value>The alert duration in hours.</value>
        public int AlertDurationInHours { get; set; }

        /// <summary>
        /// Gets or sets whether the alert is enabled.
        /// </summary>
        /// <value>The alert is enabled.</value>
        public bool IsAlertEnabled { get; set; }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            CurrentTimeWidget.Text = null;
        }

    }
}