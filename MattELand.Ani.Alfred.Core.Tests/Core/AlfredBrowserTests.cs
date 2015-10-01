using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common;
using MattEland.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using MattEland.Ani.Alfred.Core;

namespace MattEland.Ani.Alfred.Tests.Core
{
    /// <summary>
    ///     A class containing tests related to Alfred's web browsing capabilities
    /// </summary>
    [UnitTestProvider]
    public sealed class AlfredBrowserTests : AlfredTestBase
    {
        public override void SetUp()
        {
            base.SetUp();

            Alfred = BuildAlfredInstance();
        }

        /// <summary>
        ///    Tests that the core subsystem should include a browser page.
        /// </summary>
        [Test]
        public void CoreSubsystemShouldIncludeWebBrowserPage()
        {
            //! Arrange

            var core = new AlfredCoreSubsystem(Container);

            Alfred.Register(core);
            Alfred.Initialize();

            //! Act

            var page = core.RootPages.FirstOrDefault(p => p.Id.Matches("Browser"));

            //! Assert

            page.ShouldNotBeNull("Web Browser Page was not found");
            page.ShouldBe<WebBrowserPage>();
        }

    }
}
