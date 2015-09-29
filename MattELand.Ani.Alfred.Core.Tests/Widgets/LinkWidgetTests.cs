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

        [Test]
        public void LinkWidgetCanBeConstructed()
        {
            //! Arrange

            const string LinkText = "MyWidgetName";

            //! Act

            var link = new LinkWidget(LinkText, BuildWidgetParams());

            //! Assert

            link.Text.ShouldBe(LinkText);
        }
    }
}
