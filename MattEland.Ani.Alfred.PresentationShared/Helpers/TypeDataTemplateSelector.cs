// ---------------------------------------------------------
// TypeDataTemplateSelector.cs
// 
// Created on:      11/07/2015 at 11:11 PM
// Last Modified:   11/07/2015 at 11:11 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.PresentationAvalon.Helpers
{
    public class TypeDataTemplateSelector : DataTemplateSelector
    {
        public TypeDataTemplateSelector()
        {
            _cache = new Dictionary<string, DataTemplate>();
        }

        [NotNull, ItemNotNull]
        private readonly Dictionary<string, DataTemplate> _cache;

        [CanBeNull]
        private DataTemplate GetTemplateForType(Type t)
        {
            if (t == null) return null;

            var key = t.Name;

            DataTemplate result;
            if (_cache.TryGetValue(key, out result)) return result;

            var resource = Application.Current.TryFindResource(key);
            if (resource != null)
            {
                result = resource as DataTemplate;
            }

            if (result == null)
            {
                var typeInfo = t.GetTypeInfo();

                foreach (var i in typeInfo.ImplementedInterfaces)
                {
                    result = GetTemplateForType(i);
                    if (result != null) break;
                }

                result = GetTemplateForType(typeInfo.BaseType);
            }

            _cache.Add(key, result);

            return result;
        }

        /// <summary>
        ///     When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate"/>
        ///     based on custom logic.
        /// </summary>
        /// <param name="item"> The data object for which to select the template. </param>
        /// <param name="container"> The container. </param>
        /// <returns>
        ///     Returns a <see cref="T:System.Windows.DataTemplate"/> or null. The default value is null.
        /// </returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            return GetTemplateForType(item.GetType());
        }
    }
}