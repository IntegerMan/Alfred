using System;

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
            CurrentTimeWidget = new TextWidget();
            BedtimeAlertWidget = new WarningWidget { IsVisible = false };
        }

        /// <summary>
        ///     Gets the name and version of the module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public override string NameAndVersion
        {
            get { return "Time 0.1 Alpha"; }
        }

        /// <summary>
        ///     Gets the current time user interface widget.
        /// </summary>
        /// <value>The current time user interface widget.</value>
        [NotNull]
        public TextWidget CurrentTimeWidget { get; }

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
            // Always update the time
            CurrentTimeWidget.Text = $"The time is now {time.ToString("t")}";

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

            if (time.Hour == BedtimeHour)
            {
                if (time.Minute >= BedtimeMinute)
                {
                    alertVisible = true;
                }
            }
            else if (time.Hour > BedtimeHour)
            {
                alertVisible = true;
            }

            // The alert is good for a few hours and the alert should be visible during that time.
            var bedtimeAlertEndHour = BedtimeHour + AlertDurationInHours;

            // Ensure we have a representable hour if we wrapped over to the next day.
            if (bedtimeAlertEndHour >= 24)
            {
                bedtimeAlertEndHour -= 24;
            }

            // Support scenarios of a 9 PM bedtime but it's 12:30 AM.
            if (time.Hour <= bedtimeAlertEndHour)
            {
                alertVisible = true;
            }

            // Finally stick the value in the widget
            BedtimeAlertWidget.IsVisible = alertVisible;
        }

        /// <summary>
        /// Gets or sets the bedtime hour.
        /// </summary>
        /// <value>The bedtime hour.</value>
        public int BedtimeHour { get; set; } = 21;

        /// <summary>
        /// Gets or sets the bedtime minute.
        /// </summary>
        /// <value>The bedtime minute.</value>
        public int BedtimeMinute { get; set; } = 30;

        /// <summary>
        /// Gets or sets the alert duration in hours.
        /// </summary>
        /// <value>The alert duration in hours.</value>
        public int AlertDurationInHours { get; set; } = 4;

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            CurrentTimeWidget.Text = null;
        }

    }
}