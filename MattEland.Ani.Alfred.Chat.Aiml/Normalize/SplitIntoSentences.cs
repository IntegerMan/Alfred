// ---------------------------------------------------------
// SplitIntoSentences.cs
// 
// Created on:      08/12/2015 at 10:38 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{
    public class SplitIntoSentences
    {
        private readonly Bot bot;
        private string inputString;

        public SplitIntoSentences(Bot bot, string inputString)
        {
            this.bot = bot;
            this.inputString = inputString;
        }

        public SplitIntoSentences(Bot bot)
        {
            this.bot = bot;
        }

        public string[] Transform(string inputString)
        {
            this.inputString = inputString;
            return Transform();
        }

        public string[] Transform()
        {
            var strArray = inputString.Split(bot.Splitters.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var list = new List<string>();
            foreach (var str1 in strArray)
            {
                var str2 = str1.Trim();
                if (str2.Length > 0)
                {
                    list.Add(str2);
                }
            }
            return list.ToArray();
        }
    }
}