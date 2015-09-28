using MattEland.Ani.Alfred.PresentationCommon.Layout;
using System.Windows;

namespace MattEland.Ani.Alfred.PresentationAvalon.Layout
{
    /// <summary>
    /// A static class containing Windows Universal specific extension methods
    /// </summary>
    public static class UniversalExtensions
    {
        /// <summary>
        /// Transforms <paramref name="size"/> to a <see cref="Size"/>.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>The rectangle.</returns>
        public static Rect ToRect(this LayoutSize size)
        {
            return new Rect(size.X, size.Y, size.Width, size.Height);
        }

        /// <summary>
        /// Transforms <paramref name="size"/> to a <see cref="Size"/>.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>The rectangle.</returns>
        public static Size ToSize(this LayoutSize size)
        {
            return new Size(size.Width, size.Height);
        }

        /// <summary>
        /// Transforms <paramref name="rect"/> to a <see cref="LayoutSize"/>.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns>The layout size.</returns>
        public static LayoutSize ToSize(this Rect rect)
        {
            return new LayoutSize(rect.X, rect.Y, rect.Width, rect.Height);
        }


        /// <summary>
        /// Transforms <paramref name="size"/> to a <see cref="LayoutSize"/>.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>The layout size.</returns>
        public static LayoutSize ToLayoutSize(this Size size)
        {
            return new LayoutSize(size.Width, size.Height);
        }

    }
}

