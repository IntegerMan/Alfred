// ---------------------------------------------------------
// ApplySubstitutions.cs
// 
// Created on:      08/12/2015 at 10:37 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{
    internal sealed class ApplySubstitutions : TextTransformer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        internal ApplySubstitutions(ChatEngine chatEngine)
            : base(chatEngine)
        {
        }

        private static string getMarker(int len)
        {
            var chArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var stringBuilder = new StringBuilder();
            var random = new Random();
            for (var index = 0; index < len; ++index)
            {
                stringBuilder.Append(chArray[random.Next(chArray.Length)]);
            }
            return stringBuilder.ToString();
        }

        protected override string ProcessChange()
        {
            return Substitute(ChatEngine.Substitutions, InputString);
        }

        internal static string Substitute([NotNull] SettingsDictionary dictionary,
                                          [NotNull] string target)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var marker = getMarker(5);
            var input = target;

            var settingNames = dictionary.SettingNames;
            if (settingNames != null)
            {
                foreach (var str in settingNames)
                {
                    var pattern = "\\b" + makeRegexSafe(str).TrimEnd().TrimStart() + "\\b";
                    var replacement = marker + dictionary.grabSetting(str).Trim() + marker;
                    input = Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
                }
            }

            return input.Replace(marker, "");
        }

        private static string makeRegexSafe(string input)
        {
            return input.Replace("\\", "").Replace(")", "\\)").Replace("(", "\\(").Replace(".", "\\.");
        }
    }
}