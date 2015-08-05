using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class AlfredStringExtensions
    {
        /// <summary>
        /// Ensures that the passed in string is not null and returns either the input string or string.empty.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A sanitized string</returns>
        [NotNull]
        public static string NonNull([CanBeNull] this string input)
        {
            return input ?? string.Empty;
        }
    }
}