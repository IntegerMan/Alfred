using System;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.PresentationCommon.Helpers
{
    /// <summary>
    ///     An attribute that promises that this class can be instantiated as a view model for the specified model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ViewModelForAttribute : UsedImplicitlyAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the ViewModelForAttribute class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public ViewModelForAttribute([NotNull] Type model)
        {
            Model = model;
        }

        /// <summary>
        ///     Gets the model.
        /// </summary>
        /// <value>
        ///     The model.
        /// </value>
        [NotNull]
        public Type Model { get; }

    }
}