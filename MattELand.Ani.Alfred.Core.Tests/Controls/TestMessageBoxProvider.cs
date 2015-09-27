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



using NUnit.Framework;

using Shouldly;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationShared.Helpers;

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
            ErrorOnMessageTypes = new HashSet<MessageBoxType>()
                            {
                                MessageBoxType.Error,
                                MessageBoxType.Warning
                            };
        }

        /// <summary>
        ///     The types of <see cref="MessageBoxType"/> to fail tests on.
        /// </summary>
        public readonly ISet<MessageBoxType> ErrorOnMessageTypes;

        /// <summary>
        ///     Shows a <paramref name="message"/> box.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="caption"> The <paramref name="message"/> caption. </param>
        /// <param name="alertType"> Type of the alert. </param>
        public override void Show(
            string message,
            string caption,
            MessageBoxType alertType)
        {
            // If it's an error message, fail
            string failMessage = $"Encountered error message:\n\n \t{caption}: {message} ({alertType})\n";
            ErrorOnMessageTypes.ShouldNotContain(alertType, failMessage);
        }
    }
}