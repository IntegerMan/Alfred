namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     Contains modes for a multifunction display. Each mode has its own unique configuration
    ///     and capabilities.
    /// </summary>
    public enum MFDMode
    {
        /// <summary>
        ///     Represents the application bootup. This should transition to another mode when completed.
        /// </summary>
        Bootup,

        /// <summary>
        ///     Represents the default application mode.
        /// </summary>
        Default
    }
}