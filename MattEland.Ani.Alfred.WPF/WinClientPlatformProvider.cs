using System.Collections.Generic;
using System.Collections.ObjectModel;

using MattEland.Ani.Alfred.Core;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    /// A collection provider that provides WPF preferred Observable Collections
    /// </summary>
    public sealed class WinClientPlatformProvider : IPlatformProvider
    {
        public ICollection<T> CreateCollection<T>()
        {
            return new ObservableCollection<T>();
        }
    }
}