using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.PresentationShared.Controls;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Controls
{
    /// <summary>
    /// An abstract class to help with user interface testing
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "IsExpressionAlwaysTrue")]
    public abstract class UserInterfaceTestBase
    {

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        [NotNull]
        protected IUserInterfaceTestable Control
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        /// <param name="control">The control.</param>
        protected void InitializeControl([NotNull] IUserInterfaceTestable control)
        {
            Control = control;
            Control.SimulateLoadedEvent();
        }

        /// <summary>
        /// Asserts that a control of the given type is present.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="controlType">Type of the control.</param>
        protected static void AssertHasControl([CanBeNull] FrameworkElement element,
                                               [NotNull] Type controlType)
        {
            Assert.IsNotNull(element, "Control was not found");
            var elementType = element.GetType();
            Assert.That(elementType == controlType || elementType.IsSubclassOf(controlType), "Element could not be cast as " + controlType.Name);
        }

        /// <summary>
        /// Asserts that a <see cref="TextBox"/> of the given type is present.
        /// </summary>
        /// <param name="element">The element.</param>
        [AssertionMethod]
        protected static void AssertHasTextBox([CanBeNull, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] FrameworkElement element)
        {
            AssertHasControl(element, typeof(TextBoxBase));
        }

        /// <summary>
        /// Asserts that a <see cref="Button"/> of the given type is present.
        /// </summary>
        /// <param name="element">The element.</param>
        [AssertionMethod]
        protected static void AssertHasButton([CanBeNull, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] FrameworkElement element)
        {
            AssertHasControl(element, typeof(ButtonBase));
        }

        /// <summary>
        /// Asserts that a <see cref="TreeView"/> of the given type is present.
        /// </summary>
        /// <param name="element">The element.</param>
        [AssertionMethod]
        protected static void AssertHasTreeView([CanBeNull, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] FrameworkElement element)
        {
            AssertHasControl(element, typeof(TreeView));
        }

        /// <summary>
        /// Asserts that a <see cref="DataGrid"/> of the given type is present.
        /// </summary>
        /// <param name="element">The element.</param>
        [AssertionMethod]
        protected static void AssertHasDataGrid([CanBeNull, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] FrameworkElement element)
        {
            AssertHasControl(element, typeof(DataGrid));
        }

        /// <summary>
        /// Asserts that a <see cref="TabControl"/> of the given type is present.
        /// </summary>
        /// <param name="element">The element.</param>
        [AssertionMethod]
        protected static void AssertHasTabControl([CanBeNull, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] FrameworkElement element)
        {
            AssertHasControl(element, typeof(TabControl));
        }
    }
}