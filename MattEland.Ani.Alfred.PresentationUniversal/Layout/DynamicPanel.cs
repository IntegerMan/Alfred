﻿// ---------------------------------------------------------
// DynamicPanel.cs
// 
// Created on:      09/12/2015 at 1:04 PM
// Last Modified:   09/12/2015 at 1:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using MattEland.Ani.Alfred.PresentationCommon.Layout;
using System.Collections.Generic;

namespace MattEland.Ani.Alfred.PresentationUniversal.Layout
{
    /// <summary>
    /// A <see cref="Panel" /> for dynamic arrangement in modes similar to
    /// <see cref="WrapPanel" />or <see cref="StackPanel" />.
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
        /// Initializes a new instance of the <see cref="DynamicPanel"/> class.
        /// </summary>
        public DynamicPanel()
        {
            StackLayoutHelper = new StackPanelLayoutHelper();
            WrapLayoutHelper = new WrapPanelLayoutHelper();
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
        /// Gets the layout helper for stack panel layouts.
        /// </summary>
        /// <value>The layout helper.</value>
        [NotNull]
        private StackPanelLayoutHelper StackLayoutHelper { get; }

        /// <summary>
        /// Gets the layout helper for wrap panel layouts.
        /// </summary>
        /// <value>The layout helper.</value>
        [NotNull]
        private WrapPanelLayoutHelper WrapLayoutHelper { get; }

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
            IList<UIElement> children = Children;

            LayoutSize layoutSize = finalSize.ToLayoutSize();

            LayoutSize result = new LayoutSize();

            switch (LayoutType)
            {
                case LayoutType.VerticalStackPanel:
                    result = StackLayoutHelper.Arrange(layoutSize, false, children);
                    break;

                case LayoutType.HorizontalStackPanel:
                    result = StackLayoutHelper.Arrange(layoutSize, true, children);
                    break;

                case LayoutType.VerticalWrapPanel:
                    result = WrapLayoutHelper.Arrange(layoutSize, false, children);
                    break;

                case LayoutType.HorizontalWrapPanel:
                    result = WrapLayoutHelper.Arrange(layoutSize, false, children);
                    break;
            }

            return result.ToSize();

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
            IList<UIElement> children = Children;

            LayoutSize layoutSize = availableSize.ToLayoutSize();

            LayoutSize result = new LayoutSize();

            switch (LayoutType)
            {
                case LayoutType.VerticalStackPanel:
                    result = StackLayoutHelper.Measure(layoutSize, false, children);
                    break;

                case LayoutType.HorizontalStackPanel:
                    result = StackLayoutHelper.Measure(layoutSize, true, children);
                    break;

                case LayoutType.VerticalWrapPanel:
                    result = WrapLayoutHelper.Measure(layoutSize, false, children);
                    break;

                case LayoutType.HorizontalWrapPanel:
                    result = WrapLayoutHelper.Measure(layoutSize, true, children);
                    break;
            }

            return result.ToSize();
        }

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

    }

}