// ---------------------------------------------------------
// AimlLoader.cs
// 
// Created on:      08/12/2015 at 10:25 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    /// A class used for building AIML resources
    /// </summary>
    public class AimlLoader
    {
        [NotNull]
        private readonly ChatEngine _chatEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AimlLoader"/> class.
        /// </summary>
        /// <param name="chatEngine">The chat engine.</param>
        /// <exception cref="System.ArgumentNullException">chatEngine</exception>
        public AimlLoader([NotNull] ChatEngine chatEngine)
        {
            if (chatEngine == null)
            {
                throw new ArgumentNullException(nameof(chatEngine));
            }

            _chatEngine = chatEngine;
        }

        /// <summary>
        /// Loads AIML resources from the directory listed in ChatEngine.AimlDirectoryPath.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">ChatEngine.AimlDirectoryPath was not set</exception>
        public void LoadAiml()
        {
            var pathToAiml = _chatEngine.AimlDirectoryPath;

            if (string.IsNullOrEmpty(pathToAiml))
            {
                throw new InvalidOperationException("ChatEngine.AimlDirectoryPath was not set");
            }

            LoadAiml(pathToAiml);
        }

        /// <summary>
        /// Loads AIML resources from a file.
        /// </summary>
        /// <param name="directoryPath">The path to the directory containing the .AIML files.</param>
        public void LoadAiml([NotNull] string directoryPath)
        {
            //- Parameter validation
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (!Directory.Exists(directoryPath))
            {
                var message = string.Format(Locale, "The directory specified as the directoryPath to the AIML files ({0}) cannot be found by the AimlLoader object. Please make sure the directory where you think the AIML files are to be found is the same as the directory specified in the settings file.", directoryPath);
                throw new FileNotFoundException(message);
            }

            // Grab all files in the directory that should meet our needs
            Log("Starting to process AIML files found in the directory " + directoryPath);

            var files = Directory.GetFiles(directoryPath, "*.aiml");
            if (files.Length <= 0)
            {
                throw new FileNotFoundException(string.Format(Locale, "Could not find any .aiml files in the specified directory ({0}). Please make sure that your aiml file end in a lowercase aiml extension, for example - myFile.aiml is valid but myFile.AIML is not.", directoryPath));
            }

            // Load each file we've found
            foreach (var filename in files)
            {
                if (filename != null)
                {
                    LoadAimlFile(filename);
                }
            }

            Log(string.Format(Locale, "Finished processing the AIML files. {0} categories processed.", Convert.ToString(_chatEngine.Size)));
        }

        /// <summary>
        /// Gets the locale we're using for the chat engine.
        /// </summary>
        /// <value>The locale.</value>
        public CultureInfo Locale
        {
            get
            {
                return _chatEngine.Locale ?? CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        /// Logs the specified message to the log.
        /// </summary>
        /// <param name="message">The message.</param>
        private void Log(string message)
        {
            if (message != null)
            {
                _chatEngine.writeToLog(message);
            }
        }

        /// <summary>
        /// Loads AIML resources from a file with the specified directoryPath.
        /// </summary>
        /// <param name="path">The directoryPath.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void LoadAimlFile([NotNull] string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            _chatEngine.writeToLog("Processing AIML file: " + path);

            // Load the document. Loads of XmlExceptions can be thrown here
            var doc = new XmlDocument();
            doc.Load(path);

            // Load the Aiml resources from the document
            LoadAimlFromXml(doc, path);
        }

        /// <summary>
        /// Loads the AIML from an XML Document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="filename">The directoryPath.</param>
        public void LoadAimlFromXml([NotNull] XmlDocument doc, [NotNull] string filename)
        {
            //- Validate
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            // Grab the nodes from the document
            var nodes = doc.DocumentElement?.ChildNodes.Cast<XmlNode>();
            if (nodes == null)
            {
                return;
            }

            //- Handle each node in turn
            foreach (var node in nodes)
            {
                if (node == null)
                    continue;

                // At the root level we support Topics and Categories
                switch (node.Name.ToUpperInvariant())
                {
                    case "TOPIC":
                        ProcessTopic(node, filename);
                        break;

                    case "CATEGORY":
                        ProcessCategory(node, "*", filename);
                        break;
                }
            }
        }

        /// <summary>
        /// Processes a topic node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="filename">The filename.</param>
        private void ProcessTopic([NotNull] XmlNode node, [NotNull] string filename)
        {
            //- Validation
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            // Loop through child categories and process them
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode != null && childNode.Name.Compare("category"))
                {
                    ProcessCategory(childNode, GetNameFromNode(node), filename);
                }
            }
        }

        /// <summary>
        /// Gets a name from node's name attribute defaulting to "*" when name is not found.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The name value</returns>
        [NotNull]
        private static string GetNameFromNode([NotNull] XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var nameValue = "*";
            var attributes = node.Attributes;
            if (attributes != null && attributes.Count == 1 & attributes[0]?.Name == "name")
            {
                // Grab the name from the node and use that as our topic
                var nameAttribute = attributes["name"];
                if (nameAttribute?.Value != null)
                {
                    nameValue = nameAttribute.Value;
                }

            }

            return nameValue;
        }

        /// <summary>
        /// Processes a category node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="filename">The filename.</param>
        private void ProcessCategory([NotNull] XmlNode node, [NotNull] string topicName, [NotNull] string filename)
        {
            //- Validation
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (topicName == null)
            {
                throw new ArgumentNullException(nameof(topicName));
            }
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            // Get the pattern node
            var patternNode = FindChildNode("pattern", node);
            if (patternNode == null)
            {
                throw new XmlException(string.Format(Locale, "Missing pattern tag in a node found in {0}", filename));
            }

            // Get the template node
            var templateNode = FindChildNode("template", node);
            if (Equals(null, templateNode))
            {
                throw new XmlException(string.Format(Locale, "Missing template tag in the node with pattern: {0} found in {1}", patternNode.InnerText, filename));
            }

            // Figure out our path for logging and validation purposes
            var path = BuildPathString(node, topicName, false);
            if (path.Length > 0)
            {
                try
                {
                    var graphmaster = _chatEngine.Graphmaster;
                    graphmaster.AddCategory(path, templateNode.OuterXml, filename);

                    _chatEngine.Size++;
                }
                catch
                {
                    Log(string.Format(Locale, "ERROR! Failed to load a new category into the graphmaster where the directoryPath = {0} and template = {1} produced by a category in the file: {2}", path, templateNode.OuterXml, filename));
                }
            }
            else
            {
                Log(string.Format(Locale, "WARNING! Attempted to load a new category with an empty pattern where the directoryPath = {0} and template = {1} produced by a category in the file: {2}", path, templateNode.OuterXml, filename));
            }
        }

        /// <summary>
        /// Builds the path string from a node given a topic name.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="isUserInput">The is user input.</param>
        /// <returns>The path string</returns>
        [NotNull]
        public string BuildPathString([NotNull] XmlNode node, [NotNull] string topicName, bool isUserInput)
        {
            //- Validation
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (topicName == null)
            {
                throw new ArgumentNullException(nameof(topicName));
            }

            // Get the pattern from the node
            var patternNode = FindChildNode("pattern", node);
            var pattern = patternNode?.InnerText ?? string.Empty;

            // Get the "that" value from the node
            var thatNode = FindChildNode("that", node);
            var that = thatNode?.InnerText ?? "*";

            // Delegate path building
            return BuildPathString(pattern, that, topicName, isUserInput);
        }

        /// <summary>
        /// Finds a child node with the specified name from the node specified.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="node">The node.</param>
        /// <returns>System.Xml.XmlNode.</returns>
        [CanBeNull]
        private static XmlNode FindChildNode([NotNull] string name, [NotNull] XmlNode node)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            return node.ChildNodes.Cast<XmlNode>().FirstOrDefault(xmlNode => xmlNode?.Name == name);
        }

        /// <summary>
        /// Builds a directoryPath string representing a compound state involving a pattern, "that" value, and topic.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="that">The that value.</param>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="isUserInput">Whether or not this is user input.</param>
        /// <returns>A directoryPath string representing the pattern, that, and topicName values.</returns>
        [NotNull]
        public string BuildPathString([NotNull] string pattern, [NotNull] string that,
                                           [NotNull] string topicName, bool isUserInput)
        {
            //- Validate inputs
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }
            if (that == null)
            {
                throw new ArgumentNullException(nameof(that));
            }
            if (topicName == null)
            {
                throw new ArgumentNullException(nameof(topicName));
            }

            // Determine if we'll sanitize input or not
            var trustInput = _chatEngine.TrustAiml & !isUserInput;

            // Build pattern string
            pattern = trustInput ? pattern.Trim() : Normalize(pattern, isUserInput).Trim();
            if (string.IsNullOrEmpty(pattern))
            {
                return string.Empty;
            }

            // Build "that" display string
            that = trustInput ? that.Trim() : Normalize(that, isUserInput).Trim();
            if (string.IsNullOrEmpty(that))
            {
                that = "*";
            }
            else if (that.Length > _chatEngine.MaxThatSize)
            {
                that = "*";
            }

            // Build Topic display string
            topicName = trustInput ? topicName.Trim() : Normalize(topicName, isUserInput).Trim();
            if (string.IsNullOrEmpty(topicName))
            {
                topicName = "*";
            }

            // Build and return the Path String
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(pattern);
            stringBuilder.Append(" <that> ");
            stringBuilder.Append(that);
            stringBuilder.Append(" <topic> ");
            stringBuilder.Append(topicName);

            return stringBuilder.ToString();
        }

        public string Normalize(string input, bool isUserInput)
        {
            var stringBuilder = new StringBuilder();
            var applySubstitutions = new ApplySubstitutions(_chatEngine);
            var illegalCharacters = new StripIllegalCharacters(_chatEngine);

            var transformedSubstitutions = applySubstitutions.Transform(input).Split(" \r\n\t".ToCharArray());

            foreach (var input1 in transformedSubstitutions)
            {
                var str = !isUserInput
                              ? (input1 == "*" || input1 == "_" ? input1 : illegalCharacters.Transform(input1))
                              : illegalCharacters.Transform(input1);
                stringBuilder.Append(str.Trim() + " ");
            }

            return stringBuilder.ToString().Replace("  ", " ");
        }
    }
}