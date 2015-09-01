// ---------------------------------------------------------
// SettingsManager.cs
// 
// Created on:      08/12/2015 at 10:28 PM
// Last Modified:   08/17/2015 at 12:21 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     A dictionary for holding setting values
    /// </summary>
    /// <remarks>
    ///     This is a lot of code that seems like it could be replaced with a standard dictionary.
    /// 
    ///     It's odd that it doesn't implement any IEnumerable or ICollection or IDictionary interfaces.
    /// 
    ///     The only unique thing about this dictionary is the upper / lowercase management and the ordered
    ///     nature of the keys
    /// 
    ///     TODO: Clean up this class by using more built in mechanisms
    /// </remarks>
    public class SettingsManager
    {
        [NotNull]
        [ItemNotNull]
        private readonly List<string> _orderedKeys = new List<string>();

        [NotNull]
        private readonly Dictionary<string, string> _settingsHash = new Dictionary<string, string>();

        /// <summary>
        ///     Gets the number of keys in the keys collection.
        /// </summary>
        /// <value>The keys.</value>
        public int Count
        {
            get { return _orderedKeys.Count; }
        }

        /// <summary>
        ///     Gets the setting names as an array.
        /// </summary>
        /// <value>The setting names.</value>
        /// <remarks>TODO: I'd love to remove this or use IEnumerable instead</remarks>
        [NotNull, ItemNotNull]
        public IEnumerable<string> Keys
        {
            get
            {
                var array = new string[_orderedKeys.Count];
                _orderedKeys.CopyTo(array, 0);
                return array;
            }
        }

        /// <summary>
        ///     Clears current values and loads values into the dictionary from a settings file at the
        ///     specified path.
        /// </summary>
        /// <param name="pathToSettings">The path to the settings file.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="XmlException"></exception>
        /// <exception cref="System.ArgumentException">pathToSettings did not have a value</exception>
        /// <exception cref="System.IO.FileNotFoundException">Could not find a settings file at the given path</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     Access to <paramref name="pathToSettings" /> is
        ///     denied.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified path is invalid (for example, it is on
        ///     an unmapped drive).
        /// </exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file. </exception>
        public void Load([NotNull] string pathToSettings)
        {
            if (pathToSettings == null)
            {
                throw new ArgumentNullException(nameof(pathToSettings));
            }
            if (string.IsNullOrWhiteSpace(pathToSettings))
            {
                throw new ArgumentException(Resources.SettingsLoadErrorNoPathToSettings,
                                            nameof(pathToSettings));
            }

            // Verify the settings file exists
            if (!new FileInfo(pathToSettings).Exists)
            {
                throw new FileNotFoundException(Resources.SettingsLoadErrorFileNotFoundException,
                                                pathToSettings);
            }

            // Build out an XML document from the path
            var document = new XmlDocument();
            document.Load(pathToSettings);

            Load(document);
        }

        /// <summary>
        ///     Clears current settings and loads settings from the document.
        /// </summary>
        /// <param name="document">The settings as XML.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        public void Load([NotNull] XmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            Clear();

            var childNodes = document.DocumentElement?.ChildNodes;
            if (childNodes == null)
            {
                return;
            }

            // Grab values from the child nodes
            foreach (XmlNode xmlNode in childNodes)
            {

                //- Sanity Check
                var attributes = xmlNode?.Attributes;
                if (attributes == null || attributes.Count < 2)
                {
                    continue;
                }

                // Grab the name and value and add a node for these values
                if (xmlNode.Name == "item" & attributes.Count == 2 &&
                    attributes[0]?.Name == "name" && attributes[1]?.Name == "value")
                {
                    var name = attributes["name"]?.Value;
                    var value = attributes["value"]?.Value;

                    if (!string.IsNullOrEmpty(name))
                    {
                        Add(name, value);
                    }
                }
            }
        }

        /// <summary>
        ///     Adds an entry with the specified name and value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public void Add([NotNull] string name, [CanBeNull] string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            name = name.ToUpperInvariant();
            Remove(name);

            _orderedKeys.Add(name);
            _settingsHash.Add(name, value);
        }

        /// <summary>
        ///     Removes an entry with the specified name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public void Remove([NotNull] string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            name = name.ToUpperInvariant();

            _orderedKeys.Remove(name);

            RemoveFromHash(name);
        }

        /// <summary>
        ///     Removes the name entry from the settings dictionary.
        /// </summary>
        /// <param name="name">The name.</param>
        private void RemoveFromHash([NotNull] string name)
        {
            _settingsHash.Remove(name.ToUpperInvariant());
        }

        /// <summary>
        ///     Updates the entry under name with the specified value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Update([NotNull] string name, [CanBeNull] string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            name = name.ToUpperInvariant();

            if (!Contains(name))
            {
                //? This seems like it should throw an exception or delegate to Add
                return;
            }

            // Remove it from the list so that when we add it again it'll be at the end
            RemoveFromHash(name);

            // Do a simple add
            _settingsHash.Add(name, value);
        }

        /// <summary>
        ///     Clears the dictionary.
        /// </summary>
        public void Clear()
        {
            _orderedKeys.Clear();
            _settingsHash.Clear();
        }

        /// <summary>
        ///     Gets the value under the specified name or string.Empty if no entry was found.
        /// </summary>
        /// <param name="name">The name of the key.</param>
        /// <returns>The value in the dictionary</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        [NotNull]
        public string GetValue([NotNull] string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            name = name.ToUpperInvariant();

            return Contains(name) ? _settingsHash[name].NonNull() : string.Empty;
        }

        /// <summary>
        ///     Determines whether this dictionary contains an entry under the given name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if there is an entry for name; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public bool Contains([NotNull] string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _orderedKeys.Contains(name.ToUpperInvariant());
        }

        /// <summary>
        ///     Copies the settings in this dictionary into the target dictionary.
        /// </summary>
        /// <param name="target">The target dictionary.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void CopyInto([NotNull] SettingsManager target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            foreach (var name in _orderedKeys)
            {
                target.Add(name, GetValue(name));
            }
        }

        /// <summary>
        ///     Loads files from the specified settings path and logs any encountered errors to the logger.
        ///     This method will not throw exceptions due to failures while loading the dictionary.
        /// </summary>
        /// <param name="pathToSettings">The path to settings.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="locale">The locale.</param>
        /// <exception cref="ArgumentNullException">pathToSettings, locale</exception>
        public void LoadSafe([NotNull] string pathToSettings,
                             [CanBeNull] IConsole logger,
                             [NotNull] CultureInfo locale)
        {
            //- Validate
            if (pathToSettings == null)
            {
                throw new ArgumentNullException(nameof(pathToSettings));
            }
            if (locale == null)
            {
                throw new ArgumentNullException(nameof(locale));
            }

            // Farm out the load to the load method, but handle all expected exceptions by logging.
            const string LogHeader = "LoadSettings";
            try
            {
                Load(pathToSettings);
            }
            catch (XmlException ex)
            {
                logger?.Log(LogHeader,
                            string.Format(locale,
                                          Resources.SettingsLoadErrorXml.NonNull(),
                                          pathToSettings,
                                          ex.Message),
                            LogLevel.Error);
            }
            catch (FileNotFoundException)
            {
                logger?.Log(LogHeader,
                            string.Format(locale,
                                          Resources.SettingsLoadErrorFileNotFound.NonNull(),
                                          pathToSettings),
                            LogLevel.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger?.Log(LogHeader,
                            string.Format(locale,
                                          Resources.SettingsLoadErrorUnauthorized.NonNull(),
                                          pathToSettings,
                                          ex.Message),
                            LogLevel.Error);
            }
            catch (SecurityException ex)
            {
                logger?.Log(LogHeader,
                            string.Format(locale,
                                          Resources.SettignsLoadErrorSecurity.NonNull(),
                                          pathToSettings,
                                          ex.Message),
                            LogLevel.Error);
            }
            catch (IOException ex)
            {
                logger?.Log(LogHeader,
                            string.Format(locale,
                                          Resources.SettingsLoadErrorIOException.NonNull(),
                                          pathToSettings,
                                          ex.Message),
                            LogLevel.Error);
            }
        }

        /// <summary>
        /// Loads settings values from an XML source and logs any exceptions encountered without rethrowing
        /// them.
        /// </summary>
        /// <param name="xml">The XML. This is the actual XML and not a file path.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="locale">The locale.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="xml" /> is <see langword="null" />.</exception>
        public void LoadXmlSafe([NotNull] string xml,
                                [CanBeNull] IConsole logger,
                                [CanBeNull] CultureInfo locale)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }
            try
            {
                LoadXml(xml);
            }
            catch (XmlException ex)
            {
                locale = locale ?? CultureInfo.CurrentCulture;
                logger?.Log("LoadSettingsXml",
                            string.Format(locale,
                                          Resources.LoadSettingsXmlXmlException.NonNull(),
                                          ex.Message),
                            LogLevel.Error);
            }
        }

        /// <summary>
        ///     Loads settings values from an XML source.
        /// </summary>
        /// <param name="xml">The XML. This is the actual XML and not a file path.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="xml" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="XmlException">
        ///     There is a load or parse error in the XML. In this case, the
        ///     document remains empty.
        /// </exception>
        public void LoadXml([NotNull] string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            var document = new XmlDocument();
            document.LoadXml(xml);

            Load(document);
        }
    }
}