// ---------------------------------------------------------
// AlfredSpeechProvider.cs
// 
// Created on:      08/07/2015 at 2:00 PM
// Last Modified:   08/07/2015 at 3:41 PM
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
    public sealed class AlfredSpeechProvider : IDisposable
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
                console.Log(LogHeader, Resources.InitializingSpeechModule.NonNull(), LogLevel.Verbose);

                // Enumerate all detected voices for diagnostic purposes
                LogInstalledVoices(console);
            }

            // We want Alfred to sound lik an English butler so request the closest thing we can find
            var greatBritainCulture = new CultureInfo("en-GB");
            _speech.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Senior, 0, greatBritainCulture);

            // Set to slightly faster than normal
            _speech.Rate = 2;

            // Everything else is just logging, so... get out of here
            if (console == null)
            {
                return;
            }

            // Log what voice we're using
            var voice = _speech.Voice;
            if (voice != null)
            {
                var message = string.Format(CultureInfo.CurrentCulture, Resources.UsingVoiceLog.NonNull(), voice.Name);
                console.Log(LogHeader,
                            message.NonNull(),
                            LogLevel.Verbose);
            }
            else
            {
                console.Log(LogHeader,
                            Resources.UsingUnknownVoice.NonNull(),
                            LogLevel.Warning);
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
            console.Log(LogHeader, Resources.FindVoiceModules.NonNull(), Level);

            var voices = _speech.GetInstalledVoices();

            foreach (var voice in voices)
            {
                LogDetectedVoice(console, voice);
            }
        }

        /// <summary>
        /// Logs the detected voice's information to the console.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="voice">The voice.</param>
        private static void LogDetectedVoice([NotNull] IConsole console, [CanBeNull] InstalledVoice voice)
        {
            // We have a voice; log what we've found
            if (voice != null)
            {
                var info = voice.VoiceInfo;
                if (info != null)
                {
                    var message = string.Format(CultureInfo.CurrentCulture,
                                                Resources.VoiceFoundList.NonNull(),
                                                info.Name,
                                                info.Gender,
                                                info.Culture?.EnglishName,
                                                voice.Enabled);

                    console.Log(LogHeader, message, LogLevel.Verbose);
                }
                else
                {
                    console.Log(LogHeader, Resources.UnknownVoiceFound.NonNull(), LogLevel.Warning);
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

        /// <summary>
        /// Disposes of all allocated resources
        /// </summary>
        public void Dispose()
        {
            _speech.Dispose();
        }
    }
}