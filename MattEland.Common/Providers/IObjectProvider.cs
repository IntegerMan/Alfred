// ---------------------------------------------------------
// IObjectProvider.cs
// 
// Created on:      08/27/2015 at 5:44 PM
// Last Modified:   08/27/2015 at 5:44 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Common.Providers
{
    /// <summary>
    ///     Defines a class capable of providing an object
    /// </summary>
    public interface IObjectProvider
    {

        /// <summary>
        ///     Creates an instance of the requested type.
        /// </summary>
        /// <param name="requestedType">The type that was requested.</param>
        /// <param name="args">The arguments</param>
        /// <returns>A new instance of the requested type</returns>
        [CanBeNull]
        object CreateInstance([NotNull] Type requestedType, [CanBeNull] params object[] args);
    }
}