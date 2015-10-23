// ---------------------------------------------------------
// DispatcherUpdatePump.cs
// 
// Created on:      10/22/2015 at 11:52 PM
// Last Modified:   10/22/2015 at 11:52 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows.Threading;

using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     An update pump that runs on the Dispatcher and invokes an action every tick.
    /// </summary>
    public sealed class DispatcherUpdatePump
    {

        [NotNull]
        private readonly Observable<bool> _isRunning;
        /// <summary>
        ///     The dispatcher timer that triggers the update.
        /// </summary>
        [NotNull]
        private readonly DispatcherTimer _timer;

        /// <summary>
        ///     The workspace.
        /// </summary>
        [NotNull]
        private readonly Observable<Action> _updateAction;

        /// <summary>
        ///     Initializes a new instance of the DispatcherUpdatePump class.
        /// </summary>
        /// <param name="updateFrequency"> The update frequency. </param>
        /// <param name="updateAction"> The action to take when the timer is triggered. </param>
        public DispatcherUpdatePump(TimeSpan updateFrequency, [CanBeNull] Action updateAction)
        {
            //- Set up private fields
            _updateAction = new Observable<Action>(updateAction);
            _isRunning = new Observable<bool>(false);

            // Set up the timer
            _timer = BuildTimer(updateFrequency);
        }

        /// <summary>
        ///     Sets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///     true if this instance is running, false if not.
        /// </value>
        [NotNull]
        public bool IsRunning
        {
            [DebuggerStepThrough]
            get
            { return _isRunning; }
            set
            {
                //- Early exit if no change
                if (IsRunning == value) return;

                // Set internal state
                _isRunning.Value = value;

                // Start or stop depending on what we are now
                if (value)
                {
                    _timer.Start();
                }
                else
                {
                    _timer.Stop();
                }
            }
        }

        /// <summary>
        ///     The action to take when the update pump is triggered.
        /// </summary>
        [CanBeNull]
        public Action UpdateAction
        {
            [DebuggerStepThrough]
            get
            { return _updateAction.Value; }

            [DebuggerStepThrough]
            set
            { _updateAction.Value = value; }
        }

        /// <summary>
        ///     Builds the Dispatcher timer.
        /// </summary>
        /// <param name="updateFrequency"> The update frequency. </param>
        /// <returns>
        ///     A DispatcherTimer.
        /// </returns>
        [NotNull]
        private DispatcherTimer BuildTimer(TimeSpan updateFrequency)
        {
            const DispatcherPriority Priority = DispatcherPriority.Normal;
            var dispatcher = Dispatcher.CurrentDispatcher;

            /* Build the timer. We'll point it to our own method instead of action for consolidated
               error handling / logging in the future */

            return new DispatcherTimer(updateFrequency, Priority, OnTimerTick, dispatcher);
        }

        /// <summary>
        ///     Raises the timer tick event.
        /// </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        private void OnTimerTick(object sender, EventArgs e)
        {
            /* Invoke the action we were given. This may throw exceptions. That's fine for now, 
               but eventually some logging would be nice. */

            var action = _updateAction.Value;
            action?.Invoke();
        }

        /// <summary>
        ///     Starts / resumes the update pump.
        /// </summary>
        public void Start()
        {
            IsRunning = true;
        }

        /// <summary>
        ///     Stops the update pump.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
        }
    }
}