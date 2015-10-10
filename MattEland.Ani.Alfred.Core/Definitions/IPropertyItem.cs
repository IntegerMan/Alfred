using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    /// Describes a property provided by an <see cref="IPropertyProvider"/>
    /// </summary>
    public interface IPropertyItem
    {
        /// <summary>
        /// Gets the display name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        [NotNull]
        string DisplayName { get; }

        /// <summary>
        /// Gets a value indicating whether this instance should be editable in a property grid.
        /// </summary>
        /// <value><c>true</c> if this instance is editable; otherwise, <c>false</c>.</value>
        bool IsEditable { get; }

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        /// <value>The value.</value>
        [CanBeNull]
        object Value { get; set; }

        /// <summary>
        /// Gets the formatted value of <see cref="IPropertyItem.Value"/>.
        /// In cases where the Value is null, a descriptive value of no item should be present
        /// </summary>
        /// <value>The formatted value.</value>
        [NotNull]
        string DisplayValue { get; }
    }
}