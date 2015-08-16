// ---------------------------------------------------------
// ChatEngine.Loading.cs
// 
// Created on:      08/16/2015 at 12:51 AM
// Last Modified:   08/16/2015 at 2:54 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.IO;
using System.Security;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    public partial class ChatEngine
    {
        [NotNull]
        private readonly AimlLoader _aimlLoader;

        /// <summary>
        ///     Gets a value indicating whether input in AIML files should be trusted.
        ///     If false the input will go through the full normalization process.
        /// </summary>
        /// <value>Whether or not AIML files are trusted.</value>
        public bool TrustAiml { get; } = true;

        /// <summary>
        ///     Loads all .aiml files from a directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="directoryPath" /> is
        ///     <see langword="null" />.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified path is invalid (for example, it is on
        ///     an unmapped drive).
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     Access to <paramref name="directoryPath" /> is
        ///     denied.
        /// </exception>
        /// <exception cref="IOException">There was an I/O related error reading files from the directory.</exception>
        public void LoadAimlFromDirectory([NotNull] string directoryPath)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            _aimlLoader.LoadAiml(directoryPath);
        }

        /// <summary>
        ///     Loads a specific aiml file.
        /// </summary>
        /// <param name="aimlFile">The aiml file.</param>
        /// <param name="filename">The filename.</param>
        /// <exception cref="ArgumentNullException"><paramref name="aimlFile" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="filename" /> is <see langword="null" />.</exception>
        internal void LoadAimlFile([NotNull] XmlDocument aimlFile, [NotNull] string filename)
        {
            //- Validate
            if (aimlFile == null)
            {
                throw new ArgumentNullException(nameof(aimlFile));
            }
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            // Load the file
            _aimlLoader.LoadAimlFromXml(aimlFile, filename);
        }

        /// <summary>
        ///     Loads settings from the specified settings directory path.
        ///     There is assumed to be a settings.xml file in this directory.
        /// </summary>
        /// <param name="settingsDirectoryPath">The settings directory path.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IOException"><paramref name="settingsDirectoryPath" /> is <see langword="null" />.</exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified path is invalid (for example, it is on
        ///     an unmapped drive).
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     Access to <paramref name="settingsDirectoryPath" />
        ///     is denied.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="FileNotFoundException">Could not find a settings file at the given path</exception>
        /// <exception cref="XmlException">The settings file was not found.</exception>
        public void LoadSettingsFromDirectory([NotNull] string settingsDirectoryPath)
        {
            //- Validate
            if (settingsDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(settingsDirectoryPath));
            }

            // Invoke
            Librarian.LoadSettings(settingsDirectoryPath);
        }

        /// <summary>
        ///     Adds the category to the graph.
        /// </summary>
        /// <param name="node">The template node.</param>
        /// <param name="path">The path.</param>
        /// <param name="filename">The filename.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filename" />, <paramref name="path" />, or
        ///     <paramref name="node" /> is <see langword="null" />.
        /// </exception>
        public void AddCategoryToGraph([NotNull] XmlNode node,
                                       [NotNull] string path,
                                       [NotNull] string filename)
        {
            //- Validate
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            // You can't add nodes with an empty path
            if (string.IsNullOrEmpty(path))
            {
                var message = string.Format(Locale,
                                            Resources.ChatEngineAddCategoryErrorNoPath,
                                            path,
                                            node.OuterXml,
                                            filename);
                Log(message, LogLevel.Warning);
                return;
            }

            // Add the node to the graph
            RootNode.AddCategory(path, node.OuterXml, filename);
        }
    }
}