using System;

using MattEland.Common.Annotations;

using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    /// Defines a property within the Alfred application
    /// </summary>
    public class AlfredProperty : IPropertyItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredProperty" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="isEditable">The is editable.</param>
        public AlfredProperty([NotNull] string name,
                              [CanBeNull] object value,
                              bool isEditable = false)
        {
            // Validate
            if (name.IsEmpty())
            {
                throw new ArgumentNullException(nameof(name));
            }

            _value = value;
            DisplayName = name;
            IsEditable = isEditable;
        }

        /// <summary>
        /// Gets the display name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        [NotNull]
        public string DisplayName { get; }

        /// <summary>
        /// Gets a value indicating whether this instance should be editable in a property grid.
        /// </summary>
        /// <value><c>true</c> if this instance is editable; otherwise, <c>false</c>.</value>
        public bool IsEditable { get; }

        [CanBeNull]
        private object _value;

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        /// <value>The value.</value>
        [CanBeNull]
        public object Value
        {
            get { return _value; }
            set
            {
                if (!IsEditable)
                {
                    throw new InvalidOperationException("Cannot set Value when IsEditable is false");
                }

                _value = value;
            }
        }

        /// <summary>
        /// Gets the formatted value of <see cref="IPropertyItem.Value"/>.
        /// In cases where the Value is null, a descriptive value of no item should be present
        /// </summary>
        /// <value>The formatted value.</value>
        public string DisplayValue
        {
            get
            {
                var value = Value;

                // Provide a displayable value for nulls
                if (value == null)
                {
                    return "[Not Set]";
                }

                // For everything else, just rely on ToString()
                return value.ToString().NonNull();
            }
        }
    }
}