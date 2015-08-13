// ---------------------------------------------------------
// Request.cs
// 
// Created on:      08/12/2015 at 10:22 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

namespace MattEland.Ani.Alfred.Chat.Aiml
{
    public class Request
    {
        public ChatEngine chatEngine;
        public bool hasTimedOut;
        public string rawInput;
        public Result result;
        public DateTime StartedOn;
        public User user;

        public Request(string rawInput, User user, ChatEngine chatEngine)
        {
            this.rawInput = rawInput;
            this.user = user;
            this.chatEngine = chatEngine;
            StartedOn = DateTime.Now;
        }
    }
}