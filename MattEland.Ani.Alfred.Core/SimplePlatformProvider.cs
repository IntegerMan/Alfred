// ---------------------------------------------------------
// SimplePlatformProvider.cs
// 
// Created on:      07/26/2015 at 4:32 PM
// Last Modified:   08/03/2015 at 1:57 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// A simplistic platform provider for testing purposes. This class cannot be inherited.
    /// </summary>
    public sealed class SimplePlatformProvider : IPlatformProvider
    {
        /// <summary>
        /// Generates a collection of the specified type.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>The collection.</returns>
        [NotNull]
        public ICollection<T> CreateCollection<T>()
        {
            return new Collection<T>();
        }
    }
}