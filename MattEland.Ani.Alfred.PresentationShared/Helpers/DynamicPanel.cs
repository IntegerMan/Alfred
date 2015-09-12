// ---------------------------------------------------------
// DynamicPanel.cs
// 
// Created on:      09/12/2015 at 1:04 PM
// Last Modified:   09/12/2015 at 1:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.PresentationShared.Helpers
{
    /// <summary>
    ///     A <see cref="Panel"/> for dynamic arrangement in modes similar to 
    ///     <see cref="WrapPanel"/>or <see cref="StackPanel"/>.
    /// </summary>
    public sealed class DynamicPanel : Panel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicPanel"/> class.
        /// </summary>
        public DynamicPanel()
        {
            LayoutType = LayoutTypes.VerticalStackPanel;
        }

        /// <summary>
        ///     Governs various layout types supported by <see cref="DynamicPanel"/>
        /// </summary>
        public enum LayoutTypes
        {
            /// <summary>
            ///     Represents a layout similar to StackPanel in Vertical orientation
            /// </summary>
            VerticalStackPanel,

            /// <summary>
            ///     Represents a layout similar to StackPanel in Horizontal orientation
            /// </summary>
            HorizontalStackPanel,

            /// <summary>
            ///     Represents a layout similar to WrapPanel in Vertical orientation
            /// </summary>
            VerticalWrapPanel,

            /// <summary>
            ///     Represents a layout similar to WrapPanel in Horizontal orientation
            /// </summary>
            HorizontalWrapPanel
        }

        /// <summary>
        ///     Gets or sets the type of the layout.
        /// </summary>
        /// <value>
        ///     The type of the layout.
        /// </value>
        public LayoutTypes LayoutType { get; set; }

        /// <summary>
        ///     When overridden in a derived class, measures the size in layout required for child
        ///     elements and determines a size for the <see cref="T:FrameworkElement"/>-
        ///     derived class.
        /// </summary>
        /// <param name="availableSize">
        ///     The available size that this element can give to child elements. Infinity can be
        ///     specified as a value to indicate that the element will size to whatever content is
        ///     available.
        /// </param>
        /// <returns>
        ///     The size that this element determines it needs during layout, based on its calculations
        ///     of child element sizes.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var children = InternalChildren;
            Debug.Assert(children != null);

            switch (LayoutType)
            {
                case LayoutTypes.VerticalStackPanel:
                    return StackPanelMeasureOverride(availableSize, false, children);

                case LayoutTypes.HorizontalStackPanel:
                    return StackPanelMeasureOverride(availableSize, true, children);

                case LayoutTypes.VerticalWrapPanel:

                    // TODO
                    return base.MeasureOverride(availableSize);

                case LayoutTypes.HorizontalWrapPanel:

                    // TODO
                    return base.MeasureOverride(availableSize);

                default:
                    return base.MeasureOverride(availableSize);
            }
        }

        /// <summary>
        ///     <see cref="MeasureOverride"/> implementation for <see cref="StackPanel"/> based layouts.
        /// </summary>
        /// <param name="availableSize">
        /// The available size that this element can give to child elements. Infinity can be
        /// specified as a value to indicate that the element will size to whatever content is
        /// available.
        /// </param>
        /// <param name="isHorizontal">
        /// <see langword="true" /> if this instance is horizontal arrangement.
        /// </param>
        /// <param name="children">The children.</param>
        /// <returns>A Size.</returns>
        private static Size StackPanelMeasureOverride(Size availableSize,
                                                      bool isHorizontal,
                                                      [NotNull] UIElementCollection children)
        {
            var stackDesiredSize = new Size();

            // Allow children as much size as they want
            if (isHorizontal)
            {
                availableSize.Width = double.PositiveInfinity;
            }
            else
            {
                availableSize.Height = double.PositiveInfinity;
            }

            //  Iterate through children.
            for (int i = 0, count = children.Count; i < count; ++i)
            {
                // Get next child.
                var child = children[i];

                if (child == null) continue;

                // Measure the child.
                child.Measure(availableSize);
                var childDesiredSize = child.DesiredSize;

                // Accumulate child size.
                if (isHorizontal)
                {
                    stackDesiredSize.Width += childDesiredSize.Width;
                    stackDesiredSize.Height = Math.Max(stackDesiredSize.Height,
                                                       childDesiredSize.Height);
                }
                else
                {
                    stackDesiredSize.Width = Math.Max(stackDesiredSize.Width, childDesiredSize.Width);
                    stackDesiredSize.Height += childDesiredSize.Height;
                }
            }

            return stackDesiredSize;
        }

        /// <summary>
        ///     When overridden in a derived class, positions child elements and determines a size for a
        ///     <see cref="T:FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="finalSize">
        ///     The final area within the parent that this element should use to arrange itself and its
        ///     children.
        /// </param>
        /// <returns>
        ///     The actual size used.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var children = InternalChildren;
            Debug.Assert(children != null);

            switch (LayoutType)
            {
                case LayoutTypes.VerticalStackPanel:
                    return StackPanelArrangeOverride(finalSize, false, children);

                case LayoutTypes.HorizontalStackPanel:
                    return StackPanelArrangeOverride(finalSize, true, children);

                case LayoutTypes.VerticalWrapPanel:
                    return WrapPanelArrangeOverride(finalSize, false, children);

                case LayoutTypes.HorizontalWrapPanel:
                    return WrapPanelArrangeOverride(finalSize, true, children);

                default:
                    return base.ArrangeOverride(finalSize);
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
        private static Size WrapPanelArrangeOverride(Size finalSize,
                                                     bool isHorizontal,
                                                     [NotNull] UIElementCollection children)
        {
            var firstInLine = 0;
            double accumulatedV = 0;

            double maxSize = isHorizontal ? finalSize.Width : finalSize.Height;
            double lineSize = 0;
            double lineOtherSize = 0;

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];

                if (child == null) continue;

                var childSize = GetChildSizeForWrapPanel(child, isHorizontal);

                // Do we need to move on to the next line?
                if (lineSize + childSize.Width > maxSize)
                {
                    // Arrange the line
                    ArrangeLine(accumulatedV, lineOtherSize, firstInLine, i, isHorizontal, children);

                    accumulatedV += lineOtherSize;
                    lineSize = childSize.Width;

                    // The element is Larger then the constraint - give it a separate line
                    if (childSize.Width > maxSize)
                    {
                        // Switch to the next line which will only contain one element
                        ArrangeLine(accumulatedV, childSize.Height, i, ++i, isHorizontal, children);

                        accumulatedV += childSize.Height;

                        lineSize = 0;
                    }

                    firstInLine = i;
                }
                else
                {
                    // We have more space to eat up
                    lineSize += childSize.Width;
                    lineOtherSize = Math.Max(lineOtherSize, childSize.Height);
                }
            }

            //arrange the last line, if any
            if (firstInLine < children.Count)
            {
                ArrangeLine(accumulatedV,
                            lineOtherSize,
                            firstInLine,
                            children.Count,
                            isHorizontal,
                            children);
            }

            return finalSize;
        }

        private static Size GetChildSizeForWrapPanel(UIElement child, bool isHorizontal)
        {
            var childMainSize = isHorizontal ? child.DesiredSize.Width : child.DesiredSize.Height;

            var childOtherSize = isHorizontal ? child.DesiredSize.Height : child.DesiredSize.Width;

            var childSize = new Size(childMainSize, childOtherSize);
            return childSize;
        }

        private static void ArrangeLine(double accumulatedV,
                                        double lineOtherSize,
                                        int firstItemIndex,
                                        int lastItemIndex,
                                        bool isHorizontal,
                                        UIElementCollection children)
        {

            throw NotImplementedException();

        }

        /// <summary>
        ///     <see cref="ArrangeOverride"/> implementation for when <see cref="LayoutType"/> is
        ///     VerticalStackPanel or HorizontalStackPanel.
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
        private static Size StackPanelArrangeOverride(Size finalSize,
                                                      bool isHorizontal,
                                                      [NotNull] UIElementCollection children)
        {
            var childRect = new Rect(finalSize);
            var previousChildSize = 0.0;

            // Arrange and Position Children.
            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];

                if (child == null) continue;

                if (isHorizontal)
                {
                    childRect.X += previousChildSize;

                    childRect.Width = child.DesiredSize.Width;
                    previousChildSize = childRect.Width;

                    childRect.Height = Math.Max(finalSize.Height, child.DesiredSize.Height);
                }
                else
                {
                    childRect.Y += previousChildSize;

                    childRect.Height = child.DesiredSize.Height;
                    previousChildSize = childRect.Height;

                    childRect.Width = Math.Max(finalSize.Width, child.DesiredSize.Width);
                }

                child.Arrange(childRect);
            }
            return finalSize;
        }

    }

}