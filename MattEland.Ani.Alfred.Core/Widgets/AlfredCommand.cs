// ---------------------------------------------------------
// AlfredCommand.cs
// 
// Created on:      08/03/2015 at 1:40 PM
// Last Modified:   08/03/2015 at 1:46 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
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
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        ///     true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param
        ///     name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public virtual bool CanExecute([CanBeNull] object parameter)
        {
            return true;
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param
        ///     name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public virtual void Execute([CanBeNull] object parameter)
        {
        }

        /// <summary>
        /// Occurs when the result of CanExecute changes and should be re-evaluated.
        /// </summary>
        public event EventHandler CanExecuteChanged;
    }
}