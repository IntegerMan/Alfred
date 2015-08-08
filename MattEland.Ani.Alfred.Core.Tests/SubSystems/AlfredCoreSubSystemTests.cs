// ---------------------------------------------------------
// AlfredCoreSubSystemTests.cs
// 
// Created on:      08/08/2015 at 6:16 PM
// Last Modified:   08/08/2015 at 6:16 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Modules;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests.SubSystems
{
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class AlfredCoreSubSystemTests
    {
        [NotNull]
        private AlfredControlSubSystem _subsystem;

        [NotNull]
        private AlfredProvider _alfred;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _subsystem = new AlfredControlSubSystem();
            _alfred = new AlfredProvider();
        }

        [Test]
        public void SubsystemCanBeRegisteredInAlfred()
        {
            _alfred.Register(_subsystem);

            Assert.AreEqual(1, _alfred.SubSystems.Count(), "Subsystem was not registered");
            Assert.Contains(_subsystem, _alfred.SubSystems as ICollection, "The subsystem was not found in the collection");
        }

    }
}