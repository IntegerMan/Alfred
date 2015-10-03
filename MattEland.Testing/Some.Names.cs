using System;
using System.Linq;
using System.Text;
using MattEland.Common;
using JetBrains.Annotations;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MattEland.Testing
{
    public partial class Some
    {

        /// <summary>
        ///     List of first names
        /// </summary>
        [NotNull]
        private readonly IList<string> _firstNames;

        /// <summary>
        ///     List of last names.
        /// </summary>
        [NotNull]
        private readonly IList<string> _lastNames;

        /// <summary>
        ///     Gets a random full name. Values will be generated randomly each time this property is called.
        /// </summary>
        /// <value>
        ///     A new random full name.
        /// </value>
        [NotNull]
        public string FullName
        {
            get
            {
                var first = FirstName;
                var last = LastName;

                return string.Format("{0} {1}", first, last);
            }
        }

        /// <summary>
        ///     Gets a random first name. Values will be generated randomly each time this property is called.
        /// </summary>
        /// <value>
        ///     A new random first name.
        /// </value>
        [NotNull]
        public string FirstName
        {
            get
            {
                return _firstNames.GetRandomItem(_rand);
            }
        }

        /// <summary>
        ///     Gets a random last name. Values will be generated randomly each time this property is called.
        /// </summary>
        /// <value>
        ///     A new random last name.
        /// </value>
        [NotNull]
        public string LastName
        {
            get
            {
                return _lastNames.GetRandomItem(_rand);
            }
        }

        /// <summary>
        ///     Gets a list of first names.
        /// </summary>
        /// <returns>
        ///     The first names.
        /// </returns>
        [NotNull, ItemNotNull]
        private static IList<string> GetFirstNames()
        {
            return new List<string>() { "Bob",
                                          "Susie",
                                          "Calvin",
                                          "Lynne",
                                          "Holly",
                                          "Daisy",
                                          "Hannah",
                                          "Tawanda",
                                          "Matt",
                                          "Heather",
                                          "Billy",
                                          "Bill",
                                          "Bruce",
                                          "Marla",
                                          "Dan",
                                          "Wes",
                                          "Stephanie",
                                          "Doug",
                                          "Alex",
                                          "Adam",
                                          "Brandon",
                                          "Braydan",
                                          "Dylan",
                                          "Chuck",
                                          "Charley",
                                          "Jimbo",
                                          "Bubba",
                                          "Al",
                                          "Earl",
                                          "Ryan",
                                          "Randy",
                                          "Maddie",
                                          "Liz",
                                          "Jen",
                                          "Jan",
                                          "Michelle",
                                          "Jessica",
                                          "Justin",
                                          "Kyle",
                                          "Kim",
                                          "Andrew",
                                          "Andy",
                                          "Snake",
                                          "Spike",
                                          "Cuftbert",
                                          "Killer",
                                          "Debbie",
                                          "Katie",
                                          "Payton",
                                          "Owen",
                                          "Parker",
                                          "Chris"
                                          };
        }

        /// <summary>
        ///     Gets a list of last names.
        /// </summary>
        /// <returns>
        ///     The last names.
        /// </returns>
        [NotNull, ItemNotNull]
        private static IList<string> GetLastNames()
        {
            return new List<string>() { "Wayne",
                                         "Eland",
                                         "Dole",
                                         "Jennings",
                                         "Banner",
                                         "Hogan",
                                         "Arnolds",
                                         "Edmunds",
                                         "Gates",
                                         "Miller",
                                         "Pliskin",
                                         "Royce",
                                         "Cuftbert",
                                         "Sharp",
                                         "Patch",
                                         "Harris",
                                         "Torres"
                                         };
        }

    }
}
