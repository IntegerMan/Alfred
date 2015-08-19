// ---------------------------------------------------------
// TagHandlerParameters.cs
// 
// Created on:      08/14/2015 at 1:24 PM
// Last Modified:   08/15/2015 at 11:16 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     A container for parameters given to AimlTagHandler objects upon creation. This
    ///     keeps invocations from being unseemly long and makes it easier to manage parameter
    ///     changes with many consumers.
    /// </summary>
    public class TagHandlerParameters
    {
        [NotNull]
        private readonly ChatEngine _chatEngine;

        [NotNull]
        private readonly SubQuery _query;

        [NotNull]
        private readonly Request _request;

        private readonly Result _result;

        [NotNull]
        private readonly XmlNode _templateNode;

        [NotNull]
        private readonly User _user;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TagHandlerParameters" /> class.
        /// </summary>
        /// <param name="chatEngine">The chat engine.</param>
        /// <param name="user">The user.</param>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="result">The result.</param>
        /// <param name="templateNode">The template node.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="query" />, <paramref name="request" />, <paramref name="user" />, <paramref name="chatEngine" />, or
        ///     <paramref name="templateNode" /> are <see langword="null" />.
        /// </exception>
        public TagHandlerParameters([NotNull] ChatEngine chatEngine,
                                    [NotNull] User user,
                                    [NotNull] SubQuery query,
                                    [NotNull] Request request,
                                    Result result,
                                    [NotNull] XmlNode templateNode)
        {
            //- Validate
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (templateNode == null)
            {
                throw new ArgumentNullException(nameof(templateNode));
            }

            //- Set Properties
            _chatEngine = chatEngine;
            _user = user;
            _query = query;
            _request = request;
            _result = result;
            _templateNode = templateNode;
        }

        /// <summary>
        ///     Gets the chat engine.
        /// </summary>
        /// <value>The chat engine.</value>
        [NotNull]
        public ChatEngine ChatEngine
        {
            get { return _chatEngine; }
        }

        /// <summary>
        ///     Gets the user.
        /// </summary>
        /// <value>The user.</value>
        [NotNull]
        public User User
        {
            get { return _user; }
        }

        /// <summary>
        ///     Gets the query.
        /// </summary>
        /// <value>The query.</value>
        [NotNull]
        public SubQuery Query
        {
            get { return _query; }
        }

        /// <summary>
        ///     Gets the request.
        /// </summary>
        /// <value>The request.</value>
        [NotNull]
        public Request Request
        {
            get { return _request; }
        }

        /// <summary>
        ///     Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public Result Result
        {
            get { return _result; }
        }

        /// <summary>
        ///     Gets the template node.
        /// </summary>
        /// <value>The template node.</value>
        [NotNull]
        public XmlNode TemplateNode
        {
            get { return _templateNode; }
        }
    }
}