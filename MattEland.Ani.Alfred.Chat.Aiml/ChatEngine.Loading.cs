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

        public string PathToConfigFiles
        {
            get
            {
                return Path.Combine(_startDirectory,
                                    GlobalSettings.GetValue("configdirectory"));
            }
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

        public void LoadSettings()
        {
            LoadSettings(Path.Combine(_startDirectory,
                                      Path.Combine("config", "Settings.xml")));
        }

        public void LoadSettings(string pathToSettings)
        {
            GlobalSettings.Load(pathToSettings);
            if (!GlobalSettings.Contains("splittersfile"))
            {
                GlobalSettings.Add("splittersfile", "Splitters.xml");
            }
            if (!GlobalSettings.Contains("person2substitutionsfile"))
            {
                GlobalSettings.Add("person2substitutionsfile", "Person2Substitutions.xml");
            }
            if (!GlobalSettings.Contains("personsubstitutionsfile"))
            {
                GlobalSettings.Add("personsubstitutionsfile", "PersonSubstitutions.xml");
            }
            if (!GlobalSettings.Contains("gendersubstitutionsfile"))
            {
                GlobalSettings.Add("gendersubstitutionsfile", "GenderSubstitutions.xml");
            }
            if (!GlobalSettings.Contains("substitutionsfile"))
            {
                GlobalSettings.Add("substitutionsfile", "Substitutions.xml");
            }
            if (!GlobalSettings.Contains("aimldirectory"))
            {
                GlobalSettings.Add("aimldirectory", "aiml");
            }
            if (!GlobalSettings.Contains("configdirectory"))
            {
                GlobalSettings.Add("configdirectory", "config");
            }
            SecondPersonToFirstPersonSubstitutions.Load(Path.Combine(PathToConfigFiles,
                                                   GlobalSettings.GetValue(
                                                                           "person2substitutionsfile")));
            FirstPersonToSecondPersonSubstitutions.Load(Path.Combine(PathToConfigFiles,
                                                  GlobalSettings.GetValue("personsubstitutionsfile")));
            GenderSubstitutions.Load(Path.Combine(PathToConfigFiles,
                                                  GlobalSettings.GetValue("gendersubstitutionsfile")));
            Substitutions.Load(Path.Combine(PathToConfigFiles,
                                            GlobalSettings.GetValue("substitutionsfile")));
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
            /*
            catch
            {
                Log(string.Format(Locale,
                                  "Failed to load a new category into the graphmaster where the directoryPath = {0} and template = {1} produced by a category in the file: {2}",
                                  path,
                                  node.OuterXml,
                                  filename),
                    LogLevel.Error);
            }
            */
        }

    }
}