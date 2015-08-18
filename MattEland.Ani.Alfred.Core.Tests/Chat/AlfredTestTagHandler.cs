﻿// ---------------------------------------------------------
// AlfredTestTagHandler.cs
// 
// Created on:      08/17/2015 at 11:36 PM
// Last Modified:   08/17/2015 at 11:36 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    /// A tag handler for testing. Not intended for actual product incorporation.
    /// </summary>
    [HandlesAimlTag("alfredtest")]
    public class AlfredTestTagHandler : AimlTagHandler
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="parameters"/> is <see langword="null" />.</exception>
        public AlfredTestTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            WasInvoked = true;
            return string.Empty;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the class's transform / process change method was called.
        /// </summary>
        /// <value><c>true</c> if it was invoked; otherwise, <c>false</c>.</value>
        public static bool WasInvoked { get; set; }
    }
}