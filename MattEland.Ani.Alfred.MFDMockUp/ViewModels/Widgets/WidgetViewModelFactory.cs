using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common.Annotations;
using MattEland.Presentation.Logical.Widgets;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Widgets
{
    /// <summary>
    ///     A widget view model factory.
    /// </summary>
    public static class WidgetViewModelFactory
    {
        /// <summary>
        ///     Creates a view model for the specified widget.
        /// </summary>
        /// <param name="widget"> The widget. </param>
        /// <returns>
        ///     A view model appropriate for that widget.
        /// </returns>
        [NotNull]
        public static WidgetViewModel ViewModelFor([NotNull] IWidget widget)
        {
            Contract.Requires(widget != null);
            Contract.Ensures(Contract.Result<WidgetViewModel>() != null);

            WidgetViewModel output = null;

            output = TryCreateViewModel<ProgressBarWidget, ProgressBarWidgetViewModel>(widget);
            if (output != null) return output;

            return new WidgetViewModel(widget);
        }

        private static TViewModel TryCreateViewModel<TModel, TViewModel>(IWidget widget) where TViewModel : WidgetViewModel
        {
            if (widget is TModel)
            {
                // Constructors should follow a single parameter model, so this *should* work.
                try
                {
                    return (TViewModel)Activator.CreateInstance(typeof(TViewModel), widget);
                }
                catch (Exception)
                {
                    // It's cool. We'll just default it.
                }

            }

            return null;
        }
    }
}
