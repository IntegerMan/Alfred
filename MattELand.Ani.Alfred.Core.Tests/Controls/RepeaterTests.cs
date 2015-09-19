// ---------------------------------------------------------
// RepeaterTests.cs
// 
// Created on:      09/19/2015 at 12:19 PM
// Last Modified:   09/19/2015 at 12:19 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Controls
{
    /// <summary>
    ///     Unit tests around the <see cref="Repeater"/> widget.
    /// </summary>
    [UnitTestProvider]
    public sealed class RepeaterTests : UserInterfaceTestBase
    {
        /// <summary>
        ///     The repeater's items collection should be widgets
        /// </summary>
        [Test]
        [SuppressMessage("ReSharper", "TryCastAlwaysSucceeds")]
        [SuppressMessage("ReSharper", "RedundantCast")]
        public void RepeaterItemsShouldBeWidgets()
        {
            //! Arrange

            var repeater = BuildRepeater();

            //! Act

            var items = repeater.Items;
            var widgets = items as IEnumerable<IWidget>;

            //! Assert

            widgets.ShouldNotBeNull();
        }

        /// <summary>
        ///     The repeater's items collection should not be <see langword="null"/>
        /// </summary>
        [Test]
        public void RepeaterItemsShouldNotBeNull()
        {
            //! Arrange

            var repeater = BuildRepeater();

            //! Act

            var items = repeater.Items;

            //! Assert

            items.ShouldNotBeNull();
        }

        /// <summary>
        ///     The repeater should use vertical stacked layout by default
        /// </summary>
        [Test]
        public void RepeaterShouldUseVerticalLayout()
        {
            //! Arrange

            var repeater = BuildRepeater();

            //! Act

            var layout = repeater.LayoutType;

            //! Assert

            layout.ShouldBe(LayoutType.VerticalStackPanel);
        }

    }
}