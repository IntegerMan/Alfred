using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.MFDMockUp.Views;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the bootup screen.
    /// </summary>
    [ViewModelFor(typeof(BootupScreenModel))]
    [UsedImplicitly]
    public sealed class BootupScreenViewModel : ScreenViewModel
    {
        /// <summary>
        ///     The model.
        /// </summary>
        [NotNull]
        private readonly BootupScreenModel _model;

        /// <summary>
        ///     Initializes a new instance of the BootupScreenViewModel class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public BootupScreenViewModel([NotNull] BootupScreenModel model) : base(model)
        {
            _model = model;
        }

        /// <summary>
        ///     Gets the progress.
        /// </summary>
        /// <value>
        ///     The progress.
        /// </value>
        public double Progress { get { return _model.Progress; } }

        /// <summary>
        ///     Gets a message to display while loading.
        /// </summary>
        /// <value>
        ///     A message to display while loading.
        /// </value>
        public string LoadingMessage { get { return _model.LoadingMessage; } }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other"> The view model to compare to this instance. </param>
        /// <returns>
        ///     true if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        private bool Equals([NotNull] BootupScreenViewModel other)
        {
            Contract.Requires(other != null);

            return _model.Equals(other._model);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is BootupScreenViewModel && Equals((BootupScreenViewModel)obj);
        }

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return _model.GetHashCode();
        }
    }

}
