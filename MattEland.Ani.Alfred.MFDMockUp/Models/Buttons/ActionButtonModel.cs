using System;
using System.Diagnostics.Contracts;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    /// <summary>
    ///     A data model for a type of button that queues an <see cref="Action"/> to be executed by
    ///     the processor the next time an update occurs. This class cannot be inherited.
    /// </summary>
    public sealed class ActionButtonModel : ButtonModel
    {
        /// <summary>
        ///     The action to invoke.
        /// </summary>
        [NotNull]
        private readonly Action _action;

        /// <summary>
        ///     Initializes a new instance of the ActionButtonModel class.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <param name="action"> The action to execute. </param>
        public ActionButtonModel(string text, [NotNull] Action action) : base(text)
        {
            Contract.Requires(action != null);

            _action = action;
        }

        /// <summary>
        ///     Process the command by interacting with the current processor frame.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="result"> The result. </param>
        internal override void ProcessCommand(MFDProcessor processor, MFDProcessorResult result)
        {
            // TODO: This should do some sort of exception handling / fault registration

            _action.Invoke();
        }
    }
}