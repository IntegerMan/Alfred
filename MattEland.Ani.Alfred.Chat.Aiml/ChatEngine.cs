﻿// ---------------------------------------------------------
// ChatEngine.cs
// 
// Created on:      08/12/2015 at 9:45 PM
// Last Modified:   08/13/2015 at 3:13 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using JetBrains.Annotations;
using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    /// <summary>
    ///     ChatEngine represents a ChatBot, its parsing capabilities, its knowledge library, etc.
    /// </summary>
    /// <remarks>
    ///     TODO: This is a very monolithic class that needs to have many more utility classes and
    ///     have its responsibilities shared. Of course, documentation is important too.
    /// </remarks>
    public class ChatEngine
    {
        public delegate void LogMessageDelegate();

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        [CanBeNull]
        public IConsole Logger { get; set; }

        [NotNull]
        public SettingsDictionary DefaultPredicates;
        public SettingsDictionary GenderSubstitutions;
        public SettingsDictionary GlobalSettings;

        public Node Graphmaster;

        public bool isAcceptingUserInput = true;
        public int MaxThatSize = 256;
        public SettingsDictionary Person2Substitutions;
        public SettingsDictionary PersonSubstitutions;
        public int Size;

        [NotNull, ItemNotNull]
        public List<string> Splitters = new List<string>();
        public SettingsDictionary Substitutions;

        public bool TrustAiml = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatEngine" /> class.
        /// </summary>
        public ChatEngine()
        {
            setup();
        }

        private string NotAcceptingUserInputMessage
        {
            get { return GlobalSettings.GetValue("notacceptinguserinputmessage"); }
        }

        public double TimeOut
        {
            get { return Convert.ToDouble(GlobalSettings.GetValue("timeout")); }
        }

        /// <summary>
        /// Gets the timeout message.
        /// This message is used when a request takes too long and times out.
        /// </summary>
        /// <value>The timeout message.</value>
        [NotNull]
        public string TimeOutMessage
        {
            get
            {
                var timeoutMessage = GlobalSettings.GetValue("timeoutmessage");

                if (string.IsNullOrEmpty(TimeOutMessage))
                {
                    timeoutMessage = "Your request has timed out. Please try again or phrase your sentence differently.";
                }

                // ReSharper disable once AssignNullToNotNullAttribute
                return timeoutMessage;
            }
        }

        /// <summary>
        /// Gets the locale of this instance.
        /// </summary>
        /// <value>The locale.</value>
        public CultureInfo Locale
        {
            get
            {
                /* TODO: I'm not sure I like reading from GlobalSettings.
                Maybe for a web deploy, but CurrentCulture works great. */

                var cultureValue = GlobalSettings.GetValue("culture");
                if (string.IsNullOrEmpty(cultureValue))
                {
                    return CultureInfo.CurrentCulture;
                }

                return new CultureInfo(cultureValue);
            }
        }

        public Regex Strippers
        {
            get { return new Regex(GlobalSettings.GetValue("stripperregex"), RegexOptions.IgnorePatternWhitespace); }
        }

        public string AdminEmail
        {
            get { return GlobalSettings.GetValue("adminemail"); }
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
                    GlobalSettings.Add("adminemail", value);
                }
                else
                {
                    GlobalSettings.Add("adminemail", "");
                }
            }
        }

        public bool WillCallHome
        {
            get { return GlobalSettings.GetValue("willcallhome").ToLower() == "true"; }
        }

        public Gender Sex
        {
            get
            {
                Gender gender;
                switch (Convert.ToInt32(GlobalSettings.GetValue("gender")))
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
            get { return Path.Combine(Environment.CurrentDirectory, GlobalSettings.GetValue("aimldirectory")); }
        }

        public string PathToConfigFiles
        {
            get { return Path.Combine(Environment.CurrentDirectory, GlobalSettings.GetValue("configdirectory")); }
        }

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
            GlobalSettings = new SettingsDictionary();
            GenderSubstitutions = new SettingsDictionary();
            Person2Substitutions = new SettingsDictionary();
            PersonSubstitutions = new SettingsDictionary();
            Substitutions = new SettingsDictionary();
            DefaultPredicates = new SettingsDictionary();
            Graphmaster = new Node();
        }

        public void loadSettings()
        {
            loadSettings(Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "Settings.xml")));
        }

        public void loadSettings(string pathToSettings)
        {
            GlobalSettings.Load(pathToSettings);
            if (!GlobalSettings.Contains("version"))
            {
                GlobalSettings.Add("version", Environment.Version.ToString());
            }
            if (!GlobalSettings.Contains("name"))
            {
                GlobalSettings.Add("name", "Unknown");
            }
            if (!GlobalSettings.Contains("botmaster"))
            {
                GlobalSettings.Add("botmaster", "Unknown");
            }
            if (!GlobalSettings.Contains("master"))
            {
                GlobalSettings.Add("botmaster", "Unknown");
            }
            if (!GlobalSettings.Contains("author"))
            {
                GlobalSettings.Add("author", "Nicholas H.Tollervey");
            }
            if (!GlobalSettings.Contains("location"))
            {
                GlobalSettings.Add("location", "Unknown");
            }
            if (!GlobalSettings.Contains("gender"))
            {
                GlobalSettings.Add("gender", "-1");
            }
            if (!GlobalSettings.Contains("birthday"))
            {
                GlobalSettings.Add("birthday", "2006/11/08");
            }
            if (!GlobalSettings.Contains("birthplace"))
            {
                GlobalSettings.Add("birthplace", "Towcester, Northamptonshire, UK");
            }
            if (!GlobalSettings.Contains("website"))
            {
                GlobalSettings.Add("website", "http://sourceforge.net/projects/aimlbot");
            }
            if (GlobalSettings.Contains("adminemail"))
            {
                AdminEmail = GlobalSettings.GetValue("adminemail");
            }
            else
            {
                GlobalSettings.Add("adminemail", "");
            }
            if (!GlobalSettings.Contains("islogging"))
            {
                GlobalSettings.Add("islogging", "False");
            }
            if (!GlobalSettings.Contains("willcallhome"))
            {
                GlobalSettings.Add("willcallhome", "False");
            }
            if (!GlobalSettings.Contains("timeout"))
            {
                GlobalSettings.Add("timeout", "2000");
            }
            if (!GlobalSettings.Contains("timeoutmessage"))
            {
                GlobalSettings.Add("timeoutmessage", "ERROR: The request has timed out.");
            }
            if (!GlobalSettings.Contains("culture"))
            {
                GlobalSettings.Add("culture", "en-US");
            }
            if (!GlobalSettings.Contains("splittersfile"))
            {
                GlobalSettings.Add("splittersfile", "Splitters.xml");
            }
            if (!GlobalSettings.Contains("person2substitutionsfile"))
            {
                GlobalSettings.Add("person2substitutionsfile", "Person2Substitutions.xml");
            }
            if (!GlobalSettings.Contains("personsubstitutionsfile"))
            {
                GlobalSettings.Add("personsubstitutionsfile", "PersonSubstitutions.xml");
            }
            if (!GlobalSettings.Contains("gendersubstitutionsfile"))
            {
                GlobalSettings.Add("gendersubstitutionsfile", "GenderSubstitutions.xml");
            }
            if (!GlobalSettings.Contains("defaultpredicates"))
            {
                GlobalSettings.Add("defaultpredicates", "DefaultPredicates.xml");
            }
            if (!GlobalSettings.Contains("substitutionsfile"))
            {
                GlobalSettings.Add("substitutionsfile", "Substitutions.xml");
            }
            if (!GlobalSettings.Contains("aimldirectory"))
            {
                GlobalSettings.Add("aimldirectory", "aiml");
            }
            if (!GlobalSettings.Contains("configdirectory"))
            {
                GlobalSettings.Add("configdirectory", "config");
            }
            if (!GlobalSettings.Contains("logdirectory"))
            {
                GlobalSettings.Add("logdirectory", "logs");
            }
            if (!GlobalSettings.Contains("maxlogbuffersize"))
            {
                GlobalSettings.Add("maxlogbuffersize", "64");
            }
            if (!GlobalSettings.Contains("notacceptinguserinputmessage"))
            {
                GlobalSettings.Add("notacceptinguserinputmessage",
                                   "This ChatEngine is currently set to not accept user input.");
            }
            if (!GlobalSettings.Contains("stripperregex"))
            {
                GlobalSettings.Add("stripperregex", "[^0-9a-zA-Z]");
            }
            Person2Substitutions.Load(Path.Combine(PathToConfigFiles,
                                                   GlobalSettings.GetValue("person2substitutionsfile")));
            PersonSubstitutions.Load(Path.Combine(PathToConfigFiles,
                                                  GlobalSettings.GetValue("personsubstitutionsfile")));
            GenderSubstitutions.Load(Path.Combine(PathToConfigFiles,
                                                  GlobalSettings.GetValue("gendersubstitutionsfile")));
            DefaultPredicates.Load(Path.Combine(PathToConfigFiles,
                                                GlobalSettings.GetValue("defaultpredicates")));
            Substitutions.Load(Path.Combine(PathToConfigFiles, GlobalSettings.GetValue("substitutionsfile")));
            loadSplitters(Path.Combine(PathToConfigFiles, GlobalSettings.GetValue("splittersfile")));
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

        /// <summary>
        /// Logs the specified message to the logger.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        public void Log(string message, LogLevel level)
        {
            Logger?.Log("ChatEngine", message, level);
        }

        public Result Chat(string rawInput, string UserGUID)
        {
            return Chat(new Request(rawInput, new User(UserGUID, this), this));
        }

        public Result Chat(Request request)
        {
            var result = new Result(request.User, this, request);
            if (isAcceptingUserInput)
            {
                var aimlLoader = new AimlLoader(this);
                foreach (var pattern in SplitSentenceHelper.Split(request.RawInput, this))
                {
                    result.InputSentences.Add(pattern);
                    var str = aimlLoader.BuildPathString(pattern,
                                                         request.User.LastChatOutput,
                                                         request.User.Topic,
                                                         true);
                    result.NormalizedPaths.Add(str);
                }
                foreach (var str in result.NormalizedPaths)
                {
                    var query = new SubQuery(str);
                    query.Template = Graphmaster.Evaluate(str, query, request, MatchState.UserInput, new StringBuilder());
                    result.SubQueries.Add(query);
                }
                foreach (var query in result.SubQueries)
                {
                    if (query.Template.Length > 0)
                    {
                        try
                        {
                            var str = processNode(AimlTagHandler.GetNode(query.Template),
                                                  query,
                                                  request,
                                                  result,
                                                  request.User);
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
                            Log("A problem was encountered when trying to process the input: " +
                                       request.RawInput + " with the template: \"" + query.Template + "\"", LogLevel.Warning);
                        }
                    }
                }
            }
            else
            {
                result.OutputSentences.Add(NotAcceptingUserInputMessage);
            }
            result.Completed();
            request.User.AddResult(result);
            return result;
        }

        private string processNode(XmlNode node, SubQuery query, Request request, Result result, User user)
        {
            if (request.CheckForTimedOut())
            {
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

            if (handler.IsRecursive)
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
            var node2 = AimlTagHandler.GetNode("<node>" + handler.Transform() + "</node>");
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
        ///     Builds the tag handler from the given parameters.
        /// </summary>
        /// <remarks>
        ///     TODO: Document parameters
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
                    .Replace("*RAWINPUT*", request.RawInput)
                    .Replace("*USER*", request.User.Id);
            var stringBuilder = new StringBuilder();
            foreach (var str2 in request.Result.NormalizedPaths)
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
        ///     Adds the category to the graph.
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
                Log(string.Format(Locale,
                                         "Attempted to load a new category with an empty pattern where the directoryPath = {0} and template = {1} produced by a category in the file: {2}",
                                         path,
                                         node.OuterXml,
                                         filename), LogLevel.Warning);
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
                Log(string.Format(Locale,
                                         "Failed to load a new category into the graphmaster where the directoryPath = {0} and template = {1} produced by a category in the file: {2}",
                                         path,
                                         node.OuterXml,
                                         filename), LogLevel.Error);
            }
        }
    }
}