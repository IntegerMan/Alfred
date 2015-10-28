using MattEland.Common.Annotations;
using System;
using System.Collections.Generic;

namespace MattEland.Ani.Alfred.PresentationCommon.Layout
{
    /// <summary>
    /// A layout helper class that provides common cross-platform arrangement logic for
    /// an auto-arrangement panel
    /// </summary>
    /// <typeparam name="TChild">The type of the child.</typeparam>
    public abstract class AutoSpaceLayoutHelperBase<TChild> : LayoutHelperBase<TChild>
    {

        /// <summary>
        ///     <see cref="ArrangeOverride"/> implementation for when <see cref="LayoutType"/> is
        ///     an AutoSpacePanel
        /// </summary>
        /// <param name="finalSize">
        ///     The final area within the parent that this element should use to arrange itself and its
        ///     children.
        /// </param>
        /// <param name="isHorizontal">
        ///     <see langword="true"/> if this instance is horizontal arrangement.
        /// </param>
        /// <param name="children"> The children to arrange. </param>
        /// <returns>
        ///     The actual size used.
        /// </returns>
        public LayoutSize Arrange(LayoutSize finalSize,
                                  bool isHorizontal,
                                  [NotNull] IList<TChild> children)
        {
            var childRect = new LayoutSize(finalSize);


            double sizePerChild;
            if (isHorizontal)
            {
                sizePerChild = finalSize.Width / children.Count;
            }
            else
            {
                sizePerChild = finalSize.Height / children.Count;
            }

            // Arrange and Position Children.
            var numChildren = children.Count;
            for (int i = 0, count = numChildren; i < count; ++i)
            {
                var child = children[i];

                if (child == null) continue;

                var desiredSize = GetChildDesiredSize(child);

                if (isHorizontal)
                {
                    var diffBetweenAvailableSize = ((sizePerChild - desiredSize.Width) / 2.0);
                    childRect.X = sizePerChild * i + diffBetweenAvailableSize;

                    childRect.Width = desiredSize.Width;

                    childRect.Height = Math.Max(finalSize.Height, desiredSize.Height);
                }
                else
                {
                    var diffBetweenAvailableSize = ((sizePerChild - desiredSize.Height) / 2.0);
                    childRect.Y = sizePerChild * i + diffBetweenAvailableSize;

                    childRect.Height = desiredSize.Height;

                    childRect.Width = Math.Max(finalSize.Width, desiredSize.Width);
                }

                ArrangeChild(child, childRect);
            }

            return finalSize;
        }

        /// <summary>
        ///     <see cref="MeasureOverride"/> implementation for auto-space panel based layouts.
        /// </summary>
        /// <param name="availableSize">
        ///     The available size that this element can give to child elements. Infinity can be
        ///     specified as a value to indicate that the element will size to whatever content is
        ///     available.
        /// </param>
        /// <param name="isHorizontal">
        ///     <see langword="true" /> if this instance is horizontal arrangement.
        /// </param>
        /// <param name="children"> The children. </param>
        /// <returns>
        ///     The size that this element determines it needs during layout, based on its calculations
        ///     of child element sizes.
        /// </returns>
        public LayoutSize Measure(LayoutSize availableSize,
                                  bool isHorizontal,
                                  [NotNull] IList<TChild> children)
        {
            var stackDesiredSize = new LayoutSize();

            // Allow children as much size as they want
            if (isHorizontal)
            {
                if (!double.IsInfinity(availableSize.Width))
                {
                    availableSize.Width /= children.Count;
                }
            }
            else
            {
                if (!double.IsInfinity(availableSize.Height))
                {
                    availableSize.Height /= children.Count;
                }
            }

            double maxSize = 0;

            //  Iterate through children.
            for (int i = 0, count = children.Count; i < count; ++i)
            {
                // Get next child.
                var child = children[i];

                if (child == null) continue;

                // Measure the child.
                MeasureChild(availableSize, child);

                var childDesiredSize = GetChildDesiredSize(child);

                // Accumulate child size.
                if (isHorizontal)
                {
                    maxSize = Math.Max(maxSize, childDesiredSize.Width);

                    stackDesiredSize.Height = Math.Max(stackDesiredSize.Height,
                                                       childDesiredSize.Height);
                }
                else
                {
                    stackDesiredSize.Width = Math.Max(stackDesiredSize.Width,
                                                      childDesiredSize.Width);

                    maxSize = Math.Max(maxSize, childDesiredSize.Height);
                }
            }

            // Apply the max size to each child
            var totalSize = maxSize * children.Count;
            if (isHorizontal)
            {
                stackDesiredSize.Width = totalSize;
            }
            else
            {
                stackDesiredSize.Height = totalSize;
            }

            return stackDesiredSize;
        }

    }
}

