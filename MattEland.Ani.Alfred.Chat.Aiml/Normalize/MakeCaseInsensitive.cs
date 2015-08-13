// ---------------------------------------------------------
// MakeCaseInsensitive.cs
// 
// Created on:      08/12/2015 at 10:38 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{
    public class MakeCaseInsensitive : TextTransformer
    {
        public MakeCaseInsensitive(Bot bot, string inputString)
            : base(bot, inputString)
        {
        }

        public MakeCaseInsensitive(Bot bot)
            : base(bot)
        {
        }

        protected override string ProcessChange()
        {
            return inputString.ToUpper();
        }

        public static string TransformInput(string input)
        {
            return input.ToUpper();
        }
    }
}