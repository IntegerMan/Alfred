// ---------------------------------------------------------
// ChatEngineLibrarian.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/07/2015 at 9:58 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    /// <summary>
    ///     The librarian is a helper class that manages settings for the Chat Engine
    /// </summary>
    public sealed class ChatEngineLibrarian
    {
        /// <summary>
        ///     The chat engine.
        /// </summary>
        [NotNull]
        private readonly ChatEngine _chatEngine;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatEngineLibrarian" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="chatEngine" /> is <see langword="null" /> .
        /// </exception>
        internal ChatEngineLibrarian([NotNull] ChatEngine chatEngine)
        {
            //-Validate
            if (chatEngine == null) { throw new ArgumentNullException(nameof(chatEngine)); }

            _chatEngine = chatEngine;

            // Initialize settings dictionaries
            GlobalSettings = new SettingsManager();
            GenderSubstitutions = new SettingsManager();
            SecondPersonToFirstPersonSubstitutions = new SettingsManager();
            FirstPersonToSecondPersonSubstitutions = new SettingsManager();
            Substitutions = new SettingsManager();
        }

        /// <summary>
        ///     Gets the substitutions dictionary. <see cref="Substitutions"/> are common phrases
        ///     that are condensed and translated to their equivalent meaning to increase bot
        ///     flexibility and make content authoring easier.
        /// </summary>
        /// <value>
        /// The substitutions.
        /// </value>
        [NotNull]
        public SettingsManager Substitutions { get; }

        /// <summary>
        ///     Gets the gender substitutions dictionary. This is a collection of male and female
        ///     pronouns and their replacement values to use when the "gender" AIML tag is present.
        /// </summary>
        /// <value>
        /// The gender substitutions dictionary.
        /// </value>
        [NotNull]
        public SettingsManager GenderSubstitutions { get; }

        /// <summary>
        ///     Gets the global settings dictionary.
        /// </summary>
        /// <value>
        /// The global settings.
        /// </value>
        [NotNull]
        public SettingsManager GlobalSettings { get; }

        /// <summary>
        ///     Gets the person substitutions settings dictionary for second person to first person
        ///     conversions.
        /// </summary>
        /// <value>
        /// The person substitutions settings dictionary.
        /// </value>
        [NotNull]
        public SettingsManager SecondPersonToFirstPersonSubstitutions { get; }

        /// <summary>
        ///     Gets the person substitutions settings dictionary. This contains things related to
        ///     moving from the first person to the second person.
        /// </summary>
        /// <value>
        /// The person substitutions settings dictionary.
        /// </value>
        [NotNull]
        public SettingsManager FirstPersonToSecondPersonSubstitutions { get; }

        /// <summary>
        ///     Loads settings into the various dictionaries.
        /// </summary>
        /// <param name="pathToConfigFiles">The path to the configuration files directory.</param>
        /// <exception cref="ArgumentException">Path to settings must be provided.</exception>
        /// <exception cref="XmlException">Path to settings must be provided.</exception>
        /// <exception cref="FileNotFoundException">The settings file was not found.</exception>
        /// <exception cref="SecurityException">
        /// Could not find a settings file at the given path
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// Access to <paramref name="pathToConfigFiles" /> is denied.
        /// </exception>
        /// <exception cref="IOException">
        /// The specified path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        internal void LoadSettingsFromConfigDirectory([NotNull] string pathToConfigFiles)
        {
            // Validate
            if (pathToConfigFiles.IsEmpty())
            {
                throw new ArgumentException(Resources.LibrarianLoadSettingsErrorNoConfigDirectory,
                                            nameof(pathToConfigFiles));
            }

            // Load global settings and ensure default values are present
            var pathToSettings = Path.Combine(pathToConfigFiles, "Settings.xml");
            GlobalSettings.Load(pathToSettings);
            AddDefaultSettings();

            // Load the individual dictionaries. If any one of these fail, the failure will be logged but things will move on
            var person2Path = Path.Combine(pathToConfigFiles,
                                           GlobalSettings.GetValue("person2substitutionsfile"));

            SecondPersonToFirstPersonSubstitutions.LoadSafe(person2Path,
                                                            _chatEngine.Logger,
                                                            _chatEngine.Locale);

            var person1Path = Path.Combine(pathToConfigFiles,
                                           GlobalSettings.GetValue(@"personsubstitutionsfile"));
            FirstPersonToSecondPersonSubstitutions.LoadSafe(person1Path,
                                                            _chatEngine.Logger,
                                                            _chatEngine.Locale);

            var genderPath = Path.Combine(pathToConfigFiles,
                                          GlobalSettings.GetValue(@"gendersubstitutionsfile"));
            GenderSubstitutions.LoadSafe(genderPath, _chatEngine.Logger, _chatEngine.Locale);

            var substitutionPath = Path.Combine(pathToConfigFiles,
                                                GlobalSettings.GetValue(@"substitutionsfile"));
            Substitutions.LoadSafe(substitutionPath, _chatEngine.Logger, _chatEngine.Locale);
        }

        /// <summary>
        ///     Loads various settings from XML files. Input parameters are all optional and should
        ///     be the actual XML and not a file path.
        /// </summary>
        /// <param name="globalXml">The global XML.</param>
        /// <param name="firstPersonXml">The first person XML.</param>
        /// <param name="secondPersonXml">The second person XML.</param>
        /// <param name="genderXml">The gender XML.</param>
        /// <param name="substitutionsXml">The substitutions XML.</param>
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        internal void LoadSettingsFromXml(
            [CanBeNull] string globalXml = null,
            [CanBeNull] string firstPersonXml = null,
            [CanBeNull] string secondPersonXml = null,
            [CanBeNull] string genderXml = null,
            [CanBeNull] string substitutionsXml = null)
        {
            if (globalXml.HasText())
            {
                GlobalSettings.LoadXmlSafe(globalXml, _chatEngine.Logger, _chatEngine.Locale);
            }

            AddDefaultSettings();

            if (firstPersonXml.HasText())
            {
                FirstPersonToSecondPersonSubstitutions.LoadXmlSafe(firstPersonXml,
                                                                   _chatEngine.Logger,
                                                                   _chatEngine.Locale);
            }
            if (secondPersonXml.HasText())
            {
                SecondPersonToFirstPersonSubstitutions.LoadXmlSafe(secondPersonXml,
                                                                   _chatEngine.Logger,
                                                                   _chatEngine.Locale);
            }
            if (genderXml.HasText())
            {
                GenderSubstitutions.LoadXmlSafe(genderXml, _chatEngine.Logger, _chatEngine.Locale);
            }
            if (substitutionsXml.HasText())
            {
                Substitutions.LoadXmlSafe(substitutionsXml, _chatEngine.Logger, _chatEngine.Locale);
            }
        }

        /// <summary>
        ///     Adds default settings to the global settings file.
        /// </summary>
        private void AddDefaultSettings()
        {
            AddSettingIfMissing(@"person2substitutionsfile", @"Person2Substitutions.xml");
            AddSettingIfMissing(@"personsubstitutionsfile", @"PersonSubstitutions.xml");
            AddSettingIfMissing(@"gendersubstitutionsfile", @"GenderSubstitutions.xml");
            AddSettingIfMissing(@"substitutionsfile", @"Substitutions.xml");
            AddSettingIfMissing(@"aimldirectory", @"aiml");
            AddSettingIfMissing(@"configdirectory", @"config");
        }

        /// <summary>
        ///     Adds a setting to the global settings dictionary if that setting is not present.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settingName"/></exception>
        private void AddSettingIfMissing(
            [NotNull] string settingName,
            [CanBeNull] string defaultValue)
        {
            if (settingName == null) { throw new ArgumentNullException(nameof(settingName)); }

            if (!GlobalSettings.Contains(settingName))
            {
                GlobalSettings.Add(settingName, defaultValue);
            }
        }
    }
}