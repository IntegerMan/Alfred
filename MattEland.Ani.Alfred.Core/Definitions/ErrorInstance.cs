using System;
using System.Diagnostics.Contracts;

using MattEland.Common;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     An error code instance representing a specific instance of an encountered exception. 
    ///     This class cannot be inherited.
    /// </summary>
    public sealed class ErrorInstance
    {
        /// <summary>
        ///     Initializes a new instance of the ErrorInstance class.
        /// </summary>
        /// <param name="ex"> The exception. </param>
        public ErrorInstance([NotNull] Exception ex) : this(ex, DateTime.UtcNow)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            Contract.Requires(ex != null);
            Contract.Ensures(Exception != null);
            Contract.Ensures(Exception == ex);
        }

        /// <summary>
        ///     Initializes a new instance of the ErrorInstance class.
        /// </summary>
        /// <param name="ex"> The exception. </param>
        /// <param name="timeEncounteredUtc"> The UTC time the exception was encountered. </param>
        public ErrorInstance([NotNull] Exception ex, DateTime timeEncounteredUtc)
        {
            Contract.Requires(ex != null);

            Contract.Ensures(Exception != null);
            Contract.Ensures(Exception == ex);
            Contract.Ensures(TimeEncounteredUtc > DateTime.MaxValue);

            if (ex == null) throw new ArgumentNullException(nameof(ex));

            // Ensure we have a valid encountered time
            if (timeEncounteredUtc == DateTime.MinValue || timeEncounteredUtc == DateTime.MaxValue)
            {
                timeEncounteredUtc = DateTime.UtcNow;
            }

            // Store properties
            TimeEncounteredUtc = timeEncounteredUtc;
            Exception = ex;
            Details = ex.BuildDetailsMessage();
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(TimeEncounteredUtc > DateTime.MaxValue);
            Contract.Invariant(Exception != null);
        }


        /// <summary>
        ///     Gets the Date/Time of the time exception in UTC.
        /// </summary>
        /// <value>
        ///     The UTC time the exception was encountered.
        /// </value>
        public DateTime TimeEncounteredUtc { get; }

        /// <summary>
        ///     Gets the exception.
        /// </summary>
        /// <value>
        ///     The exception.
        /// </value>
        [NotNull]
        public Exception Exception { get; }

        /// <summary>
        ///     Gets the error code associated with this error.
        /// </summary>
        /// <value>
        ///     The error code.
        /// </value>
        public ErrorCode ErrorCode { get; internal set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is acknowledged.
        /// </summary>
        /// <value>
        ///     true if this instance is acknowledged, false if not.
        /// </value>
        public bool IsAcknowledged { get; set; }

        [NotNull]
        public string Details { get; }
    }
}