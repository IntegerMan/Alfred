// ---------------------------------------------------------
// User.cs
// 
// Created on:      08/12/2015 at 10:24 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    public class User
    {
        private readonly string id;
        private readonly List<Result> Results = new List<Result>();
        public Bot bot;
        public SettingsDictionary Predicates;

        public User(string UserID, Bot bot)
        {
            if (UserID.Length <= 0)
            {
                throw new Exception("The UserID cannot be empty");
            }
            id = UserID;
            this.bot = bot;
            Predicates = new SettingsDictionary(this.bot);
            this.bot.DefaultPredicates.Clone(Predicates);
            Predicates.addSetting("topic", "*");
        }

        public string UserID
        {
            get { return id; }
        }

        public string Topic
        {
            get { return Predicates.grabSetting("topic"); }
        }

        public Result LastResult
        {
            get
            {
                if (Results.Count > 0)
                {
                    return Results[0];
                }
                return null;
            }
        }

        public string getLastBotOutput()
        {
            if (Results.Count > 0)
            {
                return Results[0].RawOutput;
            }
            return "*";
        }

        public string getThat()
        {
            return getThat(0, 0);
        }

        public string getThat(int n)
        {
            return getThat(n, 0);
        }

        public string getThat(int n, int sentence)
        {
            if (n >= 0 & n < Results.Count)
            {
                var result = Results[n];
                if (sentence >= 0 & sentence < result.OutputSentences.Count)
                {
                    return result.OutputSentences[sentence];
                }
            }
            return string.Empty;
        }

        public string getResultSentence()
        {
            return getResultSentence(0, 0);
        }

        public string getResultSentence(int n)
        {
            return getResultSentence(n, 0);
        }

        public string getResultSentence(int n, int sentence)
        {
            if (n >= 0 & n < Results.Count)
            {
                var result = Results[n];
                if (sentence >= 0 & sentence < result.InputSentences.Count)
                {
                    return result.InputSentences[sentence];
                }
            }
            return string.Empty;
        }

        public void addResult(Result latestResult)
        {
            Results.Insert(0, latestResult);
        }
    }
}