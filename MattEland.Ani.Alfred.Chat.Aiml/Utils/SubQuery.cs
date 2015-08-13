// ---------------------------------------------------------
// SubQuery.cs
// 
// Created on:      08/12/2015 at 10:35 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    public class SubQuery
    {
        public string FullPath;
        public List<string> InputStar = new List<string>();
        public string Template = string.Empty;
        public List<string> ThatStar = new List<string>();
        public List<string> TopicStar = new List<string>();

        public SubQuery(string fullPath)
        {
            FullPath = fullPath;
        }
    }
}