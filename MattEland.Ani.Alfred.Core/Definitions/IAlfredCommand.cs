// ---------------------------------------------------------
// IAlfredCommand.cs
// 
// Created on:      09/02/2015 at 10:03 PM
// Last Modified:   09/02/2015 at 10:03 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Represents a command that can be executed on the user interface layer
    /// </summary>
    public interface IAlfredCommand
    {
        /// <summary>
        ///     Gets or sets the <see cref="Action" /> that is invoked when a command's <see ref="Execute"/> method is called.
        /// </summary>
        /// <value>
        /// The executed <see cref="Action"/>.
        /// </value>
        Action ExecuteAction { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the command is enabled.
        /// </summary>
        /// <value><c>true</c> if this command is enabled; otherwise, <c>false</c>.</value>
        bool IsEnabled { get; set; }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this
        ///     <see langword="object"/>
        ///     can be set to null.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if this command can be executed; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        bool CanExecute([CanBeNull] object parameter);

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this <see langword="object"/>
        /// can be set to null.
        /// </param>
        /// <exception cref="Exception">
        /// A <see langword="delegate"/> callback throws an exception.
        /// </exception>
        void Execute([CanBeNull] object parameter);

        /// <summary>
        ///     Occurs when the result of <see ref="CanExecute"/> changes and should be re-evaluated.
        /// </summary>
        event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Raises the <see cref="AlfredCommand.CanExecuteChanged"/> event.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}