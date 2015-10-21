
namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Governs various layout types supported by presentation arrangement panels
    /// </summary>
    public enum LayoutType
    {
        /// <summary>
        ///     Represents a layout similar to StackPanel in Vertical orientation
        /// </summary>
        VerticalStackPanel,

        /// <summary>
        ///     Represents a layout similar to StackPanel in Horizontal orientation
        /// </summary>
        HorizontalStackPanel,

        /// <summary>
        ///     Represents a layout similar to WrapPanel in Vertical orientation
        /// </summary>
        VerticalWrapPanel,

        /// <summary>
        ///     Represents a layout similar to WrapPanel in Horizontal orientation
        /// </summary>
        HorizontalWrapPanel,

        /// <summary>
        ///     Represents a layout that automatically spaces out elements vertically.
        /// </summary>
        VerticalAutoSpacePanel,

        /// <summary>
        ///     Represents a layout that automatically spaces out elements horizontally.
        /// </summary>
        HorizontalAutoSpacePanel
    }
}