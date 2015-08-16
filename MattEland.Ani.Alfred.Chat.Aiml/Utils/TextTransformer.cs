﻿// ---------------------------------------------------------
// TextTransformer.cs
// 
// Created on:      08/12/2015 at 10:36 PM
// Last Modified:   08/16/2015 at 12:33 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     An abstract class representing a class that will transform input text into output text.
    /// </summary>
    public abstract class TextTransformer
    {
        [NotNull]
        private readonly ChatEngine _chatEngine;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        /// <param name="inputString">The input string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="chatEngine" /> is <see langword="null" />.</exception>
        protected TextTransformer([NotNull] ChatEngine chatEngine, [CanBeNull] string inputString)
        {
            if (chatEngine == null)
            {
                throw new ArgumentNullException(nameof(chatEngine));
            }

            _chatEngine = chatEngine;
            InputString = inputString;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        /// <exception cref="ArgumentNullException"><paramref name="chatEngine" /> is <see langword="null" />.</exception>
        protected TextTransformer([NotNull] ChatEngine chatEngine) : this(chatEngine, null)
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
            get
            {
                return _chatEngine;
            }
        }

        /// <summary>
        ///     Gets the current locale.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
        protected CultureInfo Locale
        {
            get { return _chatEngine.Locale; }
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
        public string OutputString
        {
            get { return Transform(); }
        }

        /// <summary>
        /// Gets the librarian containing settings files.
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
        protected abstract string ProcessChange();

        /// <summary>
        ///     Logs the specified message to the logger.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        protected void Log(string message, LogLevel level)
        {
            _chatEngine.Log(message, level);
        }

        /// <summary>
        ///     Gets the global setting with the specified name.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>The value of the setting or string.Empty if no setting found</returns>
        protected string GetGlobalSetting([CanBeNull] string settingName)
        {
            return null == settingName
                       ? string.Empty
                       : Librarian.GlobalSettings.GetValue(settingName);

        }
    }
}