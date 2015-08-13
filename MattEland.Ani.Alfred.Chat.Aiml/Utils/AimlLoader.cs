// ---------------------------------------------------------
// AimlLoader.cs
// 
// Created on:      08/12/2015 at 10:25 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    public class AIMLLoader
    {
        private readonly Bot bot;

        public AIMLLoader(Bot bot)
        {
            this.bot = bot;
        }

        public void loadAIML()
        {
            loadAIML(bot.PathToAIML);
        }

        public void loadAIML(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new FileNotFoundException("The directory specified as the path to the AIML files (" + path +
                                                ") cannot be found by the AIMLLoader object. Please make sure the directory where you think the AIML files are to be found is the same as the directory specified in the settings file.");
            }
            bot.writeToLog("Starting to process AIML files found in the directory " + path);
            var files = Directory.GetFiles(path, "*.aiml");
            if (files.Length <= 0)
            {
                throw new FileNotFoundException("Could not find any .aiml files in the specified directory (" + path +
                                                "). Please make sure that your aiml file end in a lowercase aiml extension, for example - myFile.aiml is valid but myFile.AIML is not.");
            }
            foreach (var filename in files)
            {
                loadAIMLFile(filename);
            }
            bot.writeToLog("Finished processing the AIML files. " + Convert.ToString(bot.Size) +
                           " categories processed.");
        }

        public void loadAIMLFile(string filename)
        {
            bot.writeToLog("Processing AIML file: " + filename);
            var doc = new XmlDocument();
            doc.Load(filename);
            loadAIMLFromXML(doc, filename);
        }

        public void loadAIMLFromXML(XmlDocument doc, string filename)
        {
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.Name == "topic")
                {
                    processTopic(node, filename);
                }
                else if (node.Name == "category")
                {
                    processCategory(node, filename);
                }
            }
        }

        private void processTopic(XmlNode node, string filename)
        {
            var topicName = "*";
            if (node.Attributes.Count == 1 & node.Attributes[0].Name == "name")
            {
                topicName = node.Attributes["name"].Value;
            }
            foreach (XmlNode node1 in node.ChildNodes)
            {
                if (node1.Name == "category")
                {
                    processCategory(node1, topicName, filename);
                }
            }
        }

        private void processCategory(XmlNode node, string filename)
        {
            processCategory(node, "*", filename);
        }

        private void processCategory(XmlNode node, string topicName, string filename)
        {
            var node1 = FindNode("pattern", node);
            var node2 = FindNode("template", node);
            if (Equals(null, node1))
            {
                throw new XmlException("Missing pattern tag in a node found in " + filename);
            }
            if (Equals(null, node2))
            {
                throw new XmlException("Missing template tag in the node with pattern: " + node1.InnerText +
                                       " found in " + filename);
            }
            var path = generatePath(node, topicName, false);
            if (path.Length > 0)
            {
                try
                {
                    bot.Graphmaster.addCategory(path, node2.OuterXml, filename);
                    ++bot.Size;
                }
                catch
                {
                    bot.writeToLog("ERROR! Failed to load a new category into the graphmaster where the path = " + path +
                                   " and template = " + node2.OuterXml + " produced by a category in the file: " +
                                   filename);
                }
            }
            else
            {
                bot.writeToLog("WARNING! Attempted to load a new category with an empty pattern where the path = " +
                               path + " and template = " + node2.OuterXml + " produced by a category in the file: " +
                               filename);
            }
        }

        public string generatePath(XmlNode node, string topicName, bool isUserInput)
        {
            var node1 = FindNode("pattern", node);
            var node2 = FindNode("that", node);
            var that = "*";
            var pattern = !Equals(null, node1) ? node1.InnerText : string.Empty;
            if (!Equals(null, node2))
            {
                that = node2.InnerText;
            }
            return generatePath(pattern, that, topicName, isUserInput);
        }

        private XmlNode FindNode(string name, XmlNode node)
        {
            foreach (XmlNode xmlNode in node.ChildNodes)
            {
                if (xmlNode.Name == name)
                {
                    return xmlNode;
                }
            }
            return null;
        }

        public string generatePath(string pattern, string that, string topicName, bool isUserInput)
        {
            var stringBuilder = new StringBuilder();
            var str1 = string.Empty;
            string str2;
            string str3;
            string str4;
            if (bot.TrustAIML & !isUserInput)
            {
                str2 = pattern.Trim();
                str3 = that.Trim();
                str4 = topicName.Trim();
            }
            else
            {
                str2 = Normalize(pattern, isUserInput).Trim();
                str3 = Normalize(that, isUserInput).Trim();
                str4 = Normalize(topicName, isUserInput).Trim();
            }
            if (str2.Length <= 0)
            {
                return string.Empty;
            }
            if (str3.Length == 0)
            {
                str3 = "*";
            }
            if (str4.Length == 0)
            {
                str4 = "*";
            }
            if (str3.Length > bot.MaxThatSize)
            {
                str3 = "*";
            }
            stringBuilder.Append(str2);
            stringBuilder.Append(" <that> ");
            stringBuilder.Append(str3);
            stringBuilder.Append(" <topic> ");
            stringBuilder.Append(str4);
            return stringBuilder.ToString();
        }

        public string Normalize(string input, bool isUserInput)
        {
            var stringBuilder = new StringBuilder();
            ApplySubstitutions applySubstitutions = new ApplySubstitutions(bot);
            StripIllegalCharacters illegalCharacters = new StripIllegalCharacters(bot);
            foreach (string input1 in applySubstitutions.Transform(input).Split(" \r\n\t".ToCharArray()))
            {
                string str = !isUserInput
                                 ? (input1 == "*" || input1 == "_" ? input1 : illegalCharacters.Transform(input1))
                                 : illegalCharacters.Transform(input1);
                stringBuilder.Append(str.Trim() + " ");
            }
            return stringBuilder.ToString().Replace("  ", " ");
        }
    }
}