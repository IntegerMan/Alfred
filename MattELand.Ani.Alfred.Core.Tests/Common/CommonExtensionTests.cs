﻿// ---------------------------------------------------------
// CommonTests.cs
// 
// Created on:      08/18/2015 at 3:13 PM
// Last Modified:   08/18/2015 at 3:13 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Common;
using MattEland.Common.Testing;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Common
{
    /// <summary>
    /// Contains tests related to MattEland.Common extensions
    /// </summary>
    [UnitTestProvider]
    public sealed class CommonExtensionTests : AlfredTestBase
    {

        [Test]
        public void Pluralize1ResultsInSingular()
        {
            var i = 1;

            var pluralized = i.Pluralize("Singular", "Plural");

            Assert.AreEqual("Singular", pluralized);
        }

        [Test]
        public void Pluralize0ResultsInPlural()
        {
            var i = 0;

            var pluralized = i.Pluralize("Singular", "Plural");

            Assert.AreEqual("Plural", pluralized);
        }

        [Test]
        public void Pluralize42ResultsInPlural()
        {
            var i = 42;

            var pluralized = i.Pluralize("Singular", "Plural");

            Assert.AreEqual("Plural", pluralized);
        }
    }
}