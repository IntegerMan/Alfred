// ---------------------------------------------------------
// AlertVisibilityCalculator.cs
// 
// Created on:      08/10/2015 at 10:44 PM
// Last Modified:   08/11/2015 at 1:47 AM
// Original author: Matt Eland
// ---------------------------------------------------------

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     Used to calculate when alarms should be displayed
    /// </summary>
    internal static class AlertVisibilityCalculator
    {
        /// <summary>
        ///     Calculates the alert visibility based on the inputed time.
        /// </summary>
        /// <param name="hour">The current hour.</param>
        /// <param name="minute">The current minute.</param>
        /// <param name="alertHour">The alert hour.</param>
        /// <param name="alertMinute">The alert minute.</param>
        /// <param name="alertDurationInHours">The alert duration in hours.</param>
        /// <returns><c>true</c> if the alert is visible, <c>false</c> otherwise.</returns>
        internal static bool CalculateAlertVisibility(int hour,
                                                      int minute,
                                                      int alertHour,
                                                      int alertMinute,
                                                      int alertDurationInHours)
        {
            if (hour == alertHour)
            {
                return minute >= alertMinute;
            }

            if (hour > alertHour)
            {
                return CalculateVisibilityAfterAlarmHour(hour, minute, alertHour, alertMinute, alertDurationInHours);
            }

            // Next we'll check early morning carryover for late alerts (e.g. 4 hour alarm display at 10 PM)
            // so make sure the end hour is representable on a 24 hour clock.
            var alertEndHour = alertHour + alertDurationInHours;
            while (alertEndHour >= 24)
            {
                alertEndHour -= 24;
            }

            // Support scenarios of a 9 PM bedtime but it's 12:30 AM.
            return hour <= alertEndHour;

        }

        /// <summary>
        ///     Calculates the alert visibility when the current hour is after the alarm hour.
        /// </summary>
        /// <param name="hour">The current hour.</param>
        /// <param name="minute">The current minute.</param>
        /// <param name="alertHour">The alert hour.</param>
        /// <param name="alertMinute">The alert minute.</param>
        /// <param name="alertDurationInHours">The alert duration in hours.</param>
        /// <returns><c>true</c> if the alert should be visible, <c>false</c> otherwise.</returns>
        /// <remarks>
        ///     This is a method to help break down the complexity of CalculateAlertVisibility.
        ///     Times come in this way to allow for testable scenarios.
        /// </remarks>
        private static bool CalculateVisibilityAfterAlarmHour(int hour,
                                                              int minute,
                                                              int alertHour,
                                                              int alertMinute,
                                                              int alertDurationInHours)
        {
            // Check for when we're on the hour the alert will expire
            var alertEndHour = (alertHour + alertDurationInHours);

            if (hour == alertEndHour && minute < alertMinute)
            {
                return true;
            }

            return hour < alertEndHour;
        }
    }
}