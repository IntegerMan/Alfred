using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// An interface that promises the ability to build collections. This lets each platform provide their preferred (observable) collections.
    /// </summary>
    public interface ICollectionProvider
    {
        /// <summary>
        /// Generates a collection of the specified type.
        /// </summary>
        /// <returns>The collection.</returns>
        [NotNull]
        ICollection<T> CreateCollection<T>();
    }
}