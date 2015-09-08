// ---------------------------------------------------------
// HandlesAimlTagAttribute.cs
// 
// Created on:      08/14/2015 at 1:01 AM
// Last Modified:   08/14/2015 at 1:01 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// An attribute describing a class that can handle an AIML tag
    /// </summary>
    [AttributeUsage(AttributeTargets.Class), UsedImplicitly, PublicAPI]
    public sealed class HandlesAimlTagAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlesAimlTagAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the tag this class handles.</param>
        public HandlesAimlTagAttribute([NotNull] string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the tag this class handles.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public string Name
        {
            [DebuggerStepThrough]
            get;
        }
    }
}