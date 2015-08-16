// ---------------------------------------------------------
// ChatEngine.Loading.cs
// 
// Created on:      08/16/2015 at 12:51 AM
// Last Modified:   08/16/2015 at 12:51 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
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


        /// <summary>
        /// Gets the time out limit for a request in milliseconds. Defaults to 2000 (2 seconds).
        /// </summary>
        /// <value>The time out.</value>
        public double TimeOut { get; set; } = 2000;

        /// <summary>
        ///     Gets the timeout message.
        ///     This message is used when a request takes too long and times out.
        /// </summary>
        /// <value>The timeout message.</value>
        [NotNull]
        public string TimeOutMessage
        {
            get
            {
                var timeoutMessage = GlobalSettings.GetValue("timeoutmessage");

                if (string.IsNullOrEmpty(timeoutMessage))
                {
                    return "Your request has timed out. Please try again or phrase your sentence differently.";
                }

                // ReSharper disable once AssignNullToNotNullAttribute
                return timeoutMessage;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size that can be used to hold a path in the that value.
        /// </summary>
        /// <value>The maximum size of the that.</value>
        public int MaxThatSize { get; set; } = 256;

        [NotNull]
        private readonly TagHandlerFactory _tagFactory;

        /// <summary>
        ///     Gets or sets the root node of the Aiml knowledge graph.
        /// </summary>
        /// <value>The root node.</value>
        [NotNull]
        public Node RootNode { get; set; }

        private string ProcessNode([NotNull] XmlNode node,
                                   SubQuery query,
                                   [NotNull] Request request,
                                   Result result,
                                   User user)
        {
            //- Validation
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.CheckForTimedOut())
            {
                return string.Empty;
            }
            var str = node.Name.ToLower();
            if (str == "template")
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

            var handler = _tagFactory.Build(node, query, request, result, user, str);

            if (Equals(null, handler))
            {
                return node.InnerText;
            }

            if (handler.IsRecursive)
            {
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
            var node2 = AimlTagHandler.GetNode("<node>" + handler.Transform() + "</node>");
            if (!node2.HasChildNodes)
            {
                return node2.InnerXml;
            }
            var stringBuilder1 = new StringBuilder();
            foreach (XmlNode node1 in node2.ChildNodes)
            {
                stringBuilder1.Append(ProcessNode(node1, query, request, result, user));
            }
            return stringBuilder1.ToString();
        }

        public Result Chat(string rawInput, string UserGUID)
        {
            return Chat(new Request(rawInput, new User(UserGUID, this), this));
        }

        public Result Chat(Request request)
        {
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
            foreach (var query in result.SubQueries)
            {
                if (query.Template.Length > 0)
                {
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
                        Log("A problem was encountered when trying to process the input: " +
                            request.RawInput + " with the template: \"" + query.Template +
                            "\": " + ex.Message,
                            LogLevel.Error);
                    }
                }
            }
            result.Completed();
            request.User.AddResult(result);
            return result;
        }

        /// <summary>
        ///     Gets the count of AIML nodes in memory.
        /// </summary>
        /// <value>The count of AIML nodes.</value>
        public int NodeCount
        {
            get { return RootNode.ChildrenCount; }
        }

    }
}