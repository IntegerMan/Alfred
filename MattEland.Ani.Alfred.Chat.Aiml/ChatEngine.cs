// ---------------------------------------------------------
// ChatEngine.cs
// 
// Created on:      08/12/2015 at 9:45 PM
// Last Modified:   08/14/2015 at 1:53 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

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
        [NotNull]
        private readonly TagHandlerFactory _tagFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatEngine" /> class.
        /// </summary>
        public ChatEngine()
        {
            _tagFactory = new TagHandlerFactory(this);
            GlobalSettings = new SettingsDictionary();
            GenderSubstitutions = new SettingsDictionary();
            Person2Substitutions = new SettingsDictionary();
            PersonSubstitutions = new SettingsDictionary();
            Substitutions = new SettingsDictionary();
            DefaultPredicates = new SettingsDictionary();
            RootNode = new Node();
        }

        [NotNull]
        public SettingsDictionary DefaultPredicates { get; }

        /// <summary>
        /// Gets the gender substitutions dictionary. This is a collection of male and female pronouns and their
        /// replacement values to use when the "gender" AIML tag is present.
        /// </summary>
        /// <value>The gender substitutions dictionary.</value>
        [NotNull]
        public SettingsDictionary GenderSubstitutions { get; }

        [NotNull]
        public SettingsDictionary GlobalSettings { get; }

        /// <summary>
        ///     Gets or sets the root node of the Aiml knowledge graph.
        /// </summary>
        /// <value>The root node.</value>
        [NotNull]
        public Node RootNode { get; set; }

        public bool IsAcceptingUserInput { get; set; } = true;
        public int MaxThatSize { get; set; } = 256;

        [NotNull]
        public SettingsDictionary Person2Substitutions { get; }

        [NotNull]
        public SettingsDictionary PersonSubstitutions { get; }

        public int Size { get; private set; }

        [NotNull]
        [ItemNotNull]
        public List<string> Splitters { get; } = new List<string>();

        public SettingsDictionary Substitutions { get; }
        public bool TrustAiml { get; } = true;

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        [CanBeNull]
        public IConsole Logger { get; set; }

        private string NotAcceptingUserInputMessage
        {
            get { return GlobalSettings.GetValue("notacceptinguserinputmessage"); }
        }

        public double TimeOut
        {
            get { return Convert.ToDouble(GlobalSettings.GetValue("timeout")); }
        }

        /// <summary>
        ///     Gets the timeout message.
        ///     This message is used when a request takes too long and times out.
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
        ///     Gets the locale of this instance.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
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

        public string AimlDirectoryPath
        {
            get { return Path.Combine(Environment.CurrentDirectory, GlobalSettings.GetValue("aimldirectory")); }
        }

        public string PathToConfigFiles
        {
            get { return Path.Combine(Environment.CurrentDirectory, GlobalSettings.GetValue("configdirectory")); }
        }

        public void LoadAIMLFromFiles()
        {
            new AimlLoader(this).LoadAiml();
        }

        public void LoadAimlFile(XmlDocument newAIML, string filename)
        {
            new AimlLoader(this).LoadAimlFromXml(newAIML, filename);
        }

        public void LoadSettings()
        {
            LoadSettings(Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "Settings.xml")));
        }

        public void LoadSettings(string pathToSettings)
        {
            GlobalSettings.Load(pathToSettings);
            if (!GlobalSettings.Contains("version"))
            {
                GlobalSettings.Add("version", this.GetAssemblyVersion()?.ToString());
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
                GlobalSettings.Add("author", "Matt Eland");
            }
            if (!GlobalSettings.Contains("location"))
            {
                GlobalSettings.Add("location", "Unknown");
            }
            if (!GlobalSettings.Contains("GenderTagHandler"))
            {
                GlobalSettings.Add("GenderTagHandler", "-1");
            }
            if (!GlobalSettings.Contains("birthday"))
            {
                GlobalSettings.Add("birthday", "8/10/2015");
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
            LoadSplitters(Path.Combine(PathToConfigFiles, GlobalSettings.GetValue("splittersfile")));
        }

        private void LoadSplitters(string pathToSplitters)
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
        ///     Logs the specified message to the logger.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        public void Log(string message, LogLevel level)
        {
            Logger?.Log("ChatEngine", message, level);
        }

        [UsedImplicitly]
        public Result Chat(string rawInput, string UserGUID)
        {
            return Chat(new Request(rawInput, new User(UserGUID, this), this));
        }

        public Result Chat(Request request)
        {
            var result = new Result(request.User, this, request);
            if (IsAcceptingUserInput)
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
                    query.Template = RootNode.Evaluate(str, query, request, MatchState.UserInput, new StringBuilder());
                    result.SubQueries.Add(query);
                }
                foreach (var query in result.SubQueries)
                {
                    if (query.Template.Length > 0)
                    {
                        try
                        {
                            var str = ProcessNode(AimlTagHandler.GetNode(query.Template),
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
                            Log("A problem was encountered when trying to process the input: " +
                                request.RawInput + " with the template: \"" + query.Template + "\": " + ex.Message,
                                LogLevel.Error);
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

        private string ProcessNode([NotNull] XmlNode node, SubQuery query, Request request, Result result, User user)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
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
                        stringBuilder.Append(ProcessNode(node1, query, request, result, user));
                    }
                }
                return stringBuilder.ToString();
            }

            var handler = _tagFactory.Build(node, query, request, result, user, str);

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
                            node1.InnerXml = ProcessNode(node1, query, request, result, user);
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
                stringBuilder1.Append(ProcessNode(node1, query, request, result, user));
            }
            return stringBuilder1.ToString();
        }

        [UsedImplicitly]
        public void SaveToBinaryFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            var fileStream = File.Create(path);
            new BinaryFormatter().Serialize(fileStream, RootNode);
            fileStream.Close();
        }

        [UsedImplicitly]
        public void LoadFromBinaryFile(string path)
        {
            var fileStream = File.OpenRead(path);
            RootNode = (Node)new BinaryFormatter().Deserialize(fileStream);
            fileStream.Close();
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
                                  filename),
                    LogLevel.Warning);
                return;
            }

            // Add it to the graph
            try
            {
                RootNode.AddCategory(path, node.OuterXml, filename);

                Size++;
            }
            catch
            {
                Log(string.Format(Locale,
                                  "Failed to load a new category into the graphmaster where the directoryPath = {0} and template = {1} produced by a category in the file: {2}",
                                  path,
                                  node.OuterXml,
                                  filename),
                    LogLevel.Error);
            }
        }
    }
}