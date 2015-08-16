// ---------------------------------------------------------
// ChatEngine.Processing.cs
// 
// Created on:      08/16/2015 at 12:52 AM
// Last Modified:   08/16/2015 at 1:28 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    public partial class ChatEngine
    {
        [NotNull]
        private readonly TagHandlerFactory _tagFactory;

        /// <summary>
        ///     Gets the time out limit for a request in milliseconds. Defaults to 2000 (2 seconds).
        /// </summary>
        /// <value>The time out.</value>
        public double TimeOut { get; set; } = 2000;

        /// <summary>
        ///     Gets or sets the maximum size that can be used to hold a path in the that value.
        /// </summary>
        /// <value>The maximum size of the that.</value>
        public int MaxThatSize { get; set; } = 256;

        /// <summary>
        ///     Gets or sets the root node of the Aiml knowledge graph.
        /// </summary>
        /// <value>The root node.</value>
        [NotNull]
        public Node RootNode { get; set; }

        /// <summary>
        ///     Gets the count of AIML nodes in memory.
        /// </summary>
        /// <value>The count of AIML nodes.</value>
        public int NodeCount
        {
            get { return RootNode.ChildrenCount; }
        }

        /// <summary>
        /// Accepts a chat message from the user and returns the chat engine's reply.
        /// </summary>
        /// <param name="input">The input chat message.</param>
        /// <param name="user">The user.</param>
        /// <returns>A result object containing the engine's reply.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">A chat message is required to interact with the system.</exception>
        public Result Chat([NotNull] string input, [NotNull] User user)
        {
            //- Validate
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (input.IsNullOrWhitespace())
            {
                throw new ArgumentException(Resources.ChatErrorNoMessage, nameof(input));
            }

            // Build a request that we can work with internally.
            return Chat(new Request(input, user, this));
        }

        /// <summary>
        /// Accepts a chat message from the user and returns the chat engine's reply.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A result object containing the engine's reply.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request"/> is <see langword="null" />.</exception>
        internal Result Chat([NotNull] Request request)
        {
            //- Validation
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new Result(request.User, this, request);
            var aimlLoader = new AimlLoader(this);
            foreach (var pattern in SplitSentenceHelper.Split(request.RawInput, this))
            {
                result.InputSentences.Add(pattern);
                var str = aimlLoader.BuildPathString(pattern,
                                                     request.User.LastChatOutput,
                                                     request.User.Topic,
                                                     true);
                result.NormalizedPaths.Add(str);
            }
            foreach (var str in result.NormalizedPaths)
            {
                var query = new SubQuery(str);
                query.Template = RootNode.Evaluate(str,
                                                   query,
                                                   request,
                                                   MatchState.UserInput,
                                                   new StringBuilder());
                result.SubQueries.Add(query);
            }
            foreach (var query in result.SubQueries.Where(query => query != null && query.Template.HasText()))
            {
                ProcessChatSubQuery(request, query, result);
            }
            result.Completed();
            request.User.AddResult(result);
            return result;
        }

        [SuppressMessage("ReSharper", "CatchAllClause")]
        private void ProcessChatSubQuery([NotNull] Request request,
                                         [NotNull] SubQuery query,
                                         [NotNull] Result result)
        {
            //- Validate
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            try
            {
                var node = AimlTagHandler.GetNode(query.Template);
                var str = ProcessNode(node,
                                      query,
                                      request,
                                      result,
                                      request.User);
                if (str.Length > 0)
                {
                    result.OutputSentences.Add(str);
                }
            }
            catch (Exception ex)
            {
                Log(string.Format(Locale, Resources.ChatProcessNodeError, request.RawInput, query.Template, ex.Message),
                    LogLevel.Error);
            }
        }

        private string ProcessNode([NotNull] XmlNode node,
                                   [NotNull] SubQuery query,
                                   [NotNull] Request request,
                                   [NotNull] Result result,
                                   [NotNull] User user)
        {
            //- Validation
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // If we've already timed out, give up on this.
            // This is important for iterative operations on complex queries.
            if (request.CheckForTimedOut())
            {
                return string.Empty;
            }

            // Farm out handling for template nodes
            if (node.Name.Matches("template"))
            {
                return ProcessTemplateNode(node, query, request, result, user);
            }

            // We need a handler for this type of node. Grab it from the registered tag handlers
            var handler = _tagFactory.Build(node, query, request, result, user, node.Name.ToLowerInvariant());

            // We can encounter nodes of unknown types. These will not have handlers
            if (handler == null)
            {
                //? Does the AIML specification call for string.Empty here?
                return node.InnerText;
            }

            // Farm out handling to recursive node handling function
            if (handler.IsRecursive)
            {
                return ProcessRecursiveNode(node, query, request, result, user, handler);
            }

            // Execute the transformation and build a new node XML string from the result
            var nodeContents = string.Format(Locale, "<node>{0}</node>", handler.Transform());

            // Build a node out of the output of the transform
            var evaluatedNode = AimlTagHandler.GetNode(nodeContents);
            if (evaluatedNode == null)
            {
                return string.Empty;
            }

            // If it's simple, just return it
            if (!evaluatedNode.HasChildNodes)
            {
                return evaluatedNode.InnerXml;
            }

            // Recursively process each child node and build out our output from their values.
            var sbOutput = new StringBuilder();
            foreach (XmlNode childNode in evaluatedNode.ChildNodes)
            {
                if (childNode != null)
                {
                    sbOutput.Append(ProcessNode(childNode, query, request, result, user));
                }
            }

            return sbOutput.ToString();
        }

        private string ProcessRecursiveNode(XmlNode node,
                                            SubQuery query,
                                            Request request,
                                            Result result,
                                            User user,
                                            [NotNull] TextTransformer handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (node.HasChildNodes)
            {
                foreach (XmlNode node1 in node.ChildNodes)
                {
                    if (node1.NodeType != XmlNodeType.Text)
                    {
                        node1.InnerXml = ProcessNode(node1, query, request, result, user);
                    }
                }
            }
            return handler.Transform();
        }

        private string ProcessTemplateNode(XmlNode node,
                                           SubQuery query,
                                           Request request,
                                           Result result,
                                           User user)
        {

            var stringBuilder = new StringBuilder();
            if (node.HasChildNodes)
            {
                foreach (XmlNode node1 in node.ChildNodes)
                {
                    stringBuilder.Append(ProcessNode(node1, query, request, result, user));
                }
            }
            return stringBuilder.ToString();
        }

    }
}