using System.Collections.Generic;
using System.Collections.ObjectModel;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Win8
{
    /// <summary>
    /// A collection provider that provides Windows 8 XAML preferred Observable Collections
    /// </summary>
    public sealed class Win8ClientPlatformProvider : IPlatformProvider
    {
        // TODO: It'd be great to be able to reuse the same one that WPF uses

        /// <summary>
        /// Creates a collection of the specified type.
        /// </summary>
        /// <typeparam name="T">The type the collection will contain</typeparam>
        /// <returns>The collection</returns>
        [NotNull]
        public ICollection<T> CreateCollection<T>()
        {
            return new ObservableCollection<T>();
        }

        /// <summary>
        ///     Creates a platform-friendly version of an AlfredCommand.
        /// </summary>
        /// <returns>An AlfredCommand.</returns>
        [NotNull]
        public AlfredCommand CreateCommand()
        {
            return new Win8ClientCommand();
        }
    }
}