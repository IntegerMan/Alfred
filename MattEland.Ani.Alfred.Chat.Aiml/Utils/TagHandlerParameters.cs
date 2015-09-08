// ---------------------------------------------------------
// TagHandlerParameters.cs
// 
// Created on:      08/22/2015 at 11:36 PM
// Last Modified:   08/24/2015 at 12:49 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     A container for parameters given to <see cref="AimlTagHandler"/> objects upon creation.
    ///     This keeps invocations from being unseemly long and makes it easier to manage parameter
    ///     changes with many consumers.
    /// </summary>
    public sealed class TagHandlerParameters
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="TagHandlerParameters" /> class.
        /// </summary>
        /// <param name="chatEngine">The chat engine.</param>
        /// <param name="user">The user.</param>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="chatResult">The result.</param>
        /// <param name="element">The template node.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="query" />, <paramref name="request" />, <paramref name="user" />,
        ///     <paramref name="chatEngine" />, or
        ///     <paramref name="element" /> are <see langword="null" />.
        /// </exception>
        public TagHandlerParameters([NotNull] ChatEngine chatEngine,
                                    [NotNull] User user,
                                    [NotNull] SubQuery query,
                                    [NotNull] Request request,
                                    ChatResult chatResult,
                                    [NotNull] XmlElement element)
        {
            //- Validate
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (element == null) { throw new ArgumentNullException(nameof(element)); }

            //- Set Properties
            ChatEngine = chatEngine;
            User = user;
            Query = query;
            Request = request;
            ChatResult = chatResult;
            Element = element;
        }

        /// <summary>
        ///     Gets the chat engine.
        /// </summary>
        /// <value>The chat engine.</value>
        [NotNull]
        internal ChatEngine ChatEngine { get; }

        /// <summary>
        ///     Gets the user.
        /// </summary>
        /// <value>The user.</value>
        [NotNull]
        internal User User { get; }

        /// <summary>
        ///     Gets the query.
        /// </summary>
        /// <value>The query.</value>
        [NotNull]
        internal SubQuery Query { get; }

        /// <summary>
        ///     Gets the request.
        /// </summary>
        /// <value>The request.</value>
        [NotNull]
        internal Request Request { get; }

        /// <summary>
        ///     Gets the result.
        /// </summary>
        /// <value>The result.</value>
        internal ChatResult ChatResult { get; }

        /// <summary>
        ///     Gets the template node.
        /// </summary>
        /// <value>The template node.</value>
        [NotNull]
        internal XmlElement Element { get; }
    }
}