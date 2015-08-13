// ---------------------------------------------------------
// AimlTagHandler.cs
// 
// Created on:      08/12/2015 at 10:25 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Xml;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    public abstract class AimlTagHandler : TextTransformer
    {
        public bool isRecursive = true;
        public SubQuery query;
        public Request request;
        public Result result;
        public XmlNode templateNode;
        public User user;

        public AimlTagHandler(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, templateNode.OuterXml)
        {
            this.user = user;
            this.query = query;
            this.request = request;
            this.result = result;
            this.templateNode = templateNode;
            this.templateNode.Attributes.RemoveNamedItem("xmlns");
        }

        public AimlTagHandler()
        {
        }

        public static XmlNode getNode(string outerXML)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(outerXML);
            return xmlDocument.FirstChild;
        }
    }
}