// ---------------------------------------------------------
// TextTransformer.cs
// 
// Created on:      08/12/2015 at 10:36 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     An abstract class representing a class that will transform input text into output text.
    /// </summary>
    public abstract class TextTransformer
    {
        [CanBeNull]
        private Bot _bot;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="bot">The bot.</param>
        /// <param name="inputString">The input string.</param>
        protected TextTransformer([CanBeNull] Bot bot, [CanBeNull] string inputString)
        {
            _bot = bot;
            InputString = inputString;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="bot">The bot.</param>
        protected TextTransformer([CanBeNull] Bot bot)
        {
            _bot = bot;
            InputString = string.Empty;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        protected TextTransformer()
        {
            _bot = null;
            InputString = string.Empty;
        }

        /// <summary>
        ///     Gets the chat bot associated with this transformer.
        /// </summary>
        /// <value>The bot.</value>
        [CanBeNull]
        [Obsolete("It'd be good to not need to use this anymore and rely on pass-throughs")]
        public Bot Bot
        {
            [DebuggerStepThrough]
            get { return _bot; }
            [DebuggerStepThrough]
            internal set { _bot = value; }
        }

        /// <summary>
        ///     Gets the current locale.
        /// </summary>
        /// <value>The locale.</value>
        protected CultureInfo Locale
        {
            get { return _bot != null ? _bot.Locale : CultureInfo.CurrentCulture; }
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
    }
}