// --------------------------------------------------------- AlfredCommand.cs
// 
// Created on: 08/03/2015 at 1:40 PM Last Modified: 08/05/2015 at 3:06 PM Original author: Matt
// Eland ---------------------------------------------------------

using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// Represents a command that can be executed on the user interface layer 
    /// </summary>
    /// <remarks>
    /// It would be fantastic if this could support System.Windows.Input.ICommand, but portable
    /// doesn't reference that assembly so clients will have to provide their own version to support
    /// MVVM bindings, etc.
    /// </remarks>
    public class AlfredCommand : IAlfredCommand, IHasContainer<IAlfredContainer>
    {
        /// <summary>
        /// The <see cref="Action"/> invoked when the <see ref="Execute"/> method is called. 
        /// </summary>
        [CanBeNull]
        private Action _executeAction;

        /// <summary>
        /// Whether or not the command is enabled 
        /// </summary>
        private bool _isEnabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredCommand"/> class. 
        /// </summary>
        public AlfredCommand() : this(null, AlfredContainerHelper.ProvideContainer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredCommand"/> class. 
        /// </summary>
        /// <param name="container"> The container. </param>
        public AlfredCommand(IAlfredContainer container) : this(null, container)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredCommand"/> class. 
        /// </summary>
        /// <param name="executeAction"> The execute action. </param>
        /// <param name="container"> The container. </param>
        public AlfredCommand([CanBeNull] Action executeAction, [CanBeNull] IAlfredContainer container)
        {
            ExecuteAction = executeAction;
            Container = container;
        }

        /// <summary>
        /// Occurs when the result of <see ref="CanExecute"/> changes and should be re-evaluated. 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Gets the container. 
        /// </summary>
        /// <value> The container. </value>
        [CanBeNull]
        public IAlfredContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Action"/> that is invoked when a command's
        /// <see ref="Execute"/> method is called.
        /// </summary>
        /// <value> The executed <see cref="Action"/>. </value>
        [CanBeNull]
        public Action ExecuteAction
        {
            get { return _executeAction; }
            set { _executeAction = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command is enabled. 
        /// </summary>
        /// <value> <c> true </c> if this command is enabled; otherwise, <c> false </c>. </value>
        [UsedImplicitly]
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
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
        /// Defines the method that determines whether the command can execute in its current state. 
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this
        /// <see langword="object"/> can be set to null.
        /// </param>
        /// <returns> <see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>. </returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parameter",
            Justification = "This matches the XAML ICommand interface which makes for ease of porting")]
        public bool CanExecute([CanBeNull] object parameter)
        {
            return IsEnabled;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked. 
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this
        /// <see langword="object"/> can be set to null.
        /// </param>
        /// <exception cref="Exception">
        /// A <see langword="delegate"/> callback throws an exception.
        /// </exception>
        public virtual void Execute([CanBeNull] object parameter)
        {
            // TODO: Support async invokes here via a parameter. 

            ExecuteAction?.Invoke();
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event. 
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Matching WPF schema")]
        [SuppressMessage("ReSharper", "CatchAllClause")]
        public void RaiseCanExecuteChanged()
        {
            try
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                // Build Exception Details 
                var message = ex.BuildDetailsMessage();
                message = $"Problem raising CanExecuteChanged: {message}";

                // Log it 
                message.Log("Command.CanExecuteChanged", LogLevel.Error, Container);
            }
        }
    }
}