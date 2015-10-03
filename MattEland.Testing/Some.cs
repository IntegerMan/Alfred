using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Testing
{
    /// <summary>
    /// A helper class intended to help make tests more meaningful by hiding unimportant values.
    /// </summary>
    public sealed partial class Some
    {

        [NotNull]
        private readonly Random _rand;

        /// <summary>
        /// Initializes a new instance of the <see cref="Some"/> class.
        /// </summary>
        public Some() : this(new Random())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Some"/> class.
        /// </summary>
        /// <param name="randomizer">The randomizer.</param>
        public Some(Random randomizer)
        {
            _rand = randomizer ?? new Random();

            // Prepare the name collections
            _firstNames = GetFirstNames();
            _lastNames = GetLastNames();
        }


        /// <summary>
        /// Gets an unimportant number.
        /// </summary>
        /// <value>The number.</value>
        public double Number
        {
            get { return _rand.NextDouble() * 100; }
        }

        /// <summary>
        /// Gets some sample search text.
        /// </summary>
        /// <remarks>
        /// This is functionally equivalent to <see cref="Text"/> but provides a bit more flavor.
        /// </remarks>
        /// <value>The search text.</value>
        [NotNull]
        public string SearchText
        {
            get
            {
                return "Where can I find a good electronic butler?";
            }
        }

        /// <summary>
        /// Gets unimportant text.
        /// </summary>
        /// <value>The text.</value>
        [NotNull]
        public string Text
        {
            get { return "Unimportant text"; }
        }

        /// <summary>
        /// Gets unimportant URL in string form.
        /// </summary>
        /// <value>The URL.</value>
        [NotNull]
        public string Url
        {
            get { return "http://www.matteland.com/"; }
        }

    }

}
