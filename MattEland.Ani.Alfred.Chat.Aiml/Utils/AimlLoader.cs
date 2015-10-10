// ---------------------------------------------------------
// AimlLoader.cs
// 
// Created on:      08/22/2015 at 11:36 PM
// Last Modified:   09/06/2015 at 11:01 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     A class used for building AIML resources
    /// </summary>
    internal sealed class AimlLoader
    {
        /// <summary>
        ///     The chat engine.
        /// </summary>
        [NotNull]
        private readonly ChatEngine _chatEngine;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlLoader" /> class.
        /// </summary>
        /// <param name="chatEngine">The chat engine.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        internal AimlLoader([NotNull] ChatEngine chatEngine)
        {
            if (chatEngine == null) { throw new ArgumentNullException(nameof(chatEngine)); }

            _chatEngine = chatEngine;
        }

        /// <summary>
        ///     Gets the locale we're using for the chat engine.
        /// </summary>
        /// <value>
        /// The locale.
        /// </value>
        private CultureInfo Locale
        {
            get { return _chatEngine.Locale; }
        }

        /// <summary>
        ///     Loads AIML resources from a file.
        /// </summary>
        /// <param name="directoryPath">
        /// The path to the directory containing the .AIML files.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="directoryPath" /> is <see langword="null" /> .
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The directory specified does not exist.
        /// </exception>
        public void LoadAiml([NotNull] string directoryPath)
        {
            //- Parameter validation
            if (directoryPath == null) { throw new ArgumentNullException(nameof(directoryPath)); }

            if (!Directory.Exists(directoryPath))
            {
                var message = string.Format(Locale,
                                            @"The directory specified as the directoryPath to the AIML files ({0}) cannot be found by the AimlLoader object. Please make sure the directory where you think the AIML files are to be found is the same as the directory specified in the settings file.",
                                            directoryPath);
                throw new DirectoryNotFoundException(message);
            }

            // Grab all files in the directory that should meet our needs
            Log(string.Format(Locale,
                              @"Starting to process AIML files found in the directory {0}",
                              directoryPath),
                LogLevel.Verbose);

            var files = Directory.GetFiles(directoryPath, "*.aiml");
            if (files.Length <= 0)
            {
                Log(string.Format(Locale,
                                  @"Could not find any .aiml files in the specified directory ({0}). Please make sure that your aiml file end in a lowercase aiml extension, for example - myFile.aiml is valid but myFile.AIML is not.",
                                  directoryPath),
                    LogLevel.Error);

                return;
            }

            // Load each file we've found
            foreach (var filename in files) { LoadAimlFileSafe(filename); }

            Log(string.Format(Locale,
                              @"Finished processing the AIML files. {0} categories processed.",
                              _chatEngine.NodeCount),
                LogLevel.Verbose);
        }

        /// <summary>
        ///     Loads an AIML file safely, logging exceptions as they're encountered.
        /// </summary>
        /// <param name="filename">Filename of the file.</param>
        private void LoadAimlFileSafe([CanBeNull] string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) { return; }

            try
            {
                LoadAimlFile(filename);
            }
            catch (XmlException)
            {
                Log(string.Format(Locale, "XML error reading file {0}", filename), LogLevel.Error);
            }
            catch (IOException ex)
            {
                Log(string.Format(Locale, "IO error reading file {0} {1}", filename, ex.Message),
                    LogLevel.Error);
            }
            catch (SecurityException ex)
            {
                Log(
                    string.Format(Locale,
                                  "Security error reading file {0} {1}",
                                  filename,
                                  ex.Message),
                    LogLevel.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log(
                    string.Format(Locale,
                                  "Unauthorized access exception reading file {0} {1}",
                                  filename,
                                  ex.Message),
                    LogLevel.Error);
            }
        }

        /// <summary>
        ///     Logs the specified <paramref name="message" /> to the log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        private void Log([CanBeNull] string message, LogLevel level)
        {
            if (message != null) { _chatEngine.Log(message, level); }
        }

        /// <summary>
        ///     Loads AIML resources from a file with the specified directoryPath.
        /// </summary>
        /// <param name="path">The directoryPath.</param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="XmlException">
        /// There is a load or parse error in the XML. In this case, a
        /// <see cref="FileNotFoundException" /> is raised.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified <paramref name="path" /> is invalid (for example, it is on an unmapped
        /// drive).
        /// </exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path" /> specified a file that is read-only.-or- This operation is not
        /// supported on the current platform.-or- <paramref name="path" /> specified a
        /// directory.-or- The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The file specified in <paramref name="path" /> was not found.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public void LoadAimlFile([NotNull] string path)
        {
            if (path == null) { throw new ArgumentNullException(nameof(path)); }

            Log(string.Format(Locale, Resources.AimlLoaderProcessingFile.NonNull(), path),
                LogLevel.Verbose);

            // Load the document. Loads of XmlExceptions can be thrown here
            var doc = new XmlDocument();
            doc.Load(path);

            // Load the Aiml resources from the document
            LoadAimlFromXml(doc);
        }

        /// <summary>
        ///     Loads the AIML from an XML Document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="doc" /> is <see langword="null" /> .
        /// </exception>
        public void LoadAimlFromXml([NotNull] XmlDocument doc)
        {
            //- Validate
            if (doc == null) { throw new ArgumentNullException(nameof(doc)); }

            // Grab the nodes from the document
            var nodes = doc.DocumentElement?.ChildNodes.Cast<XmlNode>();
            if (nodes == null) { return; }

            //- Handle each node in turn
            foreach (var node in nodes)
            {
                if (node == null) { continue; }

                // At the root level we support Topics and Categories
                switch (node.Name.ToUpperInvariant())
                {
                    case "TOPIC":
                        ProcessTopic(node);
                        break;

                    case "CATEGORY":
                        ProcessCategory(node, "*");
                        break;
                }
            }
        }

        /// <summary>
        ///     Processes a topic node.
        /// </summary>
        /// <param name="node">The node.</param>
        private void ProcessTopic([NotNull] XmlNode node)
        {
            //- Validation
            if (node == null) { throw new ArgumentNullException(nameof(node)); }

            // Loop through child categories and process them
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode != null && childNode.Name.Matches("category"))
                {
                    ProcessCategory(childNode, GetNameFromNode(node));
                }
            }
        }

        /// <summary>
        ///     Gets a name from node's name attribute defaulting to "*" when name is not found.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The name value</returns>
        [NotNull]
        private static string GetNameFromNode([NotNull] XmlNode node)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }

            var nameValue = "*";
            var attributes = node.Attributes;
            if (attributes != null && attributes.Count == 1 & attributes[0]?.Name == "name")
            {
                // Grab the name from the node and use that as our topic
                var nameAttribute = attributes["name"];
                if (nameAttribute?.Value != null) { nameValue = nameAttribute.Value; }
            }

            return nameValue;
        }

        /// <summary>
        ///     Processes a category <paramref name="node" /> and adds it to the ChatEngine.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="topicName">Name of the topic.</param>
        /// <exception cref="ArgumentNullException">node, <paramref name="topicName" /></exception>
        private void ProcessCategory([NotNull] XmlNode node, [NotNull] string topicName)
        {
            //- Validation
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            if (topicName == null) { throw new ArgumentNullException(nameof(topicName)); }

            // GetValue the pattern node
            var patternNode = FindChildNode("pattern", node);
            if (patternNode == null)
            {
                throw new XmlException(string.Format(Locale,
                                                     "Missing pattern tag in a node found in topic {0}",
                                                     topicName));
            }

            // GetValue the template node
            var templateNode = FindChildNode("template", node);
            if (Equals(null, templateNode))
            {
                throw new XmlException(string.Format(Locale,
                                                     "Missing template tag in the node with pattern: {0} found in topic {1}",
                                                     patternNode.InnerText,
                                                     topicName));
            }

            // Figure out our path for logging and validation purposes
            var path = BuildPathString(node, topicName, false);
            _chatEngine.AddCategoryToGraph(templateNode, path);
        }

        /// <summary>
        ///     Builds the path string from a <paramref name="node" /> given a topic name.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="isUserInput">The is user input.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="node" /> is <see langword="null" /> .
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="topicName" /> is <see langword="null" /> .
        /// </exception>
        /// <returns>The path string</returns>
        [NotNull]
        public string BuildPathString(
            [NotNull] XmlNode node,
            [NotNull] string topicName,
            bool isUserInput)
        {
            //- Validation
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            if (topicName == null) { throw new ArgumentNullException(nameof(topicName)); }

            // GetValue the pattern from the node
            var patternNode = FindChildNode("pattern", node);
            var pattern = patternNode?.InnerText ?? string.Empty;

            // GetValue the "that" value from the node
            var thatNode = FindChildNode("that", node);
            var that = thatNode?.InnerText ?? "*";

            // Delegate path building
            return BuildPathString(pattern, that, topicName, isUserInput);
        }

        /// <summary>
        ///     <para>
        ///         Finds a child <paramref name="node" /> with the specified <paramref name="name" />
        ///     </para>
        ///     <para>from the <paramref name="node" /> specified.</para>
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="node">The node.</param>
        /// <returns>System.Xml.XmlNode.</returns>
        [CanBeNull]
        private static XmlNode FindChildNode([NotNull] string name, [NotNull] XmlNode node)
        {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }

            if (node == null) { throw new ArgumentNullException(nameof(node)); }

            return node.ChildNodes.Cast<XmlNode>().FirstOrDefault(xmlNode => xmlNode?.Name == name);
        }

        /// <summary>
        ///     Builds a directoryPath string representing a compound state involving a pattern,
        ///     "that" value, and topic.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="that">The that value.</param>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="isUserInput">Whether or not this is user input.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> is <see langword="null" /> .
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="that" /> is <see langword="null" /> .
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="topicName" /> is <see langword="null" /> .
        /// </exception>
        /// <returns>
        ///     A directoryPath string representing the pattern, that, and
        ///     <paramref name="topicName" /> values.
        /// </returns>
        [NotNull]
        internal string BuildPathString(
            [NotNull] string pattern,
            [NotNull] string that,
            [NotNull] string topicName,
            bool isUserInput)
        {
            // TODO: This method has a high cyclomatic complexity and should be refactored

            //- Validate inputs
            if (pattern == null) { throw new ArgumentNullException(nameof(pattern)); }
            if (that == null) { throw new ArgumentNullException(nameof(that)); }
            if (topicName == null) { throw new ArgumentNullException(nameof(topicName)); }

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
            if (string.IsNullOrEmpty(that) || that.Length > _chatEngine.MaxThatSize)
            {
                that = "*";
            }

            // Build Topic display string
            topicName = trustInput ? topicName.Trim() : Normalize(topicName, isUserInput).Trim();
            if (string.IsNullOrEmpty(topicName)) { topicName = "*"; }

            // Build and return the Path String
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(pattern);
            stringBuilder.Append(" <that> ");
            stringBuilder.Append(that);
            stringBuilder.Append(" <topic> ");
            stringBuilder.Append(topicName);

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Normalizes the <paramref name="input" /> by stripping out illegal characters and
        ///     applying common substitutions.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="isUserInput">The is user input.</param>
        /// <returns>The normalized <paramref name="input" /></returns>
        [NotNull]
        private string Normalize([CanBeNull] string input, bool isUserInput)
        {
            // Do common substitutions
            input = TextSubstitutionHelper.Substitute(_chatEngine.Librarian.Substitutions, input);

            // Grab the words in the input
            const string WordBoundaries = " \r\n\t";
            var words = input.Split(WordBoundaries.ToCharArray());

            // Loop through each word found and append it to the output string
            var stringBuilder = new StringBuilder();
            var illegalCharacters = new SanitizingTextTransformer(_chatEngine);

            foreach (var word in words)
            {
                //- Sanity check
                if (string.IsNullOrEmpty(word)) { continue; }

                // Sanitize the input keeping in mind that this could be a dividing wildcard character
                string result;
                if (isUserInput)
                {
                    result = illegalCharacters.Transform(word);
                }
                else
                {
                    const string Wildcards = "*_";
                    result = Wildcards.Contains(word) ? word : illegalCharacters.Transform(word);
                }

                //- Add it to the output
                stringBuilder.AppendFormat(Locale, "{0} ", result.Trim());
            }

            //- Send the result back
            return stringBuilder.ToString().Replace("  ", " ");
        }
    }
}