// ---------------------------------------------------------
// TagHandlerParameters.cs
// 
// Created on:      08/14/2015 at 1:24 PM
// Last Modified:   08/14/2015 at 1:43 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

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
        private readonly ChatEngine _chatEngine;
        private readonly SubQuery _query;
        private readonly Request _request;
        private readonly Result _result;

        [NotNull]
        private readonly XmlNode _templateNode;

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
        public TagHandlerParameters(ChatEngine chatEngine,
                                    User user,
                                    SubQuery query,
                                    Request request,
                                    Result result,
                                    [NotNull] XmlNode templateNode)
        {
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
        public ChatEngine ChatEngine
        {
            get { return _chatEngine; }
        }

        /// <summary>
        ///     Gets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User
        {
            get { return _user; }
        }

        /// <summary>
        ///     Gets the query.
        /// </summary>
        /// <value>The query.</value>
        public SubQuery Query
        {
            get { return _query; }
        }

        /// <summary>
        ///     Gets the request.
        /// </summary>
        /// <value>The request.</value>
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