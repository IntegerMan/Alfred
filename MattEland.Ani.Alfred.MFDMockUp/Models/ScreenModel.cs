using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A model for a multifunction display screen.
    /// </summary>
    public abstract class ScreenModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected ScreenModel(string buttonText)
        {
            _buttonText = new Observable<string>(buttonText);
        }

        /// <summary>
        ///     The button text.
        /// </summary>
        [NotNull]
        private readonly Observable<string> _buttonText;

        /// <summary>
        ///     Gets or sets the abbreviated page's name for use in a navigation button.
        /// </summary>
        /// <value>
        ///     The button text.
        /// </value>
        [CanBeNull]
        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText.Value = value; }
        }
    }
}