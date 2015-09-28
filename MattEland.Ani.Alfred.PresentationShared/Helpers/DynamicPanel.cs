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

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.PresentationAvalon.Helpers
{
    /// <summary>
    ///     A <see cref="Panel"/> for dynamic arrangement in modes similar to 
    ///     <see cref="WrapPanel"/>or <see cref="StackPanel"/>.
    /// </summary>
    [PublicAPI]
    public sealed class DynamicPanel : Panel
    {

        /// <summary>
        ///     Defines the <see cref="LayoutType"/> dependency property.
        /// </summary>
        /// <remarks>
        ///		Defaults to LayoutType.VerticalStackPanel
        /// </remarks>
        [NotNull]
        public static readonly DependencyProperty LayoutTypeProperty = DependencyProperty.Register("LayoutType",
                                                                         typeof(LayoutType),
                                                                         typeof(DynamicPanel),
                                                                         new PropertyMetadata(Core.Definitions.LayoutType.VerticalStackPanel, OnLayoutTypeChanged));


        /// <summary>
        ///     Handles the dependency property changed event for <see cref="LayoutTypeProperty"/>.
        /// </summary>
        /// <param name="panel"> The panel. </param>
        /// <param name="e"> Event information. </param>
        private static void OnLayoutTypeChanged(DependencyObject panel, DependencyPropertyChangedEventArgs e)
        {
            var dynamicPanel = (DynamicPanel)panel;
            dynamicPanel.InvalidateMeasure();
        }

        /// <summary>
        ///     Gets or sets the LayoutType property using <see cref="LayoutTypeProperty"/>.
        /// </summary>
        /// <value>The LayoutType.</value>
        public LayoutType LayoutType
        {
            get { return (LayoutType)GetValue(LayoutTypeProperty); }
            set { SetValue(LayoutTypeProperty, value); }
        }

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
                case LayoutType.VerticalStackPanel:
                    return StackPanelMeasureOverride(availableSize, false, children);

                case LayoutType.HorizontalStackPanel:
                    return StackPanelMeasureOverride(availableSize, true, children);

                case LayoutType.VerticalWrapPanel:
                    return WrapPanelMeasureOverride(availableSize, false, children);

                case LayoutType.HorizontalWrapPanel:
                    return WrapPanelMeasureOverride(availableSize, true, children);

                default:
                    return base.MeasureOverride(availableSize);
            }
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
        private static Size WrapPanelMeasureOverride(Size availableSize,
                                                     bool isHorizontal,
                                                     [NotNull] UIElementCollection children)
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
                child.Measure(availableSize);

                var childDominant = isHorizontal
                                        ? child.DesiredSize.Width
                                        : child.DesiredSize.Height;

                var childNonDominant = isHorizontal
                                           ? child.DesiredSize.Height
                                           : child.DesiredSize.Width;

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
                       ? new Size(panelDominant, panelNonDominant)
                       : new Size(panelNonDominant, panelDominant);
        }

        /// <summary>
        ///     <see cref="MeasureOverride"/> implementation for <see cref="StackPanel"/> based layouts.
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
                case LayoutType.VerticalStackPanel:
                    return StackPanelArrangeOverride(finalSize, false, children);

                case LayoutType.HorizontalStackPanel:
                    return StackPanelArrangeOverride(finalSize, true, children);

                case LayoutType.VerticalWrapPanel:
                    return WrapPanelArrangeOverride(finalSize, false, children);

                case LayoutType.HorizontalWrapPanel:
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
            double nonDominantOffset = 0;

            double maxSize = isHorizontal ? finalSize.Width : finalSize.Height;
            double lineDominantSize = 0;
            double lineNonDominantSize = 0;

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];

                if (child == null) continue;

                var childDominantSize = isHorizontal
                                            ? child.DesiredSize.Width
                                            : child.DesiredSize.Height;

                var childNonDominantSize = isHorizontal
                                               ? child.DesiredSize.Height
                                               : child.DesiredSize.Width;

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
        private static void ArrangeWrappingLine(double lineOtherStart,
                                                double lineOtherExtent,
                                                int startIndex,
                                                int endIndex,
                                                bool isHorizontal,
                                                UIElementCollection children)
        {
            double lineSize = 0;

            // Move through the children we're working with. This assumes that line wrapping has already been computed
            for (var i = startIndex; i < endIndex; i++)
            {
                var child = children[i];

                if (child == null) continue;

                if (isHorizontal)
                {
                    var childWidth = child.DesiredSize.Width;

                    // Move it left to right
                    child.Arrange(new Rect(lineSize, lineOtherStart, childWidth, lineOtherExtent));
                    lineSize += childWidth;
                }
                else
                {
                    var childHeight = child.DesiredSize.Height;

                    // Move it top to bottom
                    child.Arrange(new Rect(lineOtherStart, lineSize, lineOtherExtent, childHeight));
                    lineSize += childHeight;
                }
            }
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