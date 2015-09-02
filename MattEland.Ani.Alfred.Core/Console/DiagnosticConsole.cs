// ---------------------------------------------------------
// DiagnosticConsole.cs
// 
// Created on:      09/01/2015 at 11:22 PM
// Last Modified:   09/02/2015 at 1:11 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     A diagnostic console for additional debugging and diagnostics in event management.
    /// </summary>
    public sealed class DiagnosticConsole : SimpleConsole
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleConsole" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        public DiagnosticConsole([NotNull] IObjectContainer container) : base(container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            // Log for diagnostics!
            string message =
                $"DiagnosticConsole instantiated. This is instance # {(ConstructedCount + 1)} with container {container}";
            Log("DC.Init", message, LogLevel.Verbose);

            ConstructedCount++;
        }

        /// <summary>
        ///     Gets or sets the number of times a console has been constructed.
        /// </summary>
        /// <value>
        ///     The number of constructed.
        /// </value>
        public static int ConstructedCount { get; private set; }

        /// <summary>
        ///     Logs the specified console event.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="evt"> The console event. </param>
        protected override void Log(IConsoleEvent evt)
        {
            if (evt == null) { throw new ArgumentNullException(nameof(evt)); }

            Debug.WriteLine($"{Container}: {evt.Title}: {evt.Message} @ {evt.Time}. # Events: {EventCount + 1}");

            base.Log(evt);
        }
    }
}