// ---------------------------------------------------------
// TextTransformer.cs
// 
// Created on:      08/12/2015 at 10:36 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    public abstract class TextTransformer
    {
        public Bot bot;
        protected string inputString;

        public TextTransformer(Bot bot, string inputString)
        {
            this.bot = bot;
            this.inputString = inputString;
        }

        public TextTransformer(Bot bot)
        {
            this.bot = bot;
            inputString = string.Empty;
        }

        public TextTransformer()
        {
            bot = null;
            inputString = string.Empty;
        }

        public string InputString
        {
            get { return inputString; }
            set { inputString = value; }
        }

        public string OutputString
        {
            get { return Transform(); }
        }

        public string Transform(string input)
        {
            inputString = input;
            return Transform();
        }

        public string Transform()
        {
            if (inputString.Length > 0)
            {
                return ProcessChange();
            }
            return string.Empty;
        }

        protected abstract string ProcessChange();
    }
}