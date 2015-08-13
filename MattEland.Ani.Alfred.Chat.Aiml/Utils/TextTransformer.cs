// ---------------------------------------------------------
// TextTransformer.cs
// 
// Created on:      08/12/2015 at 10:36 PM
// Last Modified:   08/13/2015 at 12:02 AM
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
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        /// <param name="inputString">The input string.</param>
        protected TextTransformer([CanBeNull] ChatEngine chatEngine, [CanBeNull] string inputString)
        {
            ChatEngine = chatEngine;
            InputString = inputString;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        protected TextTransformer([CanBeNull] ChatEngine chatEngine)
        {
            ChatEngine = chatEngine;
            InputString = string.Empty;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        protected TextTransformer()
        {
            ChatEngine = null;
            InputString = string.Empty;
        }

        /// <summary>
        ///     Gets the chat ChatEngine associated with this transformer.
        /// </summary>
        /// <value>The ChatEngine.</value>
        [CanBeNull]
        [Obsolete("It'd be good to not need to use this anymore and rely on pass-throughs")]
        public ChatEngine ChatEngine
        {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            internal set;
        }

        /// <summary>
        ///     Gets the current locale.
        /// </summary>
        /// <value>The locale.</value>
        protected CultureInfo Locale
        {
            get { return ChatEngine != null ? ChatEngine.Locale : CultureInfo.CurrentCulture; }
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