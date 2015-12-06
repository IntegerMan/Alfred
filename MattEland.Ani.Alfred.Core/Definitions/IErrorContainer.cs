using System;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     An interface for a class that exposes the last encountered error in a property and allows
    ///     for the error to be acknowledged and cleared out.
    /// </summary>
    public interface IErrorContainer
    {
        /// <summary>
        ///     Gets or sets the last error encountered by this component.
        /// </summary>
        /// <value>
        ///     The last error encountered.
        /// </value>
        Exception LastError { get; }

        /// <summary>
        ///     Gets a value indicating whether or not an unacknowledged error has been encountered.
        /// </summary>
        /// <value>
        ///     true if this instance has an unacknowledge error, false if not.
        /// </value>
        bool HasError { get; }

        /// <summary>
        ///     Acknowledges the <see cref="LastError"/> error and clears out the exception.
        /// </summary>
        void AcknowledgeError();
    }
}