using System;

using JetBrains.Annotations;

namespace MattEland.Common
{
    /// <summary>
    ///     Interface for exception callback handlers.
    /// </summary>
    [PublicAPI]
    public interface IExceptionCallbackHandler
    {
        /// <summary>
        ///     Handles a callback <see cref="Exception"/>.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <param name="operationName"> Name of the operation that was being performed. </param>
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        bool HandleCallbackException(
            [NotNull] Exception exception,
            [NotNull] string operationName);
    }
}