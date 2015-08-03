using System.Collections.Generic;
using System.Collections.ObjectModel;

using MattEland.Ani.Alfred.Core;

namespace MattEland.Ani.Alfred.Win8
{
    /// <summary>
    /// A collection provider that provides Windows 8 XAML preferred Observable Collections
    /// </summary>
    public sealed class Win8ClientPlatformProvider : IPlatformProvider
    {
        // TODO: It'd be great to be able to reuse the same one that WPF uses

        public ICollection<T> CreateCollection<T>()
        {
            return new ObservableCollection<T>();
        }
    }
}