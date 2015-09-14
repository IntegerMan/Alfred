// ---------------------------------------------------------
// TestMessageBoxProvider.cs
// 
// Created on:      09/05/2015 at 3:09 PM
// Last Modified:   09/05/2015 at 3:09 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Windows;

using MattEland.Ani.Alfred.PresentationShared.Helpers;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Controls
{
    /// <summary>
    /// A test message box provider. This class cannot be inherited.
    /// </summary>
    public sealed class TestMessageBoxProvider : MessageBoxProviderBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TestMessageBoxProvider"/> class.
        /// </summary>
        internal TestMessageBoxProvider()
        {
            // Set up our default error types
            ErrorOnImages = new HashSet<MessageBoxImage>
                            {
                                MessageBoxImage.Error,
                                MessageBoxImage.Asterisk,
                                MessageBoxImage.Exclamation,
                                MessageBoxImage.Warning,
                                MessageBoxImage.Stop
                            };
        }

        /// <summary>
        ///     The types of <see cref="MessageBoxImage"/> to fail tests on.
        /// </summary>
        public readonly ISet<MessageBoxImage> ErrorOnImages;

        /// <summary>
        ///     Shows a <paramref name="message"/> box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The <paramref name="message"/> caption.</param>
        /// <param name="buttons">The buttons to show.</param>
        /// <param name="icon">The icon to show.</param>
        protected override void Show(
            string message,
            string caption,
            MessageBoxButton buttons,
            MessageBoxImage icon)
        {
            // If it's an error message, fail
            string failMessage = $"Encountered error message:\n\n \t{caption}: {message} ({icon})\n";
            ErrorOnImages.ShouldNotContain(icon, failMessage);
        }
    }
}