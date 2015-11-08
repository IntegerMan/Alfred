using System.Diagnostics.Contracts;

using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Widgets
{
    /// <summary>
    ///     A ViewModel for a progress bar widget. This class cannot be inherited.
    /// </summary>
    public sealed class ProgressBarWidgetViewModel : WidgetViewModel
    {
        /// <summary>
        ///     Initializes a new instance of the ProgressBarWidgetViewModel class.
        /// </summary>
        /// <param name="widget"> The widget. </param>
        public ProgressBarWidgetViewModel([NotNull] ProgressBarWidget widget) : base(widget)
        {
            Contract.Requires(widget != null);

            ProgressBar = widget;
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(ProgressBar != null);
        }

        /// <summary>
        ///     Gets the progress bar widget.
        /// </summary>
        /// <value>
        ///     The progress bar.
        /// </value>
        [NotNull]
        public ProgressBarWidget ProgressBar { get; }
    }
}