using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// Gets unimportant URL in string form.
        /// </summary>
        /// <value>The URL.</value>
        [NotNull]
        public static string Url
        {
            get { return "http://www.matteland.com/"; }
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
