// ---------------------------------------------------------
// XamlCommandTests.cs
// 
// Created on:      09/13/2015 at 11:19 AM
// Last Modified:   09/13/2015 at 11:19 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Ani.Alfred.WPF;
using MattEland.Common.Providers;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Presentation
{
    /// <summary>
    ///     XAML command tests containing tests related to the WPF client,
    ///     <see cref="XamlClientCommand"/>, and buttons.
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    public sealed class XamlCommandTests : UserInterfaceTestBase
    {
        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

        }

        /// <summary>
        ///     Commands provided by the Container should be <see cref="XamlClientCommand"/> objects.
        /// </summary>
        [Test]
        [RequiresSTA]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void ContainerCommandShouldBeXamlCommand()
        {
            //! Arrange
            IAlfredCommand command;
            var container = new CommonContainer { Name = CurrentTestName };

            //! Act
            using (var window = new MainWindow(container, false))
            {
                var app = window.Application;

                command = app.Container.Provide<IAlfredCommand>();
            }

            //! Assert

            command.ShouldNotBeNull();
            command.ShouldBeOfType(typeof(XamlClientCommand));
            command.ShouldImplementInterface<ICommand>();
        }
    }
}