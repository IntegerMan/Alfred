using System;

namespace MattEland.Ani.Alfred.PresentationCommon.Helpers
{
    /// <summary>
    ///     An attribute that promises that this class can be instantiated as a view model for the specified model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewModelForAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the ViewModelForAttribute class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public ViewModelForAttribute(Type model)
        {
            Model = model;
        }

        /// <summary>
        ///     Gets or sets the model.
        /// </summary>
        /// <value>
        ///     The model.
        /// </value>
        public Type Model { get; set; }

    }
}