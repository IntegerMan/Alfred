// ---------------------------------------------------------
// StripIllegalCharacters.cs
// 
// Created on:      08/12/2015 at 10:39 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{
    public class StripIllegalCharacters : TextTransformer
    {
        public StripIllegalCharacters(Bot bot, string inputString)
            : base(bot, inputString)
        {
        }

        public StripIllegalCharacters(Bot bot)
            : base(bot)
        {
        }

        protected override string ProcessChange()
        {
            return bot.Strippers.Replace(inputString, " ");
        }
    }
}