// ---------------------------------------------------------
// ButtonStripDock.cs
// 
// Created on:      10/18/2015 at 3:20 PM
// Last Modified:   10/18/2015 at 3:20 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    /// <summary>
    /// Specifies the docking position of a button strip outside of a multifunction display.
    /// </summary>
    public enum ButtonStripDock
    {
        /// <summary>
        ///     Buttons on the left side of a display
        /// </summary>
        Left,
        /// <summary>
        ///     Buttons on the top side of a display
        /// </summary>
        Top,
        /// <summary>
        ///     Buttons on the right side of a display
        /// </summary>
        Right,
        /// <summary>
        ///     Buttons on the bottom side of a display
        /// </summary>
        Bottom
    }

}