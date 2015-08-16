// ---------------------------------------------------------
// AimlTagHandler.cs
// 
// Created on:      08/12/2015 at 10:25 PM
// Last Modified:   08/14/2015 at 6:01 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     An abstract class representing a TextTransformer that can also handle an AIML tag.
    /// </summary>
    public abstract class AimlTagHandler : TextTransformer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="parameters"/> is <see langword="null" />.</exception>
        protected AimlTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters.ChatEngine, parameters.TemplateNode.OuterXml)
        {
            //- Validation
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            //- Assign fields
            User = parameters.User;
            Query = parameters.Query;
            Request = parameters.Request;
            Result = parameters.Result;

            // Assign the template node and clear out the namespace values
            parameters.TemplateNode.Attributes?.RemoveNamedItem("xmlns");
            TemplateNode = parameters.TemplateNode;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is recursive.
        /// </summary>
        /// <value><c>true</c> if this instance is recursive; otherwise, <c>false</c>.</value>
        public bool IsRecursive { get; set; } = true;

        /// <summary>
        ///     Gets the query.
        /// </summary>
        /// <value>The query.</value>
        [NotNull]
        public SubQuery Query { get; }

        /// <summary>
        ///     Gets the request.
        /// </summary>
        /// <value>The request.</value>
        [NotNull]
        public Request Request { get; }

        /// <summary>
        ///     Gets the result of the operation.
        /// </summary>
        /// <value>The result.</value>
        public Result Result { get; }

        /// <summary>
        ///     Gets the template node.
        /// </summary>
        /// <value>The template node.</value>
        [NotNull]
        public XmlNode TemplateNode { get; }

        /// <summary>
        ///     Gets the user.
        /// </summary>
        /// <value>The user.</value>
        [NotNull]
        public User User { get; }

        /// <summary>
        ///     Gets the template node as an XmlElement.
        /// </summary>
        /// <value>The template element.</value>
        [CanBeNull]
        protected XmlElement TemplateElement
        {
            get { return TemplateNode as XmlElement; }
        }

        /// <summary>
        ///     Gets an XML node from an XML string.
        /// </summary>
        /// <param name="xml">The outer XML.</param>
        /// <returns>An XmlNode from the document</returns>
        /// <exception cref="ArgumentNullException">xml</exception>
        /// <exception cref="XmlException">
        ///     There is a load or parse error in the XML. In this case, the
        ///     document remains empty.
        /// </exception>
        public static XmlNode BuildNode([NotNull] string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            // Build out a document from the XML
            // TODO: Use XDocument and XElement
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            // Return the root element
            return xmlDocument.FirstChild;
        }

        /// <summary>
        ///     Gets tag handler parameters for the given template node.
        /// </summary>
        /// <param name="templateNode">The template node.</param>
        /// <returns>TagHandlerParameters.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="templateNode"/> is <see langword="null" />.</exception>
        [NotNull]
        protected TagHandlerParameters GetTagHandlerParametersForNode([NotNull] XmlNode templateNode)
        {
            if (templateNode == null)
            {
                throw new ArgumentNullException(nameof(templateNode));
            }

            return new TagHandlerParameters(ChatEngine, User, Query, Request, Result, templateNode);
        }

        /// <summary>
        ///     Gets the XML child node for an AIML star operation.
        /// </summary>
        /// <returns>The star node.</returns>
        [NotNull]
        protected static XmlNode BuildStarNode()
        {
            try
            {
                var starNode = BuildNode("<star/>");
                Debug.Assert(starNode != null);
                return starNode;
            }
            catch (XmlException ex)
            {
                Debug.Fail("GetStarNode cannot return a null value but encountered an XmlException: " + ex.Message);

                // ReSharper disable once ExceptionNotDocumented
                // ReSharper disable once HeuristicUnreachableCode
                throw;
            }
        }

        /// <summary>
        ///     Builds a star tag handler.
        /// </summary>
        /// <returns>A star tag handler.</returns>
        [NotNull]
        protected StarTagHandler BuildStarTagHandler()
        {
            var node = BuildStarNode();
            var parameters = GetTagHandlerParametersForNode(node);

            return new StarTagHandler(parameters);
        }
    }
}