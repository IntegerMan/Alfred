using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     Interface for items that have a display associated with them.
    /// </summary>
    public interface IHasDisplay
    {
        /// <summary>
        ///     Gets or sets the display.
        /// </summary>
        /// <value>
        ///     The display.
        /// </value>
        [CanBeNull]
        MultifunctionDisplay Display { get; set; }
    }
}