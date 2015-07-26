using System.Collections.Generic;
using MattEland.Ani.Alfred.Core;
using NUnit.Framework;

namespace MattELand.Ani.Alfred.Core.Tests 
{
    public class TestConsole : IConsole
    {
        private List<ConsoleEvent> _events = new List<ConsoleEvent>();

        public void Log(string title, string body)
        {
            var e = new ConsoleEvent(title, body);

            _events.Add(e);
        }
    }
}