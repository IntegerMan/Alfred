// ---------------------------------------------------------
// Request.cs
// 
// Created on:      08/12/2015 at 10:22 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    public class Request
    {
        public Bot bot;
        public bool hasTimedOut;
        public string rawInput;
        public Result result;
        public DateTime StartedOn;
        public User user;

        public Request(string rawInput, User user, Bot bot)
        {
            this.rawInput = rawInput;
            this.user = user;
            this.bot = bot;
            StartedOn = DateTime.Now;
        }
    }
}