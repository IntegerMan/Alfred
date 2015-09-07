// ---------------------------------------------------------
// ModuleTests.cs
// 
// Created on:      08/25/2015 at 11:38 AM
// Last Modified:   08/25/2015 at 11:53 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Testing;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Modules
{
    /// <summary>
    ///     Contains general test cases related to <see cref="AlfredModule" />.
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    public class ModuleTests : AlfredTestBase
    {

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Container.RegisterProvidedInstance(typeof(IAlfred), new AlfredApplication(Container));
        }

        /// <summary>
        ///     Builds widget creation parameters.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="WidgetCreationParameters"/>.</returns>
        [NotNull]
        private WidgetCreationParameters BuildWidgetParams([NotNull] string name)
        {
            return new WidgetCreationParameters(name, Container);
        }

        /// <summary>
        ///     Checks that modules with widgets yield all widgets when PropertyProviders is evaluated
        /// </summary>
        /// <remarks>
        ///     Test ALF-77 for story ALF-15
        /// </remarks>
        [Test]
        public void ModulesWithWidgetsShouldListAllChildWidgets()
        {
            //! Arrange

            const string TextBlockName = @"lblFoo";
            var textWidget = new TextWidget(BuildWidgetParams(TextBlockName));

            const string ButtonName = @"btnBar";
            var buttonWidget = new ButtonWidget(BuildWidgetParams(ButtonName));

            // Register our controls
            var module = new SimpleModule(Container, "Test Module");
            module.WidgetsToRegisterOnInitialize.Add(textWidget);
            module.WidgetsToRegisterOnInitialize.Add(buttonWidget);

            //! Act

            /* We just want to test that module.Initialize causes widgets to be added 
               and that those widgets show up as property providers. There's no need for
               a page, module, or subsystem to do this */

            module.InitializeWithoutPageOrSubsystem();

            //! Assert

            // Grab the values for assertion
            var providers = module.PropertyProviders.ToList();

            // Make sure the items we just put in there are present
            providers.Find(TextBlockName).ShouldNotBeNull($"Could not find {TextBlockName}");
            providers.Find(ButtonName).ShouldNotBeNull($"Could not find {ButtonName}");
        }

    }
}