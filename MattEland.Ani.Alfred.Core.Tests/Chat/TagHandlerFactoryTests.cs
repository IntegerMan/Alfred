// ---------------------------------------------------------
// TagHandlerFactoryTests.cs
// 
// Created on:      08/18/2015 at 3:59 PM
// Last Modified:   08/18/2015 at 5:07 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    ///     A class to test <see cref="TagHandlerFactory" />.
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class TagHandlerFactoryTests : ChatTestsBase
    {
        /// <summary>
        ///     Sets up the test fixture for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            InitChatSystem();
        }

        /// <summary>
        ///     When building dynamic and encountering a node that does have a mapping, return a new tag
        ///     handler
        /// </summary>
        /// <remarks>
        ///     See AlF-29
        /// </remarks>
        [Test]
        public void BuildDynamicWithKnownTagReturnsNewHandler()
        {
            var factory = new TagHandlerFactory(Engine);
            var result = factory.BuildTagHandlerDynamic("srai",
                                                        BuildTagHandlerParameters(
                                                                                  "<srai>Testing Rocks</srai>"));

            Assert.IsNotNull(result);

            var handler = (RedirectTagHandler)result;
            Assert.AreEqual("Testing Rocks", handler.TemplateNode.InnerText);
        }

        /// <summary>
        ///     Parameter validation for parameters
        /// </summary>
        /// <remarks>
        ///     See ALF-29
        /// </remarks>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildDynamicWithNullParamsThrowsException()
        {
            var factory = new TagHandlerFactory(Engine);
            factory.BuildTagHandlerDynamic("Foo", null);
        }

        /// <summary>
        ///     Parameter validation for TagName
        /// </summary>
        /// <remarks>
        ///     See ALF-29
        /// </remarks>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildDynamicWithNullTagThrowsException()
        {
            var factory = new TagHandlerFactory(Engine);
            var parameters = BuildTagHandlerParameters("<Foo />");
            factory.BuildTagHandlerDynamic(null, parameters);
        }

        /// <summary>
        ///     When building dynamic and encountering a node that doesn't have any mapping, return null
        /// </summary>
        /// <remarks>
        ///     See ALF-29
        /// </remarks>
        [Test]
        public void BuildDynamicWithUnknownTagReturnsNull()
        {
            var factory = new TagHandlerFactory(Engine);
            var result = factory.BuildTagHandlerDynamic("Unknown",
                                                        BuildTagHandlerParameters("<unknown />"));

            Assert.IsNull(result);
        }

        /// <summary>
        /// Tag handler factories must have a ChatEngine
        /// </summary>
        /// <remarks>
        /// See ALF-29
        /// </remarks>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildFactoryWithNullEngineThrowsException()
        {
            var factory = new TagHandlerFactory(null);
        }
    }
}