using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core
{

    /// <summary>
    /// Represents the result of sending a command to a subsystem for handling
    /// </summary>
    public sealed class AlfredCommandResult : ICommandResult
    {

        /// <summary>
        /// Gets or sets the value to set as the last input value.
        /// This is what is displayed as the user's last sentence to the system.
        /// 
        /// Changing  this is a good way of hiding redirection searches.
        /// </summary>
        /// <value>The new last input value.</value>
        public string NewLastInput { get; set; }

        /// <summary>
        /// Gets or sets the new last output value. This is what is
        /// displayed as Alfred's response to the user's statement.
        /// 
        /// Changing this is a good way of handling dynamic information in queries
        /// </summary>
        /// <value>The new last output.</value>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the chat redirection value.
        /// If this is set and the command is handled, the system
        /// will send a new chat message into the system with this
        /// value. 
        /// 
        /// This is a good way to redirect to a common prompt
        /// after executing a command.
        /// </summary>
        /// <value>The execute chat input.</value>
        public string RedirectToChat { get; set; }
    }
}