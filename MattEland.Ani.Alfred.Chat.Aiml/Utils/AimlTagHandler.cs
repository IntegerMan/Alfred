// ---------------------------------------------------------
// AimlTagHandler.cs
// 
// Created on:      08/22/2015 at 11:36 PM
// Last Modified:   08/24/2015 at 1:14 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     An abstract class representing a TextTransformer that can also handle an AIML tag.
    /// </summary>
    public abstract class AimlTagHandler : TextTransformerBase
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="parameters" /> is <see langword="null" />.</exception>
        protected AimlTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters.ChatEngine, parameters.Element.OuterXml)
        {
            //- Validation
            if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }

            //- Assign fields
            User = parameters.User;
            Query = parameters.Query;
            Request = parameters.Request;
            ChatResult = parameters.ChatResult;

            // Assign the template node and clear out the namespace values
            parameters.Element.Attributes.RemoveNamedItem("xmlns");
            TemplateElement = parameters.Element;
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
        public ChatResult ChatResult { get; }

        /// <summary>
        ///     Gets whether the template node has child nodes.
        /// </summary>
        /// <value>Whether or not there are child nodes.</value>
        protected bool HasChildNodes
        {
            get { return TemplateElement.HasChildNodes; }
        }

        /// <summary>
        ///     Gets the child nodes.
        /// </summary>
        /// <value>The child nodes.</value>
        [NotNull]
        [ItemNotNull]
        protected XmlNodeList ChildNodes
        {
            get { return TemplateElement.ChildNodes; }
        }

        /// <summary>
        ///     Gets the inner text or XML of the node representing this handler.
        /// </summary>
        /// <value>The contents of the node.</value>
        [NotNull]
        public string Contents
        {
            get { return TemplateElement.InnerText; }
            protected set { TemplateElement.InnerText = value; }
        }

        /// <summary>
        ///     Gets the name of the node representing this handler.
        /// </summary>
        /// <value>The name of the node.</value>
        [NotNull]
        protected string NodeName
        {
            get { return TemplateElement.Name; }
        }

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
        [NotNull]
        private XmlElement TemplateElement { get; }

        /// <summary>
        ///     Gets tag handler parameters for the given template node.
        /// </summary>
        /// <param name="element">The template element.</param>
        /// <returns>TagHandlerParameters.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="element" /> is <see langword="null" />
        ///     .
        /// </exception>
        [NotNull]
        private TagHandlerParameters GetTagHandlerParametersForNode([NotNull] XmlElement element)
        {
            if (element == null) { throw new ArgumentNullException(nameof(element)); }

            return new TagHandlerParameters(ChatEngine, User, Query, Request, ChatResult, element);
        }

        /// <summary>
        ///     Gets an attribute from the template element returning string.Empty if the attribute is not
        ///     found.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>The value of the attribute or string.Empty</returns>
        [NotNull]
        protected string GetAttribute([CanBeNull] string attributeName)
        {
            return GetAttributeSafe(TemplateElement, attributeName);
        }

        /// <summary>
        ///     Determines whether the specified attribute name is present on the template node.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Whether or not the attribute is present.</returns>
        protected bool HasAttribute([CanBeNull] string attributeName)
        {
            return TemplateElement.HasAttribute(attributeName);
        }

        /// <summary>
        ///     Gets the XML child node for an AIML star operation.
        /// </summary>
        /// <returns>The star node.</returns>
        [NotNull]
        protected static XmlElement BuildStarElement()
        {
            // TODO: Extract this out to a utility

            try
            {
                var starNode = BuildElement("<star/>");
                Debug.Assert(starNode != null);
                return starNode;
            }
            catch (XmlException ex)
            {
                Debug.Fail(
                           "GetStarElement cannot return a null value but encountered an XmlException: "
                           + ex.Message);

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
            // TODO: Extract this out to a utility

            var element = BuildStarElement();
            var parameters = GetTagHandlerParametersForNode(element);

            return new StarTagHandler(parameters);
        }

        /// <summary>
        ///     Executes a redirect to the specified target and returns the results
        /// </summary>
        /// <param name="redirectTarget">The redirect target.</param>
        /// <returns>The result text from the redirection</returns>
        [NotNull]
        protected string DoRedirect(string redirectTarget)
        {
            // TODO: Extract this out to a utility

            try
            {
                var xml = string.Format(CultureInfo.InvariantCulture,
                                        "<srai>{0}</srai>",
                                        redirectTarget);
                var node = BuildElement(xml.NonNull());
                var parameters = GetTagHandlerParametersForNode(node);

                return new RedirectTagHandler(parameters).Transform();
            }
            catch (XmlException xmlEx)
            {
                Error(string.Format(Locale,
                                    Resources.AimlTagHandlerDoRedirectBadXml.NonNull(),
                                    xmlEx.Message,
                                    redirectTarget));

                return string.Empty;
            }
        }

        /// <summary>
        ///     Gets an <see cref="XmlElement" /> from an XML string.
        /// </summary>
        /// <param name="xml">The outer XML.</param>
        /// <returns>An <see cref="XmlElement" /> from the xml</returns>
        /// <exception cref="ArgumentNullException">xml</exception>
        /// <exception cref="XmlException">
        ///     There is a load or parse error in the XML. In this case, the
        ///     document remains empty.
        /// </exception>
        [NotNull]
        public static XmlElement BuildElement([NotNull] string xml)
        {
            //- Validate
            if (xml.IsEmpty()) { throw new ArgumentNullException(nameof(xml)); }

            // Build out a document from the XML
            // TODO: Use XDocument and XElement
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            // Return the root element
            var child = xmlDocument.FirstChild as XmlElement;
            if (child != null) { return child; }

            //- If the item isn't an XmlElement, we have trouble; throw an XmlException.
            var message = string.Format(CultureInfo.CurrentCulture,
                                        Resources.AimlTagHandlerBuildElementBadXml.NonNull(),
                                        xml);

            throw new XmlException(message);
        }
    }
}