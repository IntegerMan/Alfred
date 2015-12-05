// ---------------------------------------------------------
// WidgetViewModel.cs
// 
// Created on:      11/05/2015 at 2:56 PM
// Last Modified:   11/05/2015 at 4:24 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.Contracts;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;
using MattEland.Presentation.Logical.Widgets;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Widgets
{
    /// <summary>
    ///     A ViewModel for widgets.
    /// </summary>
    public class WidgetViewModel
    {
        /// <summary>
        ///     Initializes a new instance of the WidgetViewModel class.
        /// </summary>
        /// <param name="widget"> The widget. </param>
        public WidgetViewModel([NotNull] IWidget widget)
        {
            Widget = widget;

            // Register with instantiation monitor
            InstantiationMonitor.Instance.NotifyItemCreated(this);
        }

        /// <summary>
        ///     Gets the widget.
        /// </summary>
        /// <value>
        ///     The widget.
        /// </value>
        [NotNull]
        public IWidget Widget { get; }

        /// <summary>
        ///     Gets the name of the widget for displaying in the user interface.
        /// </summary>
        /// <value>
        ///     The display name of the widget.
        /// </value>
        public string DisplayName
        {
            get { return Widget.DisplayName; }
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(Widget != null);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return "{" + GetType().Name + "}";
        }

        /// <summary>
        ///     Updates the values from the widget.
        /// </summary>
        public virtual void UpdateValues()
        {
            // To be implemented by overriding classes
        }
    }

}