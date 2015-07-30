using System.ComponentModel;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    /// Any sort of user interface widget for representing a module. 
    /// Widgets can range from simple text to interactive components. 
    /// Widgets do not contain user interface elements but tell the
    /// client what user interface elements to create.
    /// </summary>
    public abstract class AlfredWidget : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CanBeNull] string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}