using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MattEland.Ani.Alfred.Core;
using NUnit.Framework;

namespace MattELand.Ani.Alfred.Core.Tests
{
    [TestFixture]
    public class AlfredTests
    {

        [Test]
        public void InitializeAlfred()
        {
            var alfred = new AlfredProvider();
        }
    }
}
