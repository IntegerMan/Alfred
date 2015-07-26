using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using MattEland.Ani.Alfred.Core.Annotations;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// A simple console used for unit testing and designer window purposes
    /// </summary>
    public sealed class SimpleConsole : IConsole, INotifyPropertyChanged
    {
        /// <summary>
        /// Logs the specified message to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="body">The body.</param>
        public void Log(string title, string body)
        {
            var evt = new ConsoleEvent(title, body);

            _events.Add(evt);
            OnPropertyChanged(nameof(Events));
        }

        private readonly List<ConsoleEvent> _events = new List<ConsoleEvent>();

        /// <summary>
        /// Gets the console events.
        /// </summary>
        /// <value>The console events.</value>
        public IList<ConsoleEvent> Events => _events;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}