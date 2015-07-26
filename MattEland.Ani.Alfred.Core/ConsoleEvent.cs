using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MattEland.Ani.Alfred.Core
{
    public class ConsoleEvent
    {
        public ConsoleEvent(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public string Title { get; set; }

        public string Message { get; set; }

    }
}