using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MattEland.Ani.Alfred.PresentationCommon.Converters
{
    /// <summary>
    /// Contains the common logic used for VisibilityConverters between different presentation libraries.
    /// </summary>
    public abstract class VisibilityConverterBase
    {

        /// <summary>
        ///     Gets or sets a value indicating whether the visibility value is inverted so that
        ///     <see langword="false"/> is visible and <see langword="true"/> is not.
        /// </summary>
        /// <value>
        /// <c>true</c> if inverted; otherwise, <c>false</c> .
        /// </value>
        public bool Invert { get; set; }

        /// <summary>
        /// Determines if the specified <paramref name="value"/> should be visible.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if <paramref name="value"/> should be visible, <c>false</c> otherwise.</returns>
        public bool ShouldBeVisible(object value)
        {
            var result = false;

            if (value != null)
            {
                // Work with boolean values
                bool tryBool;
                if (bool.TryParse(value.ToString(), out tryBool))
                {
                    result = tryBool;
                }
            }

            // If we're inverting, flip around which output we'll push out
            if (Invert) { result = !result; }

            return result;
        }
    }
}
