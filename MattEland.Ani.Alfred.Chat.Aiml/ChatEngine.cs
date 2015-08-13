// ---------------------------------------------------------
// ChatEngine.cs
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

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;
using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    /// <summary>
    /// ChatEngine represents a ChatBot, its parsing capabilities, its knowledge library, etc.
    /// </summary>
    /// <remarks>
    /// TODO: This is a very monolithic class that needs to have many more utility classes and
    /// have its responsibilities shared. Of course, documentation is important too.
    /// </remarks>
    public class ChatEngine
    {
        public delegate void LogMessageDelegate();

        private readonly List<string> LogBuffer = new List<string>();
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
        public bool TrustAiml = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatEngine"/> class.
        /// </summary>
        public ChatEngine()
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

        public string AimlDirectoryPath
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
            new AimlLoader(this).LoadAiml();
        }

        public void loadAIMLFromXML(XmlDocument newAIML, string filename)
        {
            new AimlLoader(this).LoadAimlFromXml(newAIML, filename);
        }

        private void setup()
        {
            GlobalSettings = new SettingsDictionary(this);
            GenderSubstitutions = new SettingsDictionary(this);
            Person2Substitutions = new SettingsDictionary(this);
            PersonSubstitutions = new SettingsDictionary(this);
            Substitutions = new SettingsDictionary(this);
            DefaultPredicates = new SettingsDictionary(this);
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
                                          "This ChatEngine is currently set to not accept user input.");
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
                var aimlLoader = new AimlLoader(this);
                foreach (var pattern in new SplitIntoSentences(this).Transform(request.rawInput))
                {
                    result.InputSentences.Add(pattern);
                    var str = aimlLoader.BuildPathString(pattern, request.user.getLastBotOutput(), request.user.Topic, true);
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
            if (request.StartedOn.AddMilliseconds(request.chatEngine.TimeOut) < DateTime.Now)
            {
                request.chatEngine.writeToLog("WARNING! Request timeout. User: " + request.user.UserID + " raw input: \"" +
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

            var handler = BuildTagHandler(node, query, request, result, user, str);

            if (Equals(null, handler))
            {
                return node.InnerText;
            }

            if (handler.isRecursive)
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
                return handler.Transform();
            }
            var node2 = AimlTagHandler.getNode("<node>" + handler.Transform() + "</node>");
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

        /// <summary>
        /// Builds the tag handler from the given parameters.
        /// </summary>
        /// <remarks>
        /// TODO: Document parameters
        /// </remarks>
        /// <returns>The tag handler.</returns>
        [CanBeNull]
        private AimlTagHandler BuildTagHandler(XmlNode node,
                                               SubQuery query,
                                               Request request,
                                               Result result,
                                               User user,
                                               [CanBeNull] string tagName)
        {
            switch (tagName?.ToLowerInvariant())
            {
                case "ChatEngine":
                    return new bot(this, user, query, request, result, node);
                case "condition":
                    return new condition(this, user, query, request, result, node);
                case "date":
                    return new date(this, user, query, request, result, node);
                case "formal":
                    return new formal(this, user, query, request, result, node);
                case "gender":
                    return new gender(this, user, query, request, result, node);
                case "get":
                    return new get(this, user, query, request, result, node);
                case "gossip":
                    return new gossip(this, user, query, request, result, node);
                case "id":
                    return new id(this, user, query, request, result, node);
                case "input":
                    return new input(this, user, query, request, result, node);
                case "javascript":
                    return new javascript(this, user, query, request, result, node);
                case "learn":
                    return new learn(this, user, query, request, result, node);
                case "lowercase":
                    return new lowercase(this, user, query, request, result, node);
                case "person":
                    return new person(this, user, query, request, result, node);
                case "person2":
                    return new person2(this, user, query, request, result, node);
                case "random":
                    return new random(this, user, query, request, result, node);
                case "sentence":
                    return new sentence(this, user, query, request, result, node);
                case "set":
                    return new set(this, user, query, request, result, node);
                case "size":
                    return new size(this, user, query, request, result, node);
                case "sr":
                    return new sr(this, user, query, request, result, node);
                case "srai":
                    return new srai(this, user, query, request, result, node);
                case "star":
                    return new star(this, user, query, request, result, node);
                case "system":
                    return new system(this, user, query, request, result, node);
                case "that":
                    return new that(this, user, query, request, result, node);
                case "thatstar":
                    return new thatstar(this, user, query, request, result, node);
                case "think":
                    return new think(this, user, query, request, result, node);
                case "topicstar":
                    return new topicstar(this, user, query, request, result, node);
                case "uppercase":
                    return new uppercase(this, user, query, request, result, node);
                case "version":
                    return new version(this, user, query, request, result, node);
            }

            return null;
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
            Graphmaster = (Node)new BinaryFormatter().Deserialize(fileStream);
            fileStream.Close();
        }

        public void phoneHome(string errorMessage, Request request)
        {
            var message = new MailMessage("donotreply@aimlbot.com", AdminEmail);
            message.Subject = "WARNING! AIMLBot has encountered a problem...";
            var str1 =
                "Dear Botmaster,\r\n\r\nThis is an automatically generated email to report errors with your ChatEngine.\r\n\r\nAt *TIME* the ChatEngine encountered the following error:\r\n\r\n\"*MESSAGE*\"\r\n\r\nwhilst processing the following input:\r\n\r\n\"*RAWINPUT*\"\r\n\r\nfrom the user with an id of: *USER*\r\n\r\nThe normalized paths generated by the raw input were as follows:\r\n\r\n*PATHS*\r\n\r\nPlease check your AIML!\r\n\r\nRegards,\r\n\r\nThe AIMLbot program.\r\n"
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

        /// <summary>
        /// Adds the category to the graph.
        /// </summary>
        /// <param name="node">The template node.</param>
        /// <param name="path">The path.</param>
        /// <param name="filename">The filename.</param>
        public void AddCategoryToGraph([NotNull] XmlNode node, [NotNull] string path, [NotNull] string filename)
        {
            //- Validate
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (string.IsNullOrEmpty(path))
            {
                writeToLog(string.Format(Locale,
                                  "WARNING! Attempted to load a new category with an empty pattern where the directoryPath = {0} and template = {1} produced by a category in the file: {2}",
                                  path,
                                  node.OuterXml,
                                  filename));
                return;
            }

            // Add it to the graph
            try
            {
                Graphmaster.AddCategory(path, node.OuterXml, filename);

                Size++;
            }
            catch
            {
                writeToLog(string.Format(Locale,
                                  "ERROR! Failed to load a new category into the graphmaster where the directoryPath = {0} and template = {1} produced by a category in the file: {2}",
                                  path,
                                  node.OuterXml,
                                  filename));
            }
        }
    }
}