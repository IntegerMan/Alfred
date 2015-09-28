using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Ani.Alfred.PresentationCommon.Layout
{
    /// <summary>
    /// A class containing cross-platform layout logic
    /// </summary>
    /// <typeparam name="TChild">The type of the child.</typeparam>
    public abstract class LayoutHelperBase<TChild>
    {
        /// <summary>
        /// Gets the child's desired size.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <returns>Desired size.</returns>
        protected abstract LayoutSize GetChildDesiredSize([NotNull] TChild child);

        /// <summary>
        /// Arranges the child at the given dimensions.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="childRect">The child dimensions.</param>
        protected abstract void ArrangeChild(TChild child, LayoutSize childRect);

        /// <summary>
        /// Measures the child.
        /// </summary>
        /// <param name="availableSize">The available size</param>
        /// <param name="child">The child.</param>
        protected abstract void MeasureChild(LayoutSize availableSize, TChild child);

    }
}

