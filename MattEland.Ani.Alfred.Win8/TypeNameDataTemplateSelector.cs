using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Win8
{
    /// <summary>
    /// Looks up data templates by type name since Win8 doesn't support DataTypes on DataTemplates
    /// </summary>
    public class TypeNameDataTemplateSelector : DataTemplateSelector
    {
        [NotNull]
        private readonly Dictionary<string, DataTemplate> _cachedDataTemplates = new Dictionary<string, DataTemplate>();

        /// <summary>
        /// Fallback value for DataTemplate
        /// </summary>
        public string DefaultTemplateKey { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            // Grab the Type name
            var key = DefaultTemplateKey;

            var type = item?.GetType();

            if (type?.Name != null)
            {
                key = $"{type.Name.Split('.').Last()}";
            }

            var dt = GetCachedDataTemplate(key);

            try
            {
                if (dt != null) { return dt; }

                // look at all parents (visual parents)
                var fe = container as FrameworkElement;
                while (fe != null)
                {
                    dt = FindTemplate(fe, key);
                    if (dt != null) { return dt; }
                    // if you were to just look at logical parents,
                    // you'd find that there isn't a Parent for Items set
                    fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
                }

                dt = FindTemplate(null, key);
                return dt;
            }
            finally
            {
                if (dt != null)
                {
                    AddCachedDataTemplate(key, dt);
                }
            }
        }

        private DataTemplate GetCachedDataTemplate(string key)
        {
            return _cachedDataTemplates.ContainsKey(key) ? _cachedDataTemplates[key] : null;
        }

        private void AddCachedDataTemplate(string key, DataTemplate dt)
        {
            _cachedDataTemplates[key] = dt;
        }

        /// <summary>
        /// Returns a template
        /// </summary>
        /// <param name="source">Pass null to search entire app</param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static DataTemplate FindTemplate(object source, string key)
        {
            var fe = source as FrameworkElement;

            object obj;

            Debug.Assert(Application.Current != null, "Application.Current != null");

            var rd = fe != null ? fe.Resources : Application.Current.Resources;

            if (rd == null || !rd.TryGetValue(key, out obj))
            {
                return null;
            }

            var dt = obj as DataTemplate;
            return dt;
        }
    }
}