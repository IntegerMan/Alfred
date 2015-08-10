// ---------------------------------------------------------
// IStatusController.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:20 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     The Status Controller provides a way to control Alfred's status
    /// </summary>
    public interface IStatusController
    {
        /// <summary>
        ///     Gets or sets the alfred framework.
        /// </summary>
        /// <value>The alfred framework.</value>
        IAlfred Alfred { get; set; }

        /// <summary>
        ///     Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is already Online
        /// </exception>
        void Initialize();

        /// <summary>
        ///     Tells Alfred to go ahead and shut down.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is already Offline
        /// </exception>
        void Shutdown();
    }
}