// ---------------------------------------------------------
// IAlfredComponent.cs
// 
// Created on:      08/09/2015 at 6:25 PM
// Last Modified:   08/09/2015 at 6:25 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     An abstract component of Alfred
    /// </summary>
    public interface IAlfredComponent : IPropertyProvider
    {
        /// <summary>
        ///     Gets the name and version of the component.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        string NameAndVersion { get; }

        /// <summary>
        ///     Gets the status of the component.
        /// </summary>
        /// <value>The status.</value>
        AlfredStatus Status { get; }

        /// <summary>
        ///     Updates this instance.
        /// </summary>
        void Update();

        /// <summary>
        ///     Initializes the component.
        /// </summary>
        /// <param name="alfred">The alfred framework.</param>
        void Initialize([CanBeNull] IAlfred alfred);

        /// <summary>
        ///     Called when initialization completes.
        /// </summary>
        void OnInitializationCompleted();

        /// <summary>
        ///     Shuts down this instance.
        /// </summary>
        void Shutdown();

        /// <summary>
        ///     Called when shutdown completes.
        /// </summary>
        void OnShutdownCompleted();

        /// <summary>
        ///     Called when a component is registered with an alfred instance.
        /// </summary>
        /// <param name="alfredInstance">The alfred instance.</param>
        void OnRegistered(IAlfred alfredInstance);
    }
}