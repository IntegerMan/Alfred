using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Widgets
{
    /// <summary>
    /// Tests related to <see cref="LinkWidget"/>
    /// </summary>
    [UnitTestProvider]
    public sealed class LinkWidgetTests : WidgetTestsBase
    {

        /// <summary>
        /// Tests that LinkWidget instances can be created
        /// </summary>
        [Test]
        public void LinkWidgetCanBeConstructed()
        {
            //! Arrange

            var text = Some.Text;

            //! Act

            var link = new LinkWidget(text, BuildWidgetParams());

            //! Assert

            link.Text.ShouldBe(text);
        }
    }
}
