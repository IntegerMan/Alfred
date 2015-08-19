// ---------------------------------------------------------
// WarningWidgetTests.cs
// 
// Created on:      08/08/2015 at 6:19 PM
// Last Modified:   08/08/2015 at 6:20 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Core.Widgets;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Widgets
{

    /// <summary>
    ///     Contains a suite of tests oriented around the <see cref="WarningWidget">WarningWidget</see> class.
    /// </summary>
    [TestFixture]
    public sealed class WarningWidgetTests
    {
        [Test]
        public void CanInstantiateWarningWidget()
        {
            var widget = new WarningWidget();
        }
    }
}