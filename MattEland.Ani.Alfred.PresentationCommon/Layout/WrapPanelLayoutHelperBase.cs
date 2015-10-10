using MattEland.Common.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Ani.Alfred.PresentationCommon.Layout
{
    /// <summary>
    /// A layout helper class that provides common cross-platform arrangement logic for
    /// a wrapping panel
    /// </summary>
    /// <typeparam name="TChild">The type of the child.</typeparam>
    public abstract class WrapPanelLayoutHelperBase<TChild> : LayoutHelperBase<TChild>
    {

        /// <summary>
        ///     Arranges elements for a single line in a wrap panel.
        /// </summary>
        /// <remarks>
        ///     This method does not calculate wrapping and assumes that it has already been determined.
        /// </remarks>
        /// <param name="lineOtherStart"> The line's start point on the non-dominant extent. </param>
        /// <param name="lineOtherExtent"> Extent of the line's non-dominant dimension. </param>
        /// <param name="startIndex"> The start index. </param>
        /// <param name="endIndex"> The end index. </param>
        /// <param name="isHorizontal">
        ///     <see langword="true" /> if this instance uses horizontal arrangement.
        /// </param>
        /// <param name="children"> The children. </param>
        private void ArrangeWrappingLine(double lineOtherStart,
                                         double lineOtherExtent,
                                         int startIndex,
                                         int endIndex,
                                         bool isHorizontal,
                                         [NotNull] IList<TChild> children)
        {
            double lineSize = 0;

            // Move through the children we're working with. This assumes that line wrapping has already been computed
            for (var i = startIndex; i < endIndex; i++)
            {
                var child = children[i];

                if (child == null) continue;

                LayoutSize desiredSize = GetChildDesiredSize(child);

                if (isHorizontal)
                {
                    var childWidth = desiredSize.Width;

                    // Move it left to right
                    ArrangeChild(child, new LayoutSize(lineSize, lineOtherStart, childWidth, lineOtherExtent));
                    lineSize += childWidth;
                }
                else
                {
                    var childHeight = desiredSize.Height;

                    // Move it top to bottom
                    ArrangeChild(child, new LayoutSize(lineOtherStart, lineSize, lineOtherExtent, childHeight));
                    lineSize += childHeight;
                }
            }
        }

        /// <summary>
        ///     <see cref="ArrangeOverride"/> implementation for when <see cref="LayoutType"/> is
        ///     VerticalWrapPanel or HorizontalWrapPanel.
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
            var firstInLine = 0;
            double nonDominantOffset = 0;

            double maxSize = isHorizontal ? finalSize.Width : finalSize.Height;
            double lineDominantSize = 0;
            double lineNonDominantSize = 0;

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];

                if (child == null) continue;

                LayoutSize desiredSize = GetChildDesiredSize(child);

                var childDominantSize = isHorizontal
                                            ? desiredSize.Width
                                            : desiredSize.Height;

                var childNonDominantSize = isHorizontal
                                               ? desiredSize.Height
                                               : desiredSize.Width;

                // If we're done with the line, it's time to render
                if (lineDominantSize + childDominantSize > maxSize)
                {
                    // Arrange the line
                    ArrangeWrappingLine(nonDominantOffset,
                                        lineNonDominantSize,
                                        firstInLine,
                                        i,
                                        isHorizontal,
                                        children);

                    nonDominantOffset += lineNonDominantSize;

                    lineDominantSize = childDominantSize;
                    lineNonDominantSize = childNonDominantSize;

                    // If the next element is Larger then the constraint - give it a separate line
                    if (childDominantSize > maxSize)
                    {
                        // Switch to the next line which will only contain one element
                        ArrangeWrappingLine(nonDominantOffset,
                                            childNonDominantSize,
                                            i,
                                            ++i,
                                            isHorizontal,
                                            children);

                        nonDominantOffset += childNonDominantSize;

                        lineDominantSize = 0;
                    }

                    firstInLine = i;
                }
                else
                {
                    // We have more space to eat up
                    lineDominantSize += childDominantSize;
                    lineNonDominantSize = Math.Max(lineNonDominantSize, childNonDominantSize);
                }
            }

            //arrange the last line, if any
            if (firstInLine < children.Count)
            {
                ArrangeWrappingLine(nonDominantOffset,
                                    lineNonDominantSize,
                                    firstInLine,
                                    children.Count,
                                    isHorizontal,
                                    children);
            }

            return finalSize;
        }

        /// <summary>
        ///     <see cref="MeasureOverride"/> implementation for <see cref="WrapPanel"/> based layouts.
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
            var maxSize = isHorizontal ? availableSize.Width : availableSize.Height;

            var curLineDominant = 0.0;
            var curLineNonDominant = 0.0;

            var panelDominant = 0.0;
            var panelNonDominant = 0.0;

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];

                if (child == null) continue;

                // Ask the children how much they want
                MeasureChild(availableSize, child);

                LayoutSize desiredSize = GetChildDesiredSize(child);

                var childDominant = isHorizontal
                                        ? desiredSize.Width
                                        : desiredSize.Height;

                var childNonDominant = isHorizontal
                                           ? desiredSize.Height
                                           : desiredSize.Width;

                if (curLineDominant + childDominant > maxSize) // need to switch to another line
                {
                    panelDominant = Math.Max(curLineDominant, panelDominant);
                    panelNonDominant += curLineNonDominant;

                    curLineDominant = childDominant;
                    curLineNonDominant = childNonDominant;

                    //the element is wider then the constraint - give it a separate line                    
                    if (childDominant > maxSize)
                    {
                        panelDominant = Math.Max(childDominant, panelDominant);
                        panelNonDominant += childNonDominant;
                        curLineDominant = 0;
                    }
                }
                else // continue to accumulate a line
                {
                    curLineDominant += childDominant;
                    curLineNonDominant = Math.Max(childNonDominant, curLineNonDominant);
                }
            }

            // the last line size, if any should be added
            panelDominant = Math.Max(curLineDominant, panelDominant);
            panelNonDominant += curLineNonDominant;

            // Translate our dimensions to the appropriate ordering
            return isHorizontal
                       ? new LayoutSize(panelDominant, panelNonDominant)
                       : new LayoutSize(panelNonDominant, panelDominant);
        }


    }
}
