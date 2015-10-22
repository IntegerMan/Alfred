using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A data Model for the bootup screen.
    /// </summary>
    public sealed class BootupScreenModel : ScreenModel
    {

        /// <summary>
        ///     The loading progress.
        /// </summary>
        [NotNull]
        private readonly Observable<double> _progress;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public BootupScreenModel() : base("BTUP")
        {
            _progress = new Observable<double>(0.0);
        }

        /// <summary>
        ///     Gets or sets the progress of the loading operation as a value from 0.0 to 1.0.
        /// </summary>
        /// <value>
        ///     The progress.
        /// </value>
        public double Progress
        {
            get { return _progress; }
            set { _progress.Value = value; }
        }
    }
}