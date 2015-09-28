using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Ani.Alfred.PresentationCommon.Layout
{
    /// <summary>
    /// A platform-independent size implementation.
    /// </summary>
    public struct LayoutSize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSize"/> struct.
        /// </summary>
        /// <param name="size">The size to base this on.</param>
        public LayoutSize(LayoutSize size) : this(size.X, size.Y, size.Width, size.Height)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSize"/> struct.
        /// </summary>
        /// <param name="x">The X location.</param>
        /// <param name="y">The Y location.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public LayoutSize(double x, double y, double width, double height)
        {
            Width = width;
            Height = height;

            X = x;
            Y = y;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSize"/> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public LayoutSize(double width, double height) : this(0, 0, width, height)
        {
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the X location.
        /// </summary>
        /// <value>The X location.</value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y location.
        /// </summary>
        /// <value>The Y location.</value>
        public double Y { get; set; }

    }
}

