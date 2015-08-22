// ---------------------------------------------------------
// AlfredCommand.cs
// 
// Created on:      08/03/2015 at 1:40 PM
// Last Modified:   08/05/2015 at 3:06 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Represents a command that can be executed on the user interface layer
    /// </summary>
    /// <remarks>
    ///     It would be fantastic if this could support System.Windows.Input.ICommand,
    ///     but portable doesn't reference that assembly so clients will have to provide
    ///     their own version to support MVVM bindings, etc.
    /// </remarks>
    public class AlfredCommand
    {
        /// <summary>
        ///     The Action invoked when the Execute method is called.
        /// </summary>
        [CanBeNull]
        private Action _executeAction;

        /// <summary>
        ///     Whether or not the command is enabled
        /// </summary>
        private bool _isEnabled = true;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="AlfredCommand" />
        ///     class.
        /// </summary>
        public AlfredCommand() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="AlfredCommand" />
        ///     class.
        /// </summary>
        /// <param name="executeAction">
        ///     The execute action.
        /// </param>
        public AlfredCommand([CanBeNull] Action executeAction)
        {
            ExecuteAction = executeAction;
        }

        /// <summary>
        ///     Gets or sets the Action that is invoked when a command executes.
        /// </summary>
        /// <value>The executed Action.</value>
        [CanBeNull]
        public Action ExecuteAction
        {
            get { return _executeAction; }
            set { _executeAction = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this command is enabled.
        /// </summary>
        /// <value><c>true</c> if this command is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        ///     true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parameter",
            Justification = "This matches the XAML ICommand interface which makes for ease of porting")]
        public bool CanExecute([CanBeNull] object parameter)
        {
            return IsEnabled;
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public virtual void Execute([CanBeNull] object parameter)
        {
            // TODO: I could support async invokes here. I'd want to add a parameter for that, but it's possible.

            ExecuteAction?.Invoke();
        }

        /// <summary>
        ///     Occurs when the result of CanExecute changes and should be re-evaluated.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Raises the CanExecuteChanged event.
        /// </summary>
        private void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}