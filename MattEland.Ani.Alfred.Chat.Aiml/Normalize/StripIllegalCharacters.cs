// ---------------------------------------------------------
// StripIllegalCharacters.cs
// 
// Created on:      08/12/2015 at 10:39 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{
    public class StripIllegalCharacters : TextTransformer
    {
        public StripIllegalCharacters(ChatEngine chatEngine, string inputString)
            : base(chatEngine, inputString)
        {
        }

        public StripIllegalCharacters(ChatEngine chatEngine)
            : base(chatEngine)
        {
        }

        protected override string ProcessChange()
        {
            return ChatEngine.Strippers.Replace(InputString, " ");
        }
    }
}