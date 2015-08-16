// ---------------------------------------------------------
// ChatEngine.cs
// 
// Created on:      08/12/2015 at 9:45 PM
// Last Modified:   08/16/2015 at 1:04 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    /// <summary>
    ///     ChatEngine represents a ChatBot, its parsing capabilities, its knowledge library, etc.
    /// </summary>
    /// <remarks>
    ///     NOTE: This class is now a partial class. Check the other partial classes for more details
    /// 
    ///     TODO: This is a very monolithic class that needs to have many more utility classes and
    ///     have its responsibilities shared. Of course, documentation is important too.
    /// </remarks>
    public partial class ChatEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatEngine" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="SecurityException">The caller does not have the appropriate permission.</exception>
        /// <exception cref="DirectoryNotFoundException">Attempted to set a local path that cannot be found.</exception>
        public ChatEngine([CanBeNull] IConsole logger = null)
        {
            // Get logging online ASAP
            Logger = logger;

            // Set Directory-related items

            // Build helper components
            _tagFactory = new TagHandlerFactory(this);
            _aimlLoader = new AimlLoader(this);
            RootNode = new Node();
            Librarian = new ChatEngineLibrarian(this);

            // Populate smaller value dictionaries
            SentenceSplitters = new List<string> { ".", "!", "?", ";" };

        }

        /// <summary>
        /// Gets the librarian that manages settings.
        /// </summary>
        /// <value>The librarian.</value>
        [NotNull]
        internal ChatEngineLibrarian Librarian { get; }

        /// <summary>
        ///     Gets a list of sentence splitters. Sentence splitters are punctuation characters such as . or !
        ///     that define breaks between sentences.
        /// </summary>
        /// <value>The sentence splitters.</value>
        [NotNull]
        [ItemNotNull]
        public List<string> SentenceSplitters { get; }

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        [CanBeNull]
        public IConsole Logger { get; set; }

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
        ///     Logs the specified message to the logger.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        internal void Log(string message, LogLevel level)
        {
            Logger?.Log("ChatEngine", message, level);
        }


    }

}