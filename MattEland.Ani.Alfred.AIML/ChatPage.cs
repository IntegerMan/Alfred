using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    /// A custom page for interacting with Alfred via Chat
    /// </summary>
    public class ChatPage : AlfredPage
    {
        [NotNull]
        private string _input;

        [NotNull]
        private readonly IUserStatementHandler _inputHandler;

        [NotNull]
        private UserStatementResponse _response;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredPage" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="inputHandler">The input handler</param>
        public ChatPage([NotNull] string name, [NotNull] IUserStatementHandler inputHandler) : base(name)
        {
            _inputHandler = inputHandler;

            _input = string.Empty;

            _response = new UserStatementResponse(string.Empty, Resources.InitialGreeting.NonNull(), true);
        }

        /// <summary>
        /// Gets the children of this component. Depending on the type of component this is, the children will
        /// vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { yield break; }
        }

        /// <summary>
        /// Gets the last response from Alfred.
        /// </summary>
        /// <value>The last response.</value>
        [NotNull]
        [UsedImplicitly]
        public UserStatementResponse LastResponse
        {
            get { return _response; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                if (Equals(value, _response))
                    return;

                _response = value;
                OnPropertyChanged(nameof(LastResponse));
            }
        }

        /// <summary>
        /// Gets or sets the user's input.
        /// </summary>
        /// <value>The user input.</value>
        [NotNull]
        public string UserInput
        {
            get { return _input; }
            set
            {
                if (value != _input)
                {
                    _input = value.NonNull();

                    OnPropertyChanged(nameof(UserInput));
                }
            }
        }

        /// <summary>
        /// Handles user statement and grabs a resonse from Alfred.
        /// </summary>
        /// <param name="input">The user input.</param>
        /// <returns>The response to the statement</returns>
        public UserStatementResponse HandleUserStatement([CanBeNull] string input)
        {
            UserInput = input.NonNull();

            var response = _inputHandler.HandleUserStatement(UserInput);

            LastResponse = response;
            return response;
        }
    }

}