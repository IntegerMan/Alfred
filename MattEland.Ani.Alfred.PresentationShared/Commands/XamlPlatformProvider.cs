// ---------------------------------------------------------
// XamlPlatformProvider.cs
// 
// Created on:      08/08/2015 at 5:55 PM
// Last Modified:   08/08/2015 at 5:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.PresentationShared.Commands
{
    /// <summary>
    ///     A platform provider that provides WPF/XAML compliant objects
    /// </summary>
    public sealed class XamlPlatformProvider : IPlatformProvider
    {
        /// <summary>
        ///     Creates a collection of the specified type.
        /// </summary>
        /// <typeparam name="T">The type the collection will contain</typeparam>
        /// <returns>The collection</returns>
        public ICollection<T> CreateCollection<T>()
        {
            return new ObservableCollection<T>();
        }
    }
}