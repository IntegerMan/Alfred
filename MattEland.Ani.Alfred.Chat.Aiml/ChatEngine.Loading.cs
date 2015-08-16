// ---------------------------------------------------------
// ChatEngine.Loading.cs
// 
// Created on:      08/16/2015 at 12:51 AM
// Last Modified:   08/16/2015 at 12:51 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    public partial class ChatEngine
    {
        /// <summary>
        /// Holds the application's directory at startup for expediting loading values
        /// </summary>
        [NotNull]
        private readonly string _startDirectory;

        [NotNull]
        private string _aimlDirectoryPath;

        [NotNull]
        private readonly AimlLoader _aimlLoader;

        [NotNull]
        public string AimlDirectoryPath
        {
            get
            {
                return _aimlDirectoryPath;
            }
            set { _aimlDirectoryPath = value; }
        }

        public void LoadAimlFromDirectory()
        {
            _aimlLoader.LoadAiml(AimlDirectoryPath);
        }

        public void LoadAimlFromDirectory(string directoryPath)
        {
            _aimlLoader.LoadAiml(directoryPath);
        }

        public void LoadAimlFile(XmlDocument newAIML, string filename)
        {
            _aimlLoader.LoadAimlFromXml(newAIML, filename);
        }

        /// <summary>
        /// Loads settings from the specified settings directory path.
        /// There is assumed to be a settings.xml file in this directory.
        /// </summary>
        /// <param name="settingsDirectoryPath">The settings directory path.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IOException"><paramref name="settingsDirectoryPath" /> is <see langword="null" />.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="UnauthorizedAccessException">Access to <paramref name="settingsDirectoryPath" /> is denied.</exception>
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
        /// Gets a value indicating whether input in AIML files should be trusted. 
        /// If false the input will go through the full normalization process.
        /// </summary>
        /// <value>Whether or not AIML files are trusted.</value>
        public bool TrustAiml { get; } = true;

        [UsedImplicitly]
        public void SaveToBinaryFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            var fileStream = File.Create(path);
            new BinaryFormatter().Serialize(fileStream, RootNode);
            fileStream.Close();
        }

        [UsedImplicitly]
        public void LoadFromBinaryFile(string path)
        {
            var fileStream = File.OpenRead(path);
            RootNode = (Node)new BinaryFormatter().Deserialize(fileStream);
            fileStream.Close();
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
            if (string.IsNullOrEmpty(path))
            {
                Log(string.Format(Locale,
                                  "Attempted to load a new category with an empty pattern where the directoryPath = {0} and template = {1} produced by a category in the file: {2}",
                                  path,
                                  node.OuterXml,
                                  filename),
                    LogLevel.Warning);
                return;
            }

            // Add it to the graph
            RootNode.AddCategory(path, node.OuterXml, filename);
        }

    }
}