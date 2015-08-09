namespace MattEland.Ani.Alfred.Core.Definitions
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
        /// Indicating Alfred is currently online
        /// </summary>
        Online,

        /// <summary>
        /// Indicates Alfred is currently transitioning to Online mode
        /// </summary>
        Initializing,

        /// <summary>
        /// Indicates Alfred is currently transitioning to Offline mode
        /// </summary>
        Terminating
    }
}