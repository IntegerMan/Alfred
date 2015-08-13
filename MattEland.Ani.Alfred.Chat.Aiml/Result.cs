﻿// ---------------------------------------------------------
// Result.cs
// 
// Created on:      08/12/2015 at 10:23 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    public class Result
    {
        public Bot bot;
        public TimeSpan Duration;
        public List<string> InputSentences = new List<string>();
        public List<string> NormalizedPaths = new List<string>();
        public List<string> OutputSentences = new List<string>();
        public Request request;
        public List<SubQuery> SubQueries = new List<SubQuery>();
        public User user;

        public Result(User user, Bot bot, Request request)
        {
            this.user = user;
            this.bot = bot;
            this.request = request;
            this.request.result = this;
        }

        public string RawInput
        {
            get { return request.rawInput; }
        }

        public string Output
        {
            get
            {
                if (OutputSentences.Count > 0)
                {
                    return RawOutput;
                }
                if (request.hasTimedOut)
                {
                    return bot.TimeOutMessage;
                }
                var stringBuilder = new StringBuilder();
                foreach (var str in NormalizedPaths)
                {
                    stringBuilder.Append(str + Environment.NewLine);
                }
                bot.writeToLog("The bot could not find any response for the input: " + RawInput + " with the path(s): " +
                               Environment.NewLine + stringBuilder + " from the user with an id: " + user.UserID);
                return string.Empty;
            }
        }

        public string RawOutput
        {
            get
            {
                var stringBuilder = new StringBuilder();
                foreach (var str in OutputSentences)
                {
                    var sentence = str.Trim();
                    if (!checkEndsAsSentence(sentence))
                    {
                        sentence += ".";
                    }
                    stringBuilder.Append(sentence + " ");
                }
                return stringBuilder.ToString().Trim();
            }
        }

        public override string ToString()
        {
            return Output;
        }

        private bool checkEndsAsSentence(string sentence)
        {
            foreach (var str in bot.Splitters)
            {
                if (sentence.Trim().EndsWith(str))
                {
                    return true;
                }
            }
            return false;
        }
    }
}