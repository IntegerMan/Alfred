// ---------------------------------------------------------
// ModuleTests.cs
// 
// Created on:      08/25/2015 at 11:38 AM
// Last Modified:   08/25/2015 at 11:53 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Ani.Alfred.Tests.Mocks;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Modules
{
    /// <summary>
    ///     Contains general test cases related to <see cref="AlfredModule" />.
    /// </summary>
    [TestFixture]
    public class ModuleTests
    {

        /// <summary>
        ///     Builds widget creation parameters.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The WidgetCreationParameters.</returns>
        [NotNull]
        private static WidgetCreationParameters BuildWidgetParams([NotNull] string name)
        {
            return new WidgetCreationParameters(name);
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
            const string TextBlockName = "lblFoo";
            const string ButtonName = "btnBar";

            // Register our controls
            var module = new AlfredTestModule();
            module.WidgetsToRegisterOnInitialize.Add(new TextWidget(BuildWidgetParams(TextBlockName)));
            module.WidgetsToRegisterOnInitialize.Add(new ButtonWidget(BuildWidgetParams(ButtonName)));

            // Don't do this normally, but this will be enough to populate the collection to evaluate
            module.Initialize(new TestAlfred());

            // Grab the values for assertion
            var providers = module.PropertyProviders.ToList();

            // Make sure the items we just put in there are present
            Assert.IsNotNull(providers.Find(TextBlockName), $"Could not find {TextBlockName}");
            Assert.IsNotNull(providers.Find(ButtonName), $"Could not find {ButtonName}");
        }
    }
}