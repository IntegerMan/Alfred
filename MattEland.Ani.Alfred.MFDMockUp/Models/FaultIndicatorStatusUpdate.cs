using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A fault indicator status update result.
    /// </summary>
    public struct FaultIndicatorStatusUpdate
    {
        /// <summary>
        ///     Initializes a new instance of the FaultIndicatorStatusUpdate struct.
        /// </summary>
        /// <param name="status"> The status code. </param>
        public FaultIndicatorStatusUpdate(FaultIndicatorStatus status) : this(status, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the FaultIndicatorStatusUpdate struct.
        /// </summary>
        /// <param name="status"> The status code. </param>
        /// <param name="message"> The status message. </param>
        public FaultIndicatorStatusUpdate(FaultIndicatorStatus status, string message)
        {
            Message = message ?? string.Empty;
            Status = status;
        }

        #region Properties

        /// <summary>
        ///     Gets or sets the status code.
        /// </summary>
        /// <value>
        ///     The status code.
        /// </value>
        public FaultIndicatorStatus Status { get; }

        /// <summary>
        ///     Gets or sets the current status message.
        /// </summary>
        /// <value>
        ///     The status message.
        /// </value>
        [NotNull]
        public string Message { get; }

        #endregion

        #region Equality

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other"> The fault indicator status update to compare to this instance. </param>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        public bool Equals(FaultIndicatorStatusUpdate other)
        {
            return Status == other.Status && string.Equals(Message, other.Message);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is FaultIndicatorStatusUpdate && Equals((FaultIndicatorStatusUpdate)obj);
        }

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Status * 397) ^ Message.GetHashCode();
            }
        }

        #endregion

    }
}