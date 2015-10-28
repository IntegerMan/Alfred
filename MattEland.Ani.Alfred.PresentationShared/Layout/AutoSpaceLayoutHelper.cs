using MattEland.Ani.Alfred.PresentationCommon.Layout;
using MattEland.Common.Annotations;
using System.Windows;

namespace MattEland.Ani.Alfred.PresentationAvalon.Layout
{
    /// <summary>
    /// A layout helper for Stack Panel arrangements
    /// </summary>
    internal sealed class AutoSpaceLayoutHelper : AutoSpaceLayoutHelperBase<UIElement>
    {
        /// <summary>
        /// Arranges the child at the given dimensions.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="childRect">The child dimensions.</param>
        protected override void ArrangeChild(UIElement child, LayoutSize childRect)
        {
            var rect = childRect.ToRect();

            child.Arrange(rect);
        }

        /// <summary>
        /// Gets the child's desired size.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <returns>Desired size.</returns>
        protected override LayoutSize GetChildDesiredSize([NotNull] UIElement child)
        {
            var desiredSize = child.DesiredSize;

            return desiredSize.ToLayoutSize();
        }

        /// <summary>
        /// Measures the child.
        /// </summary>
        /// <param name="availableSize">The available size</param>
        /// <param name="child">The child.</param>
        protected override void MeasureChild(LayoutSize availableSize, UIElement child)
        {
            var size = availableSize.ToSize();

            child.Measure(size);
        }
    }
}
