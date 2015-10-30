using System.Diagnostics.Contracts;

using Assisticant.Fields;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the Alfred screen.
    /// </summary>
    [ViewModelFor(typeof(AlfredScreenModel))]
    [UsedImplicitly]
    public sealed class AlfredScreenViewModel
    {
        /// <summary>
        ///     The model.
        /// </summary>
        [NotNull]
        private readonly AlfredScreenModel _model;

        [NotNull]
        private readonly Computed<string> _statusMessage;

        /// <summary>
        ///     Initializes a new instance of the AlfredScreenViewModel class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public AlfredScreenViewModel([NotNull] AlfredScreenModel model)
        {
            _model = model;

            _statusMessage = new Computed<string>(
                    () => string.Format("Current Status: {0}", _model.StatusText)
        );
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other"> The view model to compare to this instance. </param>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        private bool Equals([NotNull] AlfredScreenViewModel other)
        {
            Contract.Requires(other != null);

            return _model.Equals(other._model);
        }

        /// <summary>
        ///     Gets a message describing the status of the Alfred system.
        /// </summary>
        /// <value>
        ///     A message describing the Alfred system status.
        /// </value>
        [NotNull]
        public string StatusMessage
        {
            get { return _statusMessage; }
        }

        /// <summary>
        ///     Gets the status text.
        /// </summary>
        /// <value>
        ///     The status text.
        /// </value>
        [NotNull]
        public string StatusText
        {
            get { return _model.StatusText; }
        }

        /// <summary>
        ///     Gets a value indicating whether Alfred is online.
        /// </summary>
        /// <value>
        ///     true if Alfred is online, false if not.
        /// </value>
        public bool IsOnline
        {
            get { return _model.IsOnline; }
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

            return obj is AlfredScreenViewModel && Equals((AlfredScreenViewModel)obj);
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