using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common.Annotations;
using MattEland.Presentation.Logical.Widgets;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Widgets
{
    /// <summary>
    ///     A widget view model factory.
    /// </summary>
    internal static class WidgetViewModelFactory
    {
        /// <summary>
        ///     Occurs when a widget view model is created.
        /// </summary>
        public static event EventHandler<WidgetViewModel> WidgetViewModelCreated;

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

            // TODO: Refactor this as new widget types are supported

            // Build a VM or return a cached VM
            output = TryCreateViewModel<ProgressBarWidget, ProgressBarWidgetViewModel>(widget);
            if (output == null)
            {
                // Build a default VM
                output = new WidgetViewModel(widget);
            }

            // Tell any observer that we have a new instance.
            RaiseWidgetCreated(output);

            return output;
        }

        /// <summary>
        ///     Tries to create a view model for the <paramref name="widget"/> using a specified cast
        ///     assumption from the two generic type parameters where we assume the widget might be the
        ///     specified <typeparamref name="TModel" /> and, if it is, we want to instantiate the
        ///     specified <typeparamref name="TViewModel"/> for it.
        /// </summary>
        /// <typeparam name="TModel">
        ///     Type of the model we suspect <paramref name="widget"/> might be.
        /// </typeparam>
        /// <typeparam name="TViewModel">
        ///     Type of the view model to instantiate if <paramref name="widget"/> is a
        ///     <typeparamref name="TModel"/>.
        /// </typeparam>
        /// <param name="widget"> The widget. </param>
        /// <returns>
        ///     A view model of the specified type if the assumption was correct or null otherwise.
        /// </returns>
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
                    return null;
                }

            }

            return null;
        }

        /// <summary>
        ///     Raises the widget created event.
        /// </summary>
        /// <param name="e"> The widget that was created. </param>
        private static void RaiseWidgetCreated([NotNull] WidgetViewModel e)
        {
            Contract.Requires(e != null);

            WidgetViewModelCreated?.Invoke(null, e);
        }
    }
}
