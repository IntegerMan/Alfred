// ---------------------------------------------------------
// TextTransformer.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/21/2015 at 11:58 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     An abstract class representing a class that will transform input text into output text.
    /// </summary>
    public abstract class TextTransformerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformerBase" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        /// <param name="input">The input string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="chatEngine" /> is <see langword="null" />.</exception>
        protected TextTransformerBase([NotNull] ChatEngine chatEngine, [CanBeNull] string input)
        {
            if (chatEngine == null)
            {
                throw new ArgumentNullException(nameof(chatEngine));
            }

            ChatEngine = chatEngine;
            InputString = input;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformerBase" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        /// <exception cref="ArgumentNullException"><paramref name="chatEngine" /> is <see langword="null" />.</exception>
        protected TextTransformerBase([NotNull] ChatEngine chatEngine) : this(chatEngine, null)
        {
        }

        /// <summary>
        ///     Gets the chat ChatEngine associated with this transformer.
        /// </summary>
        /// <value>The ChatEngine.</value>
        [NotNull]
        public ChatEngine ChatEngine
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets the current locale.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
        protected CultureInfo Locale
        {
            get { return ChatEngine.Locale; }
        }

        /// <summary>
        ///     Gets or sets the input string.
        /// </summary>
        /// <value>The input string.</value>
        [CanBeNull]
        public string InputString { get; set; }

        /// <summary>
        ///     Gets the output string.
        /// </summary>
        /// <value>The output string.</value>
        [NotNull]
        public string OutputString
        {
            get { return Transform(); }
        }

        /// <summary>
        ///     Gets the librarian containing settings files.
        /// </summary>
        /// <value>The librarian.</value>
        [NotNull]
        public ChatEngineLibrarian Librarian
        {
            get { return ChatEngine.Librarian; }
        }

        /// <summary>
        ///     Transforms the specified input text into output text and returns it.
        ///     The input value then becomes InputString in this instance.
        /// </summary>
        /// <param name="input">The input text.</param>
        /// <returns>The outputted text from the transform.</returns>
        [NotNull]
        public string Transform([CanBeNull] string input)
        {
            //- Store the new input
            InputString = input;

            // Process and return the result using our primary method
            return Transform();
        }

        /// <summary>
        ///     Transforms the input text into the output text.
        /// </summary>
        /// <returns>The outputted text from the transform.</returns>
        [NotNull]
        public string Transform()
        {
            //- Ensure we have a valid value
            if (string.IsNullOrWhiteSpace(InputString))
            {
                return string.Empty;
            }

            // Farm out processing the transform to the concrete implementation
            return ProcessChange();
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        [NotNull]
        protected abstract string ProcessChange();

        /// <summary>
        ///     Logs the specified message to the logger.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        protected void Log([CanBeNull] string message, LogLevel level)
        {
            ChatEngine.Log(message, level);
        }

        /// <summary>
        ///     Logs the specified message to the logger.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void Error([CanBeNull] string message)
        {
            ChatEngine.Log(message, LogLevel.Error);
        }

        /// <summary>
        ///     Gets the global setting with the specified name.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>The value of the setting or string.Empty if no setting found</returns>
        [NotNull]
        protected string GetGlobalSetting([CanBeNull] string settingName)
        {
            return settingName == null
                       ? string.Empty
                       : Librarian.GlobalSettings.GetValue(settingName).NonNull();

        }

        /// <summary>
        ///     Gets an attribute from an XML node and returns string.empty if the attribute isn't present.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="name">The name.</param>
        /// <returns>The attribute value or string.Empty as a fallback.</returns>
        [NotNull]
        protected static string GetAttributeSafe([CanBeNull] XmlElement element,
                                                 [CanBeNull] string name)
        {
            if (element != null && name.HasText() && element.HasAttribute(name))
            {
                return element.GetAttribute(name);
            }

            return string.Empty;
        }
    }
}