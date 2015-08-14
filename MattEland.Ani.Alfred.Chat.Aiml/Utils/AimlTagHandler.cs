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
        /// Gets the template node as an XmlElement.
        /// </summary>
        /// <value>The template element.</value>
        [CanBeNull]
        protected XmlElement TemplateElement
        {
            get { return TemplateNode as XmlElement; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        protected AimlTagHandler([NotNull] TagHandlerParameters parameters)
                    : base(parameters.ChatEngine, parameters.TemplateNode.OuterXml)
        {
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

        /// <summary>
        /// Gets tag handler parameters for the given template node.
        /// </summary>
        /// <param name="templateNode">The template node.</param>
        /// <returns>TagHandlerParameters.</returns>
        [NotNull]
        protected TagHandlerParameters GetTagHandlerParametersForNode([NotNull] XmlNode templateNode)
        {
            if (templateNode == null)
            {
                throw new ArgumentNullException(nameof(templateNode));
            }

            return new TagHandlerParameters(ChatEngine, User, Query, Request, Result, templateNode);
        }
    }
}