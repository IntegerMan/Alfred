using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Pages
{
    /// <summary>
    ///     Tests for the <see cref="WebBrowserPage"/>
    /// </summary>
    [UnitTestProvider]
    public sealed class WebBrowserPageTests : AlfredTestBase
    {
        /// <summary>
        ///     The web browser page's ID should be correct.
        /// </summary>
        [Test]
        public void PageShouldHaveCorrectId()
        {
            //! Arrange / Act

            var page = new WebBrowserPage(Container);

            //! Assert

            page.Id.ShouldBe("Browser");
        }


        [Test]
        public void BrowserDefaultsToCorrectUrl()
        {
            //! Arrange / Act

            var page = new WebBrowserPage(Container);

            //! Assert

            var expected = new Uri("http://www.bing.com/");

            page.Url.ShouldBe(expected);
        }

        [Test]
        public void BrowserWidgetMatchesPageOnChangeUrl()
        {
            //! Arrange

            var page = new WebBrowserPage(Container);
            var widget = page.Browser;

            //! Act

            Uri newUrl = new Uri("http://www.matteland.com");
            page.Url = newUrl;

            //! Assert

            page.Url.ShouldBe(newUrl);
            widget.Url.ShouldBe(newUrl);
        }

    }
}
