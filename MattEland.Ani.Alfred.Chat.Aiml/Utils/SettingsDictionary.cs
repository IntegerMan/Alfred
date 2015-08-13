// ---------------------------------------------------------
// SettingsDictionary.cs
// 
// Created on:      08/12/2015 at 10:28 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    public class SettingsDictionary
    {
        private readonly List<string> orderedKeys = new List<string>();
        private readonly Dictionary<string, string> settingsHash = new Dictionary<string, string>();
        protected Bot bot;

        public SettingsDictionary(Bot bot)
        {
            this.bot = bot;
        }

        public int Count
        {
            get { return orderedKeys.Count; }
        }

        public XmlDocument DictionaryAsXML
        {
            get
            {
                var xmlDocument = new XmlDocument();
                var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "");
                xmlDocument.AppendChild(xmlDeclaration);
                var node1 = xmlDocument.CreateNode(XmlNodeType.Element, "root", "");
                xmlDocument.AppendChild(node1);
                foreach (var index in orderedKeys)
                {
                    var node2 = xmlDocument.CreateNode(XmlNodeType.Element, "item", "");
                    var attribute1 = xmlDocument.CreateAttribute("name");
                    attribute1.Value = index;
                    var attribute2 = xmlDocument.CreateAttribute("value");
                    attribute2.Value = settingsHash[index];
                    node2.Attributes.Append(attribute1);
                    node2.Attributes.Append(attribute2);
                    node1.AppendChild(node2);
                }
                return xmlDocument;
            }
        }

        public string[] SettingNames
        {
            get
            {
                var array = new string[orderedKeys.Count];
                orderedKeys.CopyTo(array, 0);
                return array;
            }
        }

        public void loadSettings(string pathToSettings)
        {
            if (pathToSettings.Length <= 0)
            {
                throw new FileNotFoundException();
            }
            if (!new FileInfo(pathToSettings).Exists)
            {
                throw new FileNotFoundException();
            }
            var settingsAsXML = new XmlDocument();
            settingsAsXML.Load(pathToSettings);
            loadSettings(settingsAsXML);
        }

        public void loadSettings(XmlDocument settingsAsXML)
        {
            clearSettings();
            foreach (XmlNode xmlNode in settingsAsXML.DocumentElement.ChildNodes)
            {
                if (xmlNode.Name == "item" & xmlNode.Attributes.Count == 2 &&
                    xmlNode.Attributes[0].Name == "name" & xmlNode.Attributes[1].Name == "value")
                {
                    var name = xmlNode.Attributes["name"].Value;
                    var str = xmlNode.Attributes["value"].Value;
                    if (name.Length > 0)
                    {
                        addSetting(name, str);
                    }
                }
            }
        }

        public void addSetting(string name, string value)
        {
            var str = MakeCaseInsensitive.TransformInput(name);
            if (str.Length <= 0)
            {
                return;
            }
            removeSetting(str);
            orderedKeys.Add(str);
            settingsHash.Add(MakeCaseInsensitive.TransformInput(str), value);
        }

        public void removeSetting(string name)
        {
            var name1 = MakeCaseInsensitive.TransformInput(name);
            orderedKeys.Remove(name1);
            removeFromHash(name1);
        }

        private void removeFromHash(string name)
        {
            settingsHash.Remove(MakeCaseInsensitive.TransformInput(name));
        }

        public void updateSetting(string name, string value)
        {
            var str = MakeCaseInsensitive.TransformInput(name);
            if (!orderedKeys.Contains(str))
            {
                return;
            }
            removeFromHash(str);
            settingsHash.Add(MakeCaseInsensitive.TransformInput(str), value);
        }

        public void clearSettings()
        {
            orderedKeys.Clear();
            settingsHash.Clear();
        }

        public string grabSetting(string name)
        {
            var name1 = MakeCaseInsensitive.TransformInput(name);
            if (containsSettingCalled(name1))
            {
                return settingsHash[name1];
            }
            return string.Empty;
        }

        public bool containsSettingCalled(string name)
        {
            var str = MakeCaseInsensitive.TransformInput(name);
            if (str.Length > 0)
            {
                return orderedKeys.Contains(str);
            }
            return false;
        }

        public void Clone(SettingsDictionary target)
        {
            foreach (var name in orderedKeys)
            {
                target.addSetting(name, grabSetting(name));
            }
        }
    }
}