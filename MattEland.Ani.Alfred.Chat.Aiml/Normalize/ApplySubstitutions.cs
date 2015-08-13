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

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{
    public class ApplySubstitutions : TextTransformer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ApplySubstitutions" /> class.
        /// </summary>
        /// <param name="bot">The bot.</param>
        /// <param name="inputString">The input string.</param>
        public ApplySubstitutions(Bot bot, string inputString)
            : base(bot, inputString)
        {
        }

        public ApplySubstitutions(Bot bot)
            : base(bot)
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
            return Substitute(Bot, Bot.Substitutions, InputString);
        }

        public static string Substitute(Bot bot, SettingsDictionary dictionary, string target)
        {
            var marker = getMarker(5);
            var input = target;
            foreach (var str in dictionary.SettingNames)
            {
                var pattern = "\\b" + makeRegexSafe(str).TrimEnd().TrimStart() + "\\b";
                var replacement = marker + dictionary.grabSetting(str).Trim() + marker;
                input = Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
            }
            return input.Replace(marker, "");
        }

        private static string makeRegexSafe(string input)
        {
            return input.Replace("\\", "").Replace(")", "\\)").Replace("(", "\\(").Replace(".", "\\.");
        }
    }
}