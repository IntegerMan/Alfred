using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the home screen.
    /// </summary>
    [ViewModelFor(typeof(HomeScreenModel))]
    [UsedImplicitly]
    public sealed class HomeScreenViewModel : ScreenViewModel
    {
        /// <summary>
        ///     The model.
        /// </summary>
        [NotNull]
        private readonly HomeScreenModel _model;

        /// <summary>
        ///     Initializes a new instance of the BootupScreenViewModel class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public HomeScreenViewModel([NotNull] HomeScreenModel model) : base(model)
        {
            _model = model;
        }

        /// <summary>
        ///     Gets the version display string for the entry assembly.
        /// </summary>
        /// <value>
        ///     The version display string.
        /// </value>
        public string VersionString
        {
            get { return _model.VersionString; }
        }

        /// <summary>
        ///     Gets the application author string for display purposes.
        /// </summary>
        /// <value>
        ///     The author string.
        /// </value>
        public string AuthorString
        {
            get { return _model.AuthorString; }
        }

        /// <summary>
        ///     Gets the application name display string.
        /// </summary>
        /// <value>
        ///     The application name string.
        /// </value>
        public string ApplicationNameString
        {
            get { return _model.ApplicationNameString; }
        }

        /// <summary>
        ///     Gets the application copyright string.
        /// </summary>
        /// <value>
        ///     The application copyright string.
        /// </value>
        public string CopyrightString
        {
            get { return _model.CopyrightString; }
        }

        /// <summary>
        ///     Gets the faultIndicator indicators.
        /// </summary>
        /// <value>
        ///     The faultIndicator indicators.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<FaultIndicatorViewModel> FaultIndicators
        {
            get { return _model.FaultIndicators.Select((fault) => new FaultIndicatorViewModel(fault)); }
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other"> The view model to compare to this instance. </param>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        private bool Equals([NotNull] HomeScreenViewModel other)
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

            return obj is HomeScreenViewModel && Equals((HomeScreenViewModel)obj);
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