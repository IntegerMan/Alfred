// ---------------------------------------------------------
// ChatEngine.cs
// 
// Created on:      08/12/2015 at 9:45 PM
// Last Modified:   08/16/2015 at 1:04 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;

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
        ///     Initializes a new instance of the <see cref="ChatEngine" /> class.
        /// </summary>
        public ChatEngine()
        {
            _tagFactory = new TagHandlerFactory(this);
            GlobalSettings = new SettingsDictionary();
            GenderSubstitutions = new SettingsDictionary();
            SecondPersonToFirstPersonSubstitutions = new SettingsDictionary();
            FirstPersonToSecondPersonSubstitutions = new SettingsDictionary();
            Substitutions = new SettingsDictionary();

            SentenceSplitters = new List<string> { ".", "!", "?", ";" };

            RootNode = new Node();
        }

        /// <summary>
        ///     Gets the gender substitutions dictionary. This is a collection of male and female pronouns and
        ///     their
        ///     replacement values to use when the "gender" AIML tag is present.
        /// </summary>
        /// <value>The gender substitutions dictionary.</value>
        [NotNull]
        public SettingsDictionary GenderSubstitutions { get; }

        /// <summary>
        ///     Gets the global settings dictionary.
        /// </summary>
        /// <value>The global settings.</value>
        [NotNull]
        public SettingsDictionary GlobalSettings { get; }

        /// <summary>
        ///     Gets the person substitutions settings dictionary for second person to first person
        ///     conversions.
        /// </summary>
        /// <value>The person substitutions settings dictionary.</value>
        [NotNull]
        public SettingsDictionary SecondPersonToFirstPersonSubstitutions { get; }

        /// <summary>
        ///     Gets the person substitutions settings dictionary. This contains things related to moving from
        ///     the first person to the second person.
        /// </summary>
        /// <value>The person substitutions settings dictionary.</value>
        [NotNull]
        public SettingsDictionary FirstPersonToSecondPersonSubstitutions { get; }

        /// <summary>
        ///     Gets a list of sentence splitters. Sentence splitters are punctuation characters such as . or !
        ///     that define breaks between sentences.
        /// </summary>
        /// <value>The sentence splitters.</value>
        [NotNull]
        [ItemNotNull]
        public List<string> SentenceSplitters { get; }

        /// <summary>
        ///     Gets the substitutions dictionary. Substitutions are common phrases that are condensed and
        ///     translated to their equivalent meaning to increase bot flexibility and make content authoring
        ///     easier.
        /// </summary>
        /// <value>The substitutions.</value>
        public SettingsDictionary Substitutions { get; }

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