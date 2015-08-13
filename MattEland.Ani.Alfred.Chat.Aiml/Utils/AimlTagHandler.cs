// ---------------------------------------------------------
// AimlTagHandler.cs
// 
// Created on:      08/12/2015 at 10:25 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Xml;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    /// An abstract class representing a TextTransformer that can also handle an AIML tag.
    /// </summary>
    public abstract class AimlTagHandler : TextTransformer
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is recursive.
        /// </summary>
        /// <value><c>true</c> if this instance is recursive; otherwise, <c>false</c>.</value>
        public bool IsRecursive { get; set; } = true;

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <value>The query.</value>
        public SubQuery Query { get; }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        public Request Request { get; }

        /// <summary>
        /// Gets the result of the operation.
        /// </summary>
        /// <value>The result.</value>
        public Result Result { get; }

        /// <summary>
        /// Gets the template node.
        /// </summary>
        /// <value>The template node.</value>
        [NotNull]
        public XmlNode TemplateNode { get; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="chatEngine">The chat engine.</param>
        /// <param name="user">The user.</param>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="result">The result.</param>
        /// <param name="templateNode">The template node.</param>
        protected AimlTagHandler(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result,
                                 [NotNull] XmlNode templateNode)
                    : base(chatEngine, templateNode.OuterXml)
        {
            //- Validate
            if (templateNode?.Attributes == null)
            {
                throw new ArgumentNullException(nameof(templateNode));
            }

            //- Assign fields
            User = user;
            Query = query;
            Request = request;
            Result = result;

            // Assign the template node and clear out the namespace values
            templateNode.Attributes.RemoveNamedItem("xmlns");
            TemplateNode = templateNode;
        }

        /// <summary>
        /// Gets an XML node from an XML string.
        /// </summary>
        /// <param name="xml">The outer XML.</param>
        /// <returns>An XmlNode from the document</returns>
        /// <remarks>
        /// TODO: This is only here for convenience. An extension method may be in order
        /// </remarks>
        public static XmlNode GetNode([NotNull] string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentException(nameof(xml));
            }

            // Build out a document from the XML
            // TODO: Use XDocument and XElement
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            // Return the root element
            return xmlDocument.FirstChild;
        }
    }
}