using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Assisticant.Fields;

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

        [CanBeNull]
        private readonly Func<bool> _isSelectedFunction;

        [NotNull]
        private readonly Observable<bool> _isSelected;

        /// <summary>
        ///     Initializes a new instance of the ActionButtonModel class.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <param name="action"> The action to execute. </param>
        public ActionButtonModel(string text, [NotNull] Action action) : this(text, action, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the ActionButtonModel class.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <param name="action"> The action to execute. </param>
        /// <param name="isSelectedFunction"> The is selected function that will evaluate whether or not this button appears selected. </param>
        public ActionButtonModel(string text, [NotNull] Action action, [CanBeNull] Func<bool> isSelectedFunction) : base(text)
        {
            Contract.Requires(action != null);

            _action = action;

            _isSelectedFunction = isSelectedFunction;

            _isSelected = new Observable<bool>();
        }

        /// <summary>
        ///     Process the command by interacting with the current processor frame.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="result"> The result. </param>
        internal override void ProcessCommand(MFDProcessor processor, MFDProcessorResult result)
        {
            // TODO: This should do some sort of exception handling / faultIndicator registration

            _action.Invoke();
        }


        /// <summary>
        ///     Gets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        ///     true if this instance is selected, false if not.
        /// </value>
        public override bool IsSelected
        {
            get { return _isSelected; }
        }

        /// <summary>
        ///     Processes the current state.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="result"> The result. </param>
        [SuppressMessage("Design", "CC0016:Copy Event To Variable Before Fire", Justification = "Read-only func declaration that cannot be null")]
        public override void ProcessCurrentState(MFDProcessor processor, MFDProcessorResult result)
        {
            var isSelectedFunction = _isSelectedFunction;

            if (isSelectedFunction != null)
            {
                // TODO: This should do some sort of exception handling / faultIndicator registration

                _isSelected.Value = isSelectedFunction();
            }
        }
    }
}