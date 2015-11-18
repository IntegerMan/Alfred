using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    /// <summary>
    ///     A master mode. This represents the context for a multifunction display and governs the
    ///     available screens and button configurations.
    /// </summary>
    public sealed class MasterMode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MasterMode()
        {
        }

        /// <summary>
        ///     Gets screen change buttons.
        /// </summary>
        /// <param name="buttonProvider"> The button provider. </param>
        /// <param name="display"> The display. </param>
        /// <returns>
        ///     An enumerable of screen changebuttons.
        /// </returns>
        [NotNull, ItemNotNull]
        public IEnumerable<ButtonModel> GetScreenChangeButtons([NotNull] ButtonProvider buttonProvider,
            [NotNull] MultifunctionDisplay display)
        {
            Contract.Requires(buttonProvider != null);
            Contract.Requires(display != null);
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>() != null);
            Contract.Ensures(Contract.Result<IEnumerable<ButtonModel>>().All(b => b != null));

            return new List<ButtonModel>();
        }
    }
}