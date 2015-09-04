// ---------------------------------------------------------
// IHasStatus.cs
// 
// Created on:      09/03/2015 at 1:26 AM
// Last Modified:   09/03/2015 at 1:26 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Defines an entity that has Status and status control capabilities
    /// </summary>
    public interface IHasStatus
    {

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        AlfredStatus Status { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this instance is online.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is online; otherwise, <c>false</c> .
        /// </value>
        bool IsOnline { get; }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Shutdowns this instance.
        /// </summary>
        void Shutdown();

        /// <summary>
        ///     Tells modules to take a look at their content and update as needed.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if Alfred is not Online</exception>
        void Update();
    }
}