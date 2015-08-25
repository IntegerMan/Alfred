// ---------------------------------------------------------
// ChatEngine.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:58 AM
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

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    /// <summary>
    ///     ChatEngine represents an AIML (Artificial Intelligence Markup Language) chat bot and provides
    ///     an access point to initialize the library and process chat requests.
    /// </summary>
    /// <remarks>
    ///     NOTE: This class is now a partial class. Check the other partial classes for more details
    /// </remarks>
    public class ChatEngine
    {
        [NotNull]
        private readonly AimlLoader _aimlLoader;

        [NotNull]
        private readonly ChatProcessor _chatProcessor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatEngine" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ChatEngine([CanBeNull] IConsole logger = null)
        {
            // Get logging online ASAP
            Logger = logger;

            // Set simple properties
            MaxThatSize = 256;

            // Timeout should not occur during debugging as it makes debugging chat difficult
#if DEBUG
            Timeout = -1;
#else
            Timeout = 2000;
#endif
            RootNode = new Node();

            // Build helper components
            _chatProcessor = new ChatProcessor(this);
            _aimlLoader = new AimlLoader(this);
            Librarian = new ChatEngineLibrarian(this);

            // Populate smaller value dictionaries
            SentenceSplitters = new List<string> { ".", "!", "?", ";" };

            // Set the failure message's default value.
            FallbackResponse = Resources.ChatEngineDontUnderstandFallback.NonNull();
        }

        /// <summary>
        ///     Gets the librarian that manages settings.
        /// </summary>
        /// <value>The librarian.</value>
        [NotNull]
        public ChatEngineLibrarian Librarian { get; }

        /// <summary>
        ///     Gets a list of sentence splitters. Sentence splitters are punctuation characters such as . or !
        ///     that define breaks between sentences.
        /// </summary>
        /// <value>The sentence splitters.</value>
        [NotNull]
        [ItemNotNull]
        public ICollection<string> SentenceSplitters { get; }

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        [CanBeNull]
        public IConsole Logger { get; }

        /// <summary>
        ///     Gets the locale of this instance.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
        public CultureInfo Locale
        {
            get { return CultureInfo.CurrentCulture; }
        }

        /// <summary>
        ///     Gets the time out limit for a request in milliseconds. Defaults to 2000 (2 seconds).
        /// </summary>
        /// <value>The time out.</value>
        public double Timeout { get; }

        /// <summary>
        ///     Gets or sets the maximum size that can be used to hold a path in the that value.
        /// </summary>
        /// <value>The maximum size of the that.</value>
        public int MaxThatSize { get; }

        /// <summary>
        ///     Gets or sets the root node of the Aiml knowledge graph.
        /// </summary>
        /// <value>The root node.</value>
        [NotNull]
        public Node RootNode { get; }

        /// <summary>
        ///     Gets the count of AIML nodes in memory.
        /// </summary>
        /// <value>The count of AIML nodes.</value>
        public int NodeCount
        {
            get { return RootNode.Children.Count; }
        }

        /// <summary>
        ///     Gets a value indicating whether input in AIML files should be trusted.
        ///     If false the input will go through the full normalization process.
        /// </summary>
        /// <value>Whether or not AIML files are trusted.</value>
        public bool TrustAiml { get; } = true;

        /// <summary>
        ///     Gets or sets the owner of this chat engine. This can be used by tag handlers to get custom
        ///     information back out of the chat engine to other systems.
        /// </summary>
        /// <value>The owner.</value>
        [CanBeNull]
        public object Owner { get; set; }

        /// <summary>
        /// Gets or sets the response that is given to the user when the input is not understood.
        /// </summary>
        /// <value>The fallback response.</value>
        [NotNull]
        public string FallbackResponse { get; set; }

        /// <summary>
        ///     Logs the specified message to the logger.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        internal void Log([CanBeNull] string message, LogLevel level)
        {
            Logger?.Log("ChatEngine", message, level);
        }

        /// <summary>
        ///     Accepts a chat message from the user and returns the chat engine's reply.
        /// </summary>
        /// <param name="input">The input chat message.</param>
        /// <param name="user">The user.</param>
        /// <returns>A result object containing the engine's reply.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">A chat message is required to interact with the system.</exception>
        public Result Chat([NotNull] string input, [NotNull] User user)
        {
            //- Validate
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (input.IsNullOrWhitespace())
            {
                throw new ArgumentException(Resources.ChatErrorNoMessage, nameof(input));
            }

            // Build a request that we can work with internally.
            return _chatProcessor.ProcessChatRequest(new Request(input, user, this));
        }

        /// <summary>
        ///     Handles a redirect chat request from srai aiml elements.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The result of the request.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="request" /> is <see langword="null" />.
        /// </exception>
        internal Result ProcessRedirectChatRequest([NotNull] Request request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            return _chatProcessor.ProcessChatRequest(request);
        }

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
        [UsedImplicitly]
        public void LoadAimlFromDirectory([NotNull] string directoryPath)
        {
            if (directoryPath == null) { throw new ArgumentNullException(nameof(directoryPath)); }

            _aimlLoader.LoadAiml(directoryPath);
        }

        /// <summary>
        ///     Loads a specific aiml file.
        /// </summary>
        /// <param name="aimlFile">The aiml file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="aimlFile" /> is <see langword="null" />.</exception>
        public void LoadAimlFile([NotNull] XmlDocument aimlFile)
        {
            //- Validate
            if (aimlFile == null) { throw new ArgumentNullException(nameof(aimlFile)); }

            // Load the file
            _aimlLoader.LoadAimlFromXml(aimlFile);
        }

        /// <summary>
        ///     Loads AIML assets from a string containing the contents of an AIML file.
        /// </summary>
        /// <param name="aiml">The aiml.</param>
        /// <exception cref="XmlException">
        ///     There is a load or parse error in the XML. In this case, a
        ///     <see cref="T:System.IO.FileNotFoundException" /> is raised.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="aiml" /> is <see langword="null" />.</exception>
        public void LoadAimlFromString([NotNull] string aiml)
        {
            if (aiml.IsEmpty()) { throw new ArgumentNullException(nameof(aiml)); }

            var document = new XmlDocument();
            document.LoadXml(aiml);

            LoadAimlFile(document);
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
        [UsedImplicitly]
        public void LoadSettingsFromDirectory([NotNull] string settingsDirectoryPath)
        {
            //- Validate
            if (settingsDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(settingsDirectoryPath));
            }

            // Invoke
            Librarian.LoadSettingsFromConfigDirectory(settingsDirectoryPath);
        }

        /// <summary>
        ///     Loads various settings from XML files. Input parameters are all optional and should be the
        ///     actual XML and not a file path.
        /// </summary>
        /// <param name="globalXml">The global XML.</param>
        /// <param name="firstPersonXml">The first person XML.</param>
        /// <param name="secondPersonXml">The second person XML.</param>
        /// <param name="genderXml">The gender XML.</param>
        /// <param name="substitutionsXml">The substitutions XML.</param>
        public void LoadSettingsFromXml([CanBeNull] string globalXml = null,
                                        [CanBeNull] string firstPersonXml = null,
                                        [CanBeNull] string secondPersonXml = null,
                                        [CanBeNull] string genderXml = null,
                                        [CanBeNull] string substitutionsXml = null)
        {
            Librarian.LoadSettingsFromXml(globalXml,
                                          firstPersonXml,
                                          secondPersonXml,
                                          genderXml,
                                          substitutionsXml);
        }

        /// <summary>
        ///     Adds the category to the graph.
        /// </summary>
        /// <param name="node">The template node.</param>
        /// <param name="path">The path.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path" />, or <paramref name="node" /> is <see langword="null" />.
        /// </exception>
        public void AddCategoryToGraph([NotNull] XmlNode node, [NotNull] string path)
        {
            //- Validate
            if (path == null) { throw new ArgumentNullException(nameof(path)); }
            if (node == null) { throw new ArgumentNullException(nameof(node)); }

            // You can't add nodes with an empty path
            if (string.IsNullOrEmpty(path))
            {
                var message = string.Format(Locale,
                                            Resources.ChatEngineAddCategoryErrorNoPath.NonNull(),
                                            path,
                                            node.OuterXml);
                Log(message, LogLevel.Warning);
                return;
            }

            // Add the node to the graph
            RootNode.AddTemplate(path, node.OuterXml);
        }

        /// <summary>
        ///     Logs the specified error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public void Error([CanBeNull] string message)
        {
            Log(message, LogLevel.Error);
        }
    }

}