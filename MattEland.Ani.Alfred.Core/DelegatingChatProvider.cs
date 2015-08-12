// ---------------------------------------------------------
// DelegatingChatProvider.cs
// 
// Created on:      08/11/2015 at 10:41 PM
// Last Modified:   08/11/2015 at 11:08 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     A chat provider that intercepts responses before they get to the user and optionally executes/routes commands on
    ///     behalf of Alfred. This lets us decorate an existing IChatProvider and route responses both to submodules and to the
    ///     user.
    /// </summary>
    /// <remarks>
    ///     This supports INotifyPropertyChanged to enable data binding
    /// </remarks>
    internal sealed class DelegatingChatProvider : IChatProvider, INotifyPropertyChanged
    {
        [NotNull]
        private readonly IAlfred _alfred;

        [NotNull]
        private readonly IChatProvider _chatProvider;

        [CanBeNull]
        private string _lastInput;

        private UserStatementResponse _lastResponse;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DelegatingChatProvider" /> class.
        /// </summary>
        /// <param name="chatProvider">The chat provider.</param>
        /// <param name="alfred">The alfred instance.</param>
        public DelegatingChatProvider([NotNull] IChatProvider chatProvider, [NotNull] IAlfred alfred)
        {
            //- Validate for Sanity
            if (chatProvider == null)
            {
                throw new ArgumentNullException(nameof(chatProvider));
            }
            if (alfred == null)
            {
                throw new ArgumentNullException(nameof(alfred));
            }

            //- Store our references
            _chatProvider = chatProvider;
            _alfred = alfred;

            // Take delegated values as the last values from the chat provider
            _lastInput = chatProvider.LastInput;
            _lastResponse = chatProvider.LastResponse;
        }

        /// <summary>
        ///     Gets the chat provider.
        /// </summary>
        /// <value>The chat provider.</value>
        [NotNull]
        public IChatProvider ChatProvider
        {
            [DebuggerStepThrough]
            get
            { return _chatProvider; }
        }

        /// <summary>
        ///     Gets the alfred instance.
        /// </summary>
        /// <value>The alfred instance.</value>
        [NotNull]
        public IAlfred Alfred
        {
            [DebuggerStepThrough]
            get
            { return _alfred; }
        }

        /// <summary>
        ///     Handles a user statement.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The response to the user statement</returns>
        public UserStatementResponse HandleUserStatement(string userInput)
        {

            var response = ChatProvider.HandleUserStatement(userInput);

            // TODO: Route this to Alfred's Subsystems to handle as a command

            // Update our values so the consumer can check or bind to this instance.
            LastInput = userInput;
            LastResponse = response;

            return response;
        }

        /// <summary>
        ///     Gets the last response from the system.
        /// </summary>
        /// <value>The last response.</value>
        public UserStatementResponse LastResponse
        {
            get { return _lastResponse; }
            set
            {
                if (value.Equals(_lastResponse))
                {
                    return;
                }
                _lastResponse = value;
                OnPropertyChanged(nameof(LastResponse));
            }
        }

        /// <summary>
        ///     Gets the last input from the user.
        /// </summary>
        /// <value>The last input.</value>
        public string LastInput
        {
            get { return _lastInput; }
            private set
            {
                if (value == _lastInput)
                {
                    return;
                }
                _lastInput = value;
                OnPropertyChanged(nameof(LastInput));
            }
        }

        /// <summary>
        /// Does an initial greeting to the user without requiring any input. This should set LastInput to null.
        /// </summary>
        public void DoInitialGreeting()
        {
            // Send an event to the chat system as if the user typed it. The data files will
            // handle it from there as far as generating proper content.
            const string StartupTopic = "EVT_STARTUP";
            HandleUserStatement(StartupTopic);

            // Clear out any evidence of simulating the user talking to the chat system
            LastInput = null;
        }

        /// <summary>
        ///     Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Called when a property changes and raises the property changed event
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}