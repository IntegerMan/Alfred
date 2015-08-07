// ---------------------------------------------------------
// AlfredSpeechProvider.cs
// 
// Created on:      08/07/2015 at 2:00 PM
// Last Modified:   08/07/2015 at 3:15 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;
using System.Speech.Synthesis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core.Speech
{
    /// <summary>
    ///     Provides speech synthesis and speech recognition capabilities to the Alfred framework.
    ///     This class cannot be inherited.
    /// </summary>
    public sealed class AlfredSpeechProvider
    {
        /// <summary>
        ///     The log header used in speech related log entries
        /// </summary>
        public const string LogHeader = "Alfred.Speech";

        [NotNull]
        private readonly SpeechSynthesizer _speech;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSpeechProvider" /> class.
        /// </summary>
        /// <param name="console">The console.</param>
        public AlfredSpeechProvider([CanBeNull] IConsole console)
        {
            _speech = new SpeechSynthesizer();

            // Let's get verbose with the console
            if (console != null)
            {
                console.Log(LogHeader, "Initializing speech module...", LogLevel.Verbose);

                // Enumerate all detected voices for diagnostic purposes
                LogInstalledVoices(console);
            }

            // We want Alfred to sound lik an English butler so request the closest thing we can find
            var greatBritainCulture = new CultureInfo("en-GB");
            _speech.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Senior, 0, greatBritainCulture);

            if (console != null)
            {
                var voice = _speech.Voice;
                if (voice != null)
                {
                    console.Log(LogHeader, "Using voice: " + voice.Name, LogLevel.Verbose);
                }
                else
                {
                    console.Log(LogHeader,
                                "Using unknown voice. Speech synthesis may not function properly.",
                                LogLevel.Warning);
                }
            }
        }

        /// <summary>
        ///     Logs detected voices to the console
        /// </summary>
        /// <param name="console">The console.</param>
        private void LogInstalledVoices([NotNull] IConsole console)
        {
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            const LogLevel Level = LogLevel.Verbose;
            console.Log(LogHeader, "Searching for installed voice modules", Level);

            var voices = _speech.GetInstalledVoices();

            foreach (var voice in voices)
            {

                // Shouldn't happen, but just in case
                if (voice == null)
                {
                    continue;
                }

                // We have a voice; log what we've found
                var info = voice.VoiceInfo;
                if (info != null)
                {
                    var message = string.Format(CultureInfo.CurrentCulture,
                                                "Found voice: {0} ({1} {2}) Enabled: {3}",
                                                info.Name,
                                                info.Gender,
                                                info.Culture?.DisplayName,
                                                voice.Enabled);

                    console.Log(LogHeader, message, Level);
                }
                else
                {
                    console.Log(LogHeader, "Found unknown voice with no voice information", LogLevel.Warning);
                }
            }
        }

        /// <summary>
        ///     Says the specified phrase.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <exception cref="System.ArgumentNullException">phrase</exception>
        public void Say([NotNull] string phrase)
        {
            if (phrase == null)
            {
                throw new ArgumentNullException(nameof(phrase));
            }

            // Actually speak things
            _speech.SpeakAsync(phrase);
        }
    }
}