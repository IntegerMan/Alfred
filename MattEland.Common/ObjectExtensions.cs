// ---------------------------------------------------------
// ObjectExtensions.cs
// 
// Created on:      08/23/2015 at 2:16 PM
// Last Modified:   08/23/2015 at 2:18 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Common
{
    /// <summary>
    /// Extension methods for all classes.
    /// </summary>
    public static class ObjectExtensions
    {

        /// <summary>
        ///     Attempts to call <see cref="IDisposable.Dispose" /> on the object by casting it to a
        ///     disposable. If the item is not IDisposable, no action will be taken.
        /// </summary>
        /// <param name="item">The item.</param>
        public static void TryDispose([CanBeNull] this object item)
        {
            var disposable = item as IDisposable;
            disposable?.Dispose();
        }
    }
}