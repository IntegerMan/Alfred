// ---------------------------------------------------------
// SplitIntoSentences.cs
// 
// Created on:      08/12/2015 at 10:38 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{
    public class SplitIntoSentences
    {
        private readonly ChatEngine chatEngine;
        private string inputString;

        public SplitIntoSentences(ChatEngine chatEngine, string inputString)
        {
            this.chatEngine = chatEngine;
            this.inputString = inputString;
        }

        public SplitIntoSentences(ChatEngine chatEngine)
        {
            this.chatEngine = chatEngine;
        }

        public string[] Transform(string inputString)
        {
            this.inputString = inputString;
            return Transform();
        }

        public string[] Transform()
        {
            var strArray = inputString.Split(chatEngine.Splitters.ToArray(), StringSplitOptions.RemoveEmptyEntries);
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