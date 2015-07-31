using System.ComponentModel;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// A simple base implementation for classes that want to implement <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
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
        protected void OnPropertyChanged([CanBeNull] string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}