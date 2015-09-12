using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.PresentationShared.Controls;
using MattEland.Ani.Alfred.PresentationShared.Helpers;
using MattEland.Common.Providers;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Controls
{
    /// <summary>
    /// An <c>abstract</c> class to help with user interface testing
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "IsExpressionAlwaysTrue")]
    public abstract class UserInterfaceTestBase : MockEnabledAlfredTestBase
    {
        /// <summary>
        ///     Gets the message box provider.
        /// </summary>
        /// <value>
        ///     The message box.
        /// </value>
        protected TestMessageBoxProvider MessageBox { get; private set; }

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            // Register a message box provider instance
            MessageBox = new TestMessageBoxProvider();
            MessageBox.RegisterAsProvidedInstance(typeof(IMessageBoxProvider));
        }

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
        /// Initializes the <paramref name="control"/>.
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
        /// <param name="controlType"><see cref="Type"/> of the control.</param>
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