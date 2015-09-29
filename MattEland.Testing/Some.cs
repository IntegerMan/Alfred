using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattEland.Testing
{
    /// <summary>
    /// A helper class intended to help make tests more meaningful by hiding unimportant values.
    /// </summary>
    public static class Some
    {

        /// <summary>
        /// Gets unimportant text.
        /// </summary>
        /// <value>The text.</value>
        [NotNull]
        public static string Text
        {
            get { return "Unimportant text"; }
        }

        /// <summary>
        /// Gets an unimportant number.
        /// </summary>
        /// <value>The number.</value>
        public static double Number
        {
            get { return 42.0; }
        }
    }

}
