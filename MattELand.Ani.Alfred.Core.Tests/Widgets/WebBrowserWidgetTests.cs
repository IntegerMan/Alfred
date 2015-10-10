using MattEland.Common.Testing;
using NUnit.Framework;
using System;
using Shouldly;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Tests.Widgets
{
    /// <summary>
    ///     Contains tests related to <see cref="WebBrowserWidget"/>.
    /// </summary>
    [UnitTestProvider]
    public sealed class WebBrowserWidgetTests : WidgetTestsBase
    {
        /// <summary>
        ///     Checks to see if widgets start pointing at the correct URL
        /// </summary>
        [Test]
        public void WidgetHasUrlProperty()
        {
            //! Arrange / Act

            var browser = new WebBrowserWidget(BuildWidgetParams());

            //! Assert
            var expected = new Uri("http://www.bing.com/");
            browser.Url.ShouldBe(expected);
        }
    }
}
