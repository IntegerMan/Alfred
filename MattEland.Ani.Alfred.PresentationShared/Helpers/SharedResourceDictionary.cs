﻿// ---------------------------------------------------------
// SharedResourceDictionary.cs
// 
// Created on:      08/21/2015 at 1:41 AM
// Last Modified:   08/21/2015 at 1:44 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.PresentationShared.Helpers
{
    /// <summary>
    ///     A resource dictionary that does not load its content multiple times when imported into multiple
    ///     files.
    /// </summary>
    /// <remarks>
    ///     Adapted from http://www.wpftutorial.net/MergedDictionaryPerformance.html
    /// </remarks>
    public class SharedResourceDictionary : ResourceDictionary
    {
        /// <summary>
        ///     Internal cache of loaded dictionaries
        /// </summary>
        [NotNull]
        public static readonly Dictionary<Uri, ResourceDictionary> SharedDictionaries =
            new Dictionary<Uri, ResourceDictionary>();

        /// <summary>
        ///     The source for the resource dictionary
        /// </summary>
        private Uri _source;

        /// <summary>
        ///     Gets or sets the uniform resource identifier (URI) to load resources from.
        /// </summary>
        /// <exception cref="ArgumentNullException"
        ///            accessor="set">
        ///     <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        public new string Source
        {
            get { return _source?.OriginalString; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                // Build out the URI. This makes it easier for the resource file to be manipulated in XAML.
                var uri = new Uri(value, UriKind.RelativeOrAbsolute);

                _source = uri;

                if (!SharedDictionaries.ContainsKey(uri))
                {
                    // If the dictionary is not yet loaded, load it by setting
                    // the source of the base class
                    base.Source = uri;

                    // add it to the cache
                    SharedDictionaries.Add(uri, this);
                }
                else
                {
                    // If the dictionary is already loaded, get it from the cache
                    MergedDictionaries.Add(SharedDictionaries[uri]);
                }
            }
        }
    }
}