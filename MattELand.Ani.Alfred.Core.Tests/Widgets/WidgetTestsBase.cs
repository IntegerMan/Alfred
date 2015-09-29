using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Tests.Widgets
{
    /// <summary>
    /// A base class for testing widgets.
    /// </summary>
    public abstract class WidgetTestsBase : AlfredTestBase
    {
        /// <summary>
        ///     Builds widget parameters.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The WidgetCreationParameters.</returns>
        [NotNull]
        protected WidgetCreationParameters BuildWidgetParams(string name = "WidgetTest")
        {
            return new WidgetCreationParameters(name, Container);
        }

    }
}