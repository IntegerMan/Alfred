// ---------------------------------------------------------
// Bot.cs
// 
// Created on:      08/12/2015 at 9:45 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;
using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    public class Bot
    {
        public delegate void LogMessageDelegate();

        private readonly Dictionary<string, Assembly> LateBindingAssemblies = new Dictionary<string, Assembly>();
        private readonly List<string> LogBuffer = new List<string>();
        private Dictionary<string, TagHandler> CustomTags;
        public SettingsDictionary DefaultPredicates;
        public SettingsDictionary GenderSubstitutions;
        public SettingsDictionary GlobalSettings;
        public Node Graphmaster;
        public bool isAcceptingUserInput = true;
        public string LastLogMessage = string.Empty;
        public int MaxThatSize = 256;
        public SettingsDictionary Person2Substitutions;
        public SettingsDictionary PersonSubstitutions;
        public int Size;
        public List<string> Splitters = new List<string>();
        public DateTime StartedOn = DateTime.Now;
        public SettingsDictionary Substitutions;
        public bool TrustAIML = true;

        public Bot()
        {
            setup();
        }

        private int MaxLogBufferSize
        {
            get { return Convert.ToInt32(GlobalSettings.grabSetting("maxlogbuffersize")); }
        }

        private string NotAcceptingUserInputMessage
        {
            get { return GlobalSettings.grabSetting("notacceptinguserinputmessage"); }
        }

        public double TimeOut
        {
            get { return Convert.ToDouble(GlobalSettings.grabSetting("timeout")); }
        }

        public string TimeOutMessage
        {
            get { return GlobalSettings.grabSetting("timeoutmessage"); }
        }

        public CultureInfo Locale
        {
            get { return new CultureInfo(GlobalSettings.grabSetting("culture")); }
        }

        public Regex Strippers
        {
            get { return new Regex(GlobalSettings.grabSetting("stripperregex"), RegexOptions.IgnorePatternWhitespace); }
        }

        public string AdminEmail
        {
            get { return GlobalSettings.grabSetting("adminemail"); }
            set
            {
                if (value.Length > 0)
                {
                    if (
                        !new Regex(
                             "^(([^<>()[\\]\\\\.,;:\\s@\\\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\\\"]+)*)|(\\\".+\\\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$")
                             .IsMatch(value))
                    {
                        throw new Exception("The AdminEmail is not a valid email address");
                    }
                    GlobalSettings.addSetting("adminemail", value);
                }
                else
                {
                    GlobalSettings.addSetting("adminemail", "");
                }
            }
        }

        public bool IsLogging
        {
            get { return GlobalSettings.grabSetting("islogging").ToLower() == "true"; }
        }

        public bool WillCallHome
        {
            get { return GlobalSettings.grabSetting("willcallhome").ToLower() == "true"; }
        }

        public Gender Sex
        {
            get
            {
                Gender gender;
                switch (Convert.ToInt32(GlobalSettings.grabSetting("gender")))
                {
                    case -1:
                        gender = Gender.Unknown;
                        break;
                    case 0:
                        gender = Gender.Female;
                        break;
                    case 1:
                        gender = Gender.Male;
                        break;
                    default:
                        gender = Gender.Unknown;
                        break;
                }
                return gender;
            }
        }

        public string PathToAIML
        {
            get { return Path.Combine(Environment.CurrentDirectory, GlobalSettings.grabSetting("aimldirectory")); }
        }

        public string PathToConfigFiles
        {
            get { return Path.Combine(Environment.CurrentDirectory, GlobalSettings.grabSetting("configdirectory")); }
        }

        public string PathToLogs
        {
            get { return Path.Combine(Environment.CurrentDirectory, GlobalSettings.grabSetting("logdirectory")); }
        }

        public event LogMessageDelegate WrittenToLog;

        public void loadAIMLFromFiles()
        {
            new AIMLLoader(this).loadAIML();
        }

        public void loadAIMLFromXML(XmlDocument newAIML, string filename)
        {
            new AIMLLoader(this).loadAIMLFromXML(newAIML, filename);
        }

        private void setup()
        {
            GlobalSettings = new SettingsDictionary(this);
            GenderSubstitutions = new SettingsDictionary(this);
            Person2Substitutions = new SettingsDictionary(this);
            PersonSubstitutions = new SettingsDictionary(this);
            Substitutions = new SettingsDictionary(this);
            DefaultPredicates = new SettingsDictionary(this);
            CustomTags = new Dictionary<string, TagHandler>();
            Graphmaster = new Node();
        }

        public void loadSettings()
        {
            loadSettings(Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "Settings.xml")));
        }

        public void loadSettings(string pathToSettings)
        {
            GlobalSettings.loadSettings(pathToSettings);
            if (!GlobalSettings.containsSettingCalled("version"))
            {
                GlobalSettings.addSetting("version", Environment.Version.ToString());
            }
            if (!GlobalSettings.containsSettingCalled("name"))
            {
                GlobalSettings.addSetting("name", "Unknown");
            }
            if (!GlobalSettings.containsSettingCalled("botmaster"))
            {
                GlobalSettings.addSetting("botmaster", "Unknown");
            }
            if (!GlobalSettings.containsSettingCalled("master"))
            {
                GlobalSettings.addSetting("botmaster", "Unknown");
            }
            if (!GlobalSettings.containsSettingCalled("author"))
            {
                GlobalSettings.addSetting("author", "Nicholas H.Tollervey");
            }
            if (!GlobalSettings.containsSettingCalled("location"))
            {
                GlobalSettings.addSetting("location", "Unknown");
            }
            if (!GlobalSettings.containsSettingCalled("gender"))
            {
                GlobalSettings.addSetting("gender", "-1");
            }
            if (!GlobalSettings.containsSettingCalled("birthday"))
            {
                GlobalSettings.addSetting("birthday", "2006/11/08");
            }
            if (!GlobalSettings.containsSettingCalled("birthplace"))
            {
                GlobalSettings.addSetting("birthplace", "Towcester, Northamptonshire, UK");
            }
            if (!GlobalSettings.containsSettingCalled("website"))
            {
                GlobalSettings.addSetting("website", "http://sourceforge.net/projects/aimlbot");
            }
            if (GlobalSettings.containsSettingCalled("adminemail"))
            {
                AdminEmail = GlobalSettings.grabSetting("adminemail");
            }
            else
            {
                GlobalSettings.addSetting("adminemail", "");
            }
            if (!GlobalSettings.containsSettingCalled("islogging"))
            {
                GlobalSettings.addSetting("islogging", "False");
            }
            if (!GlobalSettings.containsSettingCalled("willcallhome"))
            {
                GlobalSettings.addSetting("willcallhome", "False");
            }
            if (!GlobalSettings.containsSettingCalled("timeout"))
            {
                GlobalSettings.addSetting("timeout", "2000");
            }
            if (!GlobalSettings.containsSettingCalled("timeoutmessage"))
            {
                GlobalSettings.addSetting("timeoutmessage", "ERROR: The request has timed out.");
            }
            if (!GlobalSettings.containsSettingCalled("culture"))
            {
                GlobalSettings.addSetting("culture", "en-US");
            }
            if (!GlobalSettings.containsSettingCalled("splittersfile"))
            {
                GlobalSettings.addSetting("splittersfile", "Splitters.xml");
            }
            if (!GlobalSettings.containsSettingCalled("person2substitutionsfile"))
            {
                GlobalSettings.addSetting("person2substitutionsfile", "Person2Substitutions.xml");
            }
            if (!GlobalSettings.containsSettingCalled("personsubstitutionsfile"))
            {
                GlobalSettings.addSetting("personsubstitutionsfile", "PersonSubstitutions.xml");
            }
            if (!GlobalSettings.containsSettingCalled("gendersubstitutionsfile"))
            {
                GlobalSettings.addSetting("gendersubstitutionsfile", "GenderSubstitutions.xml");
            }
            if (!GlobalSettings.containsSettingCalled("defaultpredicates"))
            {
                GlobalSettings.addSetting("defaultpredicates", "DefaultPredicates.xml");
            }
            if (!GlobalSettings.containsSettingCalled("substitutionsfile"))
            {
                GlobalSettings.addSetting("substitutionsfile", "Substitutions.xml");
            }
            if (!GlobalSettings.containsSettingCalled("aimldirectory"))
            {
                GlobalSettings.addSetting("aimldirectory", "aiml");
            }
            if (!GlobalSettings.containsSettingCalled("configdirectory"))
            {
                GlobalSettings.addSetting("configdirectory", "config");
            }
            if (!GlobalSettings.containsSettingCalled("logdirectory"))
            {
                GlobalSettings.addSetting("logdirectory", "logs");
            }
            if (!GlobalSettings.containsSettingCalled("maxlogbuffersize"))
            {
                GlobalSettings.addSetting("maxlogbuffersize", "64");
            }
            if (!GlobalSettings.containsSettingCalled("notacceptinguserinputmessage"))
            {
                GlobalSettings.addSetting("notacceptinguserinputmessage",
                                          "This bot is currently set to not accept user input.");
            }
            if (!GlobalSettings.containsSettingCalled("stripperregex"))
            {
                GlobalSettings.addSetting("stripperregex", "[^0-9a-zA-Z]");
            }
            Person2Substitutions.loadSettings(Path.Combine(PathToConfigFiles,
                                                           GlobalSettings.grabSetting("person2substitutionsfile")));
            PersonSubstitutions.loadSettings(Path.Combine(PathToConfigFiles,
                                                          GlobalSettings.grabSetting("personsubstitutionsfile")));
            GenderSubstitutions.loadSettings(Path.Combine(PathToConfigFiles,
                                                          GlobalSettings.grabSetting("gendersubstitutionsfile")));
            DefaultPredicates.loadSettings(Path.Combine(PathToConfigFiles,
                                                        GlobalSettings.grabSetting("defaultpredicates")));
            Substitutions.loadSettings(Path.Combine(PathToConfigFiles, GlobalSettings.grabSetting("substitutionsfile")));
            loadSplitters(Path.Combine(PathToConfigFiles, GlobalSettings.grabSetting("splittersfile")));
        }

        private void loadSplitters(string pathToSplitters)
        {
            if (new FileInfo(pathToSplitters).Exists)
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(pathToSplitters);
                if (xmlDocument.ChildNodes.Count == 2 && xmlDocument.LastChild.HasChildNodes)
                {
                    foreach (XmlNode xmlNode in xmlDocument.LastChild.ChildNodes)
                    {
                        if (xmlNode.Name == "item" & xmlNode.Attributes.Count == 1)
                        {
                            Splitters.Add(xmlNode.Attributes["value"].Value);
                        }
                    }
                }
            }
            if (Splitters.Count != 0)
            {
                return;
            }
            Splitters.Add(".");
            Splitters.Add("!");
            Splitters.Add("?");
            Splitters.Add(";");
        }

        public void writeToLog(string message)
        {
            LastLogMessage = message;
            if (IsLogging)
            {
                LogBuffer.Add(DateTime.Now + ": " + message + Environment.NewLine);
                if (LogBuffer.Count > MaxLogBufferSize - 1)
                {
                    var directoryInfo = new DirectoryInfo(PathToLogs);
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    var fileInfo = new FileInfo(Path.Combine(PathToLogs, DateTime.Now.ToString("yyyyMMdd") + ".log"));
                    var streamWriter = fileInfo.Exists ? fileInfo.AppendText() : fileInfo.CreateText();
                    foreach (var str in LogBuffer)
                    {
                        streamWriter.WriteLine(str);
                    }
                    streamWriter.Close();
                    LogBuffer.Clear();
                }
            }
            if (Equals(null, WrittenToLog))
            {
                return;
            }
            WrittenToLog();
        }

        public Result Chat(string rawInput, string UserGUID)
        {
            return Chat(new Request(rawInput, new User(UserGUID, this), this));
        }

        public Result Chat(Request request)
        {
            var result = new Result(request.user, this, request);
            if (isAcceptingUserInput)
            {
                var aimlLoader = new AIMLLoader(this);
                foreach (var pattern in new SplitIntoSentences(this).Transform(request.rawInput))
                {
                    result.InputSentences.Add(pattern);
                    var str = aimlLoader.generatePath(pattern, request.user.getLastBotOutput(), request.user.Topic, true);
                    result.NormalizedPaths.Add(str);
                }
                foreach (var str in result.NormalizedPaths)
                {
                    var query = new SubQuery(str);
                    query.Template = Graphmaster.evaluate(str, query, request, MatchState.UserInput, new StringBuilder());
                    result.SubQueries.Add(query);
                }
                foreach (var query in result.SubQueries)
                {
                    if (query.Template.Length > 0)
                    {
                        try
                        {
                            var str = processNode(AimlTagHandler.getNode(query.Template),
                                                  query,
                                                  request,
                                                  result,
                                                  request.user);
                            if (str.Length > 0)
                            {
                                result.OutputSentences.Add(str);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (WillCallHome)
                            {
                                phoneHome(ex.Message, request);
                            }
                            writeToLog("WARNING! A problem was encountered when trying to process the input: " +
                                       request.rawInput + " with the template: \"" + query.Template + "\"");
                        }
                    }
                }
            }
            else
            {
                result.OutputSentences.Add(NotAcceptingUserInputMessage);
            }
            result.Duration = DateTime.Now - request.StartedOn;
            request.user.addResult(result);
            return result;
        }

        private string processNode(XmlNode node, SubQuery query, Request request, Result result, User user)
        {
            if (request.StartedOn.AddMilliseconds(request.bot.TimeOut) < DateTime.Now)
            {
                request.bot.writeToLog("WARNING! Request timeout. User: " + request.user.UserID + " raw input: \"" +
                                       request.rawInput + "\" processing template: \"" + query.Template + "\"");
                request.hasTimedOut = true;
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
                        stringBuilder.Append(processNode(node1, query, request, result, user));
                    }
                }
                return stringBuilder.ToString();
            }
            var AimlTagHandler = getBespokeTags(user, query, request, result, node);
            if (Equals(null, AimlTagHandler))
            {
                switch (str)
                {
                    case "bot":
                        AimlTagHandler = new bot(this, user, query, request, result, node);
                        break;
                    case "condition":
                        AimlTagHandler = new condition(this, user, query, request, result, node);
                        break;
                    case "date":
                        AimlTagHandler = new date(this, user, query, request, result, node);
                        break;
                    case "formal":
                        AimlTagHandler = new formal(this, user, query, request, result, node);
                        break;
                    case "gender":
                        AimlTagHandler = new gender(this, user, query, request, result, node);
                        break;
                    case "get":
                        AimlTagHandler = new get(this, user, query, request, result, node);
                        break;
                    case "gossip":
                        AimlTagHandler = new gossip(this, user, query, request, result, node);
                        break;
                    case "id":
                        AimlTagHandler = new id(this, user, query, request, result, node);
                        break;
                    case "input":
                        AimlTagHandler = new input(this, user, query, request, result, node);
                        break;
                    case "javascript":
                        AimlTagHandler = new javascript(this, user, query, request, result, node);
                        break;
                    case "learn":
                        AimlTagHandler = new learn(this, user, query, request, result, node);
                        break;
                    case "lowercase":
                        AimlTagHandler = new lowercase(this, user, query, request, result, node);
                        break;
                    case "person":
                        AimlTagHandler = new person(this, user, query, request, result, node);
                        break;
                    case "person2":
                        AimlTagHandler = new person2(this, user, query, request, result, node);
                        break;
                    case "random":
                        AimlTagHandler = new random(this, user, query, request, result, node);
                        break;
                    case "sentence":
                        AimlTagHandler = new sentence(this, user, query, request, result, node);
                        break;
                    case "set":
                        AimlTagHandler = new set(this, user, query, request, result, node);
                        break;
                    case "size":
                        AimlTagHandler = new size(this, user, query, request, result, node);
                        break;
                    case "sr":
                        AimlTagHandler = new sr(this, user, query, request, result, node);
                        break;
                    case "srai":
                        AimlTagHandler = new srai(this, user, query, request, result, node);
                        break;
                    case "star":
                        AimlTagHandler = new star(this, user, query, request, result, node);
                        break;
                    case "system":
                        AimlTagHandler = new system(this, user, query, request, result, node);
                        break;
                    case "that":
                        AimlTagHandler = new that(this, user, query, request, result, node);
                        break;
                    case "thatstar":
                        AimlTagHandler = new thatstar(this, user, query, request, result, node);
                        break;
                    case "think":
                        AimlTagHandler = new think(this, user, query, request, result, node);
                        break;
                    case "topicstar":
                        AimlTagHandler = new topicstar(this, user, query, request, result, node);
                        break;
                    case "uppercase":
                        AimlTagHandler = new uppercase(this, user, query, request, result, node);
                        break;
                    case "version":
                        AimlTagHandler = new version(this, user, query, request, result, node);
                        break;
                    default:
                        AimlTagHandler = null;
                        break;
                }
            }
            if (Equals(null, AimlTagHandler))
            {
                return node.InnerText;
            }
            if (AimlTagHandler.isRecursive)
            {
                if (node.HasChildNodes)
                {
                    foreach (XmlNode node1 in node.ChildNodes)
                    {
                        if (node1.NodeType != XmlNodeType.Text)
                        {
                            node1.InnerXml = processNode(node1, query, request, result, user);
                        }
                    }
                }
                return AimlTagHandler.Transform();
            }
            var node2 = AimlTagHandler.getNode("<node>" + AimlTagHandler.Transform() + "</node>");
            if (!node2.HasChildNodes)
            {
                return node2.InnerXml;
            }
            var stringBuilder1 = new StringBuilder();
            foreach (XmlNode node1 in node2.ChildNodes)
            {
                stringBuilder1.Append(processNode(node1, query, request, result, user));
            }
            return stringBuilder1.ToString();
        }

        public AimlTagHandler getBespokeTags(User user, SubQuery query, Request request, Result result, XmlNode node)
        {
            if (!CustomTags.ContainsKey(node.Name.ToLower()))
            {
                return null;
            }

            var customTag = CustomTags[node.Name.ToLower()];

            // TODO: It'd be very nice not to use .Instantiate.
            var tagHandler = customTag.Instantiate(LateBindingAssemblies);

            if (Equals(null, tagHandler))
            {
                return null;
            }

            tagHandler.user = user;
            tagHandler.query = query;
            tagHandler.request = request;
            tagHandler.result = result;
            tagHandler.templateNode = node;
            tagHandler.Bot = this;

            return tagHandler;
        }

        public void saveToBinaryFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            var fileStream = File.Create(path);
            new BinaryFormatter().Serialize(fileStream, Graphmaster);
            fileStream.Close();
        }

        public void loadFromBinaryFile(string path)
        {
            var fileStream = File.OpenRead(path);
            Graphmaster = (Node) new BinaryFormatter().Deserialize(fileStream);
            fileStream.Close();
        }

        public void loadCustomTagHandlers(string pathToDLL)
        {
            var assembly = Assembly.LoadFrom(pathToDLL);
            var types = assembly.GetTypes();
            for (var index = 0; index < types.Length; ++index)
            {
                foreach (var obj in types[index].GetCustomAttributes(false))
                {
                    if (obj is CustomTagAttribute)
                    {
                        if (!LateBindingAssemblies.ContainsKey(assembly.FullName))
                        {
                            LateBindingAssemblies.Add(assembly.FullName, assembly);
                        }
                        var tagHandler = new TagHandler();
                        tagHandler.AssemblyName = assembly.FullName;
                        tagHandler.ClassName = types[index].FullName;
                        tagHandler.TagName = types[index].Name.ToLower();
                        if (CustomTags.ContainsKey(tagHandler.TagName))
                        {
                            throw new Exception("ERROR! Unable to add the custom tag: <" + tagHandler.TagName +
                                                ">, found in: " + pathToDLL +
                                                " as a handler for this tag already exists.");
                        }
                        CustomTags.Add(tagHandler.TagName, tagHandler);
                    }
                }
            }
        }

        public void phoneHome(string errorMessage, Request request)
        {
            var message = new MailMessage("donotreply@aimlbot.com", AdminEmail);
            message.Subject = "WARNING! AIMLBot has encountered a problem...";
            var str1 =
                "Dear Botmaster,\r\n\r\nThis is an automatically generated email to report errors with your bot.\r\n\r\nAt *TIME* the bot encountered the following error:\r\n\r\n\"*MESSAGE*\"\r\n\r\nwhilst processing the following input:\r\n\r\n\"*RAWINPUT*\"\r\n\r\nfrom the user with an id of: *USER*\r\n\r\nThe normalized paths generated by the raw input were as follows:\r\n\r\n*PATHS*\r\n\r\nPlease check your AIML!\r\n\r\nRegards,\r\n\r\nThe AIMLbot program.\r\n"
                    .Replace("*TIME*", DateTime.Now.ToString())
                    .Replace("*MESSAGE*", errorMessage)
                    .Replace("*RAWINPUT*", request.rawInput)
                    .Replace("*USER*", request.user.UserID);
            var stringBuilder = new StringBuilder();
            foreach (var str2 in request.result.NormalizedPaths)
            {
                stringBuilder.Append(str2 + Environment.NewLine);
            }
            var str3 = str1.Replace("*PATHS*", stringBuilder.ToString());
            message.Body = str3;
            message.IsBodyHtml = false;
            try
            {
                if (message.To.Count <= 0)
                {
                    return;
                }
                new SmtpClient().Send(message);
            }
            catch
            {
            }
        }
    }
}