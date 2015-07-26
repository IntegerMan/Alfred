namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// An enumeration containing various statuses of the Alfred system
    /// </summary>
    public enum AlfredStatus
    {
        /// <summary>
        /// Indicates Alfred is offline
        /// </summary>
        Offline,

        /// <summary>
        /// Indicates Alfred is currently initializing
        /// </summary>
        Initializing,

        /// <summary>
        /// Indicating Alfred is currently online
        /// </summary>
        Online,

        /// <summary>
        /// Indicates Alfred is shutting down
        /// </summary>
        ShuttingDown
    }
}