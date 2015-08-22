// ---------------------------------------------------------
// ChatCommandTests.cs
// 
// Created on:      08/17/2015 at 9:56 PM
// Last Modified:   08/18/2015 at 3:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    ///     Tests for AimlTagHandlers of concern.
    /// </summary>
    [TestFixture]
    public class TagHandlerTests : ChatTestsBase
    {
        /// <summary>
        ///     Sets up the testing environment prior to each test run.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            InitChatSystem();
        }

        /// <summary>
        ///     Tests that random tags with no children return empty results.
        /// </summary>
        /// <remarks>
        ///     See ALF-29
        /// </remarks>
        [Test]
        public void RandomTagWithNoChildrenReturnsEmpty()
        {
            // Build a handler to test with
            var handler = BuildTagHandler<RandomTagHandler>("random", "<random />");

            // Ensure that the results are what we expect
            var result = handler.Transform();
            Assert.That(result.IsEmpty(), $"Tag Handler result of '{result}' was not empty as expected.");
        }

        /// <summary>
        ///     Tests that random tags with unexpected children return empty results.
        /// </summary>
        /// <remarks>
        ///     See ALF-29
        /// </remarks>
        [Test]
        public void RandomTagWithWrongChildTypeReturnsEmpty()
        {
            // Build a handler to test with
            var handler = BuildTagHandler<RandomTagHandler>("random", "<random><b>Dude</b><b>Bro</b></random>");

            // Ensure that the results are what we expect
            var result = handler.Transform();
            Assert.That(result.IsEmpty(), $"Tag Handler result of '{result}' was not empty as expected.");
        }

        [Test]
        public void TextSubstitutionHelperWithNullSettingsReturnsInput()
        {
            var input = "foo";
            var output = TextSubstitutionHelper.Substitute(null, input);

            Assert.AreEqual(input, output);
        }

        [Test]
        public void TextSubstitutionHelperWithNullInputReturnsEmpty()
        {
            var output = TextSubstitutionHelper.Substitute(null, null);

            Assert.IsNotNull(output);
            Assert.That(output.IsEmpty());
        }

        [Test]
        public void TextSubstitutionHelperSubstitutesText()
        {
            var subs = new SettingsManager();
            subs.Add("Foo", "Bar");
            var output = TextSubstitutionHelper.Substitute(subs, "Foo");

            Assert.AreEqual("Bar", output);
        }

        [Test]
        public void TextSubstitutionHelperHandlesEmptyEntries()
        {
            var subs = new SettingsManager();
            subs.Add("Foo", "Bar");
            subs.Add("Frodo", null);

            var input = "Baggins";
            var output = TextSubstitutionHelper.Substitute(subs, input);

            Assert.AreEqual(input, output);
        }

        [Test]
        public void TextSubstitutionHelperHandlesNullInput()
        {
            var output = TextSubstitutionHelper.Substitute(null, "Bubba", "Gump");

            Assert.IsNotNull(output);
            Assert.That(output.IsEmpty());
        }

        [Test]
        public void TextSubstitutionHelperHandlesReplacement()
        {
            var output = TextSubstitutionHelper.Substitute("Bubbark Shrimp ZeBubba Bubbazar Bubba Gump", "Bubba", "Gump");

            Assert.AreEqual("Bubbark Shrimp ZeBubba Bubbazar Gump Gump", output);
        }

    }
}