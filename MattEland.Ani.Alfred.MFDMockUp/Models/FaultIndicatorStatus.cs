namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     An enum representing the status of a fault indicator.
    /// </summary>
    public enum FaultIndicatorStatus
    {
        /// <summary>
        ///     Represents that an item is completely offline.
        /// </summary>
        Inactive,

        /// <summary>
        ///     Represents a system that is offline and should report that status to the user.
        /// </summary>
        DisplayOffline,

        /// <summary>
        ///     Represents a system that is offline but available.
        /// </summary>
        Available,

        /// <summary>
        ///     Represents a caution that should be paid attention to but does not require immediate 
        ///     attention.
        /// </summary>
        Warning,

        /// <summary>
        ///     Represents a serious failure requiring immediate attention.
        /// </summary>
        Fault,

        /// <summary>
        ///     Represents that a system is online and functioning properly.
        /// </summary>
        Online
    }
}
