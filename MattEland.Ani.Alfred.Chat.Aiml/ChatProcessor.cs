// ---------------------------------------------------------
// ChatProcessor.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 1:45 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
    /// <summary>
    ///     A helper class that handles the logic for processing a chat request for the chat engine.
    /// </summary>
    internal class ChatProcessor
    {

        [NotNull]
        private readonly AimlLoader _aimlLoader;

        [NotNull]
        private readonly ChatEngine _chatEngine;

        [NotNull]
        private readonly TagHandlerFactory _tagFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="chatEngine" /> is <see langword="null" />.</exception>
        internal ChatProcessor([NotNull] ChatEngine chatEngine)
        {
            if (chatEngine == null) { throw new ArgumentNullException(nameof(chatEngine)); }

            // Store helpers
            _chatEngine = chatEngine;
            _tagFactory = new TagHandlerFactory(chatEngine);
            _aimlLoader = new AimlLoader(chatEngine);
        }

        /// <summary>
        ///     Gets the locale.
        /// </summary>
        /// <value>The locale.</value>
        private CultureInfo Locale
        {
            get { return _chatEngine.Locale; }
        }

        /// <summary>
        ///     Logs the specified message to the error log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The level.</param>
        private void Log(string message, LogLevel level)
        {
            _chatEngine.Log(message, level);
        }

        /// <summary>
        ///     Accepts a chat message from the user and returns the chat engine's reply.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A result object containing the engine's reply.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is <see langword="null" />.</exception>
        internal Result ProcessChatRequest([NotNull] Request request)
        {
            //- Validation
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            // Build a result for this request. This will link the two together.
            var result = new Result(request.User, _chatEngine, request);

            // Split the input into different sentences and build a path for each sentence.
            foreach (var pattern in SplitSentenceHelper.Split(request.RawInput, _chatEngine))
            {
                result.InputSentences.Add(pattern);
                var str = _aimlLoader.BuildPathString(pattern,
                                                      request.User.LastChatOutput,
                                                      request.User.Topic.NonNull(),
                                                      true);
                result.NormalizedPaths.Add(str);
            }

            // Build out SubQueries with appropriate templates based on the paths detected
            foreach (var path in result.NormalizedPaths)
            {
                // Build out a query based on the path
                var query = new SubQuery();

                // Search the node tree for the template most closely matched to this request
                var template = _chatEngine.RootNode.Evaluate(path,
                                                             query,
                                                             request,
                                                             MatchState.UserInput,
                                                             new StringBuilder());
                query.Template = template.NonNull();

                // Now that the query is polished, add it to the subqueries
                result.SubQueries.Add(query);
            }

            // Process each SubQuery to build out the result text
            foreach (var query in
                result.SubQueries.Where(query => query != null && query.Template.HasText()))
            {
                ProcessSubQuery(request, query, result);
            }

            // Complete the result and return everything
            result.Completed();
            request.User.AddResult(result);
            return result;
        }

        /// <summary>
        ///     Processes a chat SubQuery.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="query">The query.</param>
        /// <param name="result">The result.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("ReSharper", "CatchAllClause")]
        private void ProcessSubQuery([NotNull] Request request,
                                     [NotNull] SubQuery query,
                                     [NotNull] Result result)
        {
            //- Validate
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (result == null) { throw new ArgumentNullException(nameof(result)); }

            try
            {
                // Build an XML node out of the template the query traced to
                var element = AimlTagHandler.BuildElement(query.Template);

                // Process the chat node with the given template and tag handlers. This will result in the chat output.
                var nodeOutput = ProcessNode(element, query, request, result, request.User);

                // Check to see if the output had textual values, and, if so, add them to our output.
                if (nodeOutput.HasText()) { result.OutputSentences.Add(nodeOutput); }
            }
            catch (Exception ex)
            {
                /* Catching all exceptions here because ProcessNode could be doing invokes to third party code due to how
                HandlesAimlTag attribute and dynamic invocation work */
                Error(string.Format(Locale,
                                    Resources.ChatProcessNodeError.NonNull(),
                                    request.RawInput,
                                    query.Template,
                                    ex.Message));
            }
        }

        /// <summary>
        ///     Logs an error message to the console.
        /// </summary>
        /// <param name="message">The message.</param>
        private void Error([CanBeNull] string message)
        {
            Log(message, LogLevel.Error);
        }

        /// <summary>
        ///     Processes an XML node as part of resolving a chat query and returns a chat result dependant on
        ///     the type of node and the node's contents.
        ///     This method is called recursively when resolving complicated compound chat messages.
        /// </summary>
        /// <param name="element">The node.</param>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="result">The result.</param>
        /// <param name="user">The user.</param>
        /// <returns>The textual result of evaluating the node</returns>
        [NotNull]
        private string ProcessNode([NotNull] XmlElement element,
                                   [NotNull] SubQuery query,
                                   [NotNull] Request request,
                                   [NotNull] Result result,
                                   [NotNull] User user)
        {
            //- Validation
            if (element == null) { throw new ArgumentNullException(nameof(element)); }
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            // If we've already timed out, give up on this.
            // This is important for iterative operations on complex queries.
            if (request.CheckForTimedOut()) { return string.Empty; }

            // Farm out handling for template nodes
            if (element.Name.Matches("template"))
            {
                return ProcessTemplateNode(element, query, request, result, user);
            }

            // We need a handler for this type of node. Grab it from the registered tag handlers
            var handler = _tagFactory.Build(element,
                                            query,
                                            request,
                                            result,
                                            user,
                                            element.Name.ToLowerInvariant());

            // We can encounter nodes of unknown types. These will not have handlers
            if (handler == null)
            {
                //? Does the AIML specification call for string.Empty here?
                return element.InnerText;
            }

            // Farm out handling to recursive node handling function
            if (handler.IsRecursive)
            {
                return ProcessRecursiveNode(element, query, request, result, user, handler);
            }

            // Execute the transformation and build a new node XML string from the result
            var nodeContents = string.Format(Locale, "<node>{0}</node>", handler.Transform());

            // Build a node out of the output of the transform
            var evaluatedNode = AimlTagHandler.BuildElement(nodeContents);

            /* If it's simple, just return it otherwise recursively process each child node and 
               build out our output from their values. */
            return !evaluatedNode.HasChildNodes
                       ? evaluatedNode.InnerXml.NonNull()
                       : ProcessChildNodes(evaluatedNode, user, request, query, result);

        }

        /// <summary>
        ///     Processes the child nodes and returns the compound result of processing them.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="user">The user.</param>
        /// <param name="request">The request.</param>
        /// <param name="query">The query.</param>
        /// <param name="result">The result.</param>
        /// <returns>The result of processing each child.</returns>
        [NotNull]
        private string ProcessChildNodes([NotNull] XmlNode node,
                                         [NotNull] User user,
                                         [NotNull] Request request,
                                         [NotNull] SubQuery query,
                                         [NotNull] Result result)
        {
            var sbOutput = new StringBuilder();

            foreach (XmlNode childNode in node.ChildNodes)
            {
                sbOutput.Append(ProcessChildNode(childNode, user, request, query, result));
            }

            return sbOutput.ToString();
        }

        /// <summary>
        ///     Processes the child node and returns the result
        /// </summary>
        /// <param name="childNode">The child node.</param>
        /// <param name="user">The user.</param>
        /// <param name="request">The request.</param>
        /// <param name="query">The query.</param>
        /// <param name="result">The result.</param>
        private string ProcessChildNode([CanBeNull] XmlNode childNode,
                                        [NotNull] User user,
                                        [NotNull] Request request,
                                        [NotNull] SubQuery query,
                                        [NotNull] Result result)
        {
            // If it's XmlText, just return it
            var childText = childNode as XmlText;
            if (childText != null) { return childText.InnerText; }

            // For XmlElements, go through the main processing loop
            var childElement = childNode as XmlElement;
            if (childElement != null)
            {
                return ProcessNode(childElement, query, request, result, user);
            }

            return string.Empty;
        }

        /// <summary>
        ///     Processes chat contents with a handler that returns true for IsRecursive.
        /// </summary>
        /// <remarks>
        ///     This is a separate method to reduce the complexity of ProcessNode
        /// </remarks>
        /// <param name="node">The node.</param>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="result">The result.</param>
        /// <param name="user">The user.</param>
        /// <param name="handler">The handler.</param>
        /// <returns>System.String.</returns>
        [NotNull]
        private string ProcessRecursiveNode([NotNull] XmlNode node,
                                            [NotNull] SubQuery query,
                                            [NotNull] Request request,
                                            [NotNull] Result result,
                                            [NotNull] User user,
                                            [NotNull] TextTransformer handler)
        {
            //- Validation
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (handler == null) { throw new ArgumentNullException(nameof(handler)); }

            // Process child nodes
            if (node.HasChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    var childElement = childNode as XmlElement;
                    if (childElement != null)
                    {
                        childElement.InnerXml = ProcessNode(childElement,
                                                            query,
                                                            request,
                                                            result,
                                                            user);
                    }
                }
            }

            // Do the final evaluation on the handler and return that
            return handler.Transform();
        }

        /// <summary>
        ///     Processes a template node by processing all internal contents and returning the results.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="result">The result.</param>
        /// <param name="user">The user.</param>
        /// <returns>The output text.</returns>
        [NotNull]
        private string ProcessTemplateNode([NotNull] XmlNode node,
                                           [NotNull] SubQuery query,
                                           [NotNull] Request request,
                                           [NotNull] Result result,
                                           [NotNull] User user)
        {
            //- Validation
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            //- Early Exit for no children
            if (!node.HasChildNodes) { return string.Empty; }

            // Loop through all contained children and process them, returning the results
            var sbOutput = new StringBuilder();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                var value = ProcessChildNode(childNode, user, request, query, result);
                sbOutput.Append(value);
            }

            return sbOutput.ToString();
        }
    }
}