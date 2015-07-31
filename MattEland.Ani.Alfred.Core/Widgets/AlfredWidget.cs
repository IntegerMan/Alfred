namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    /// Any sort of user interface widget for representing a module. 
    /// Widgets can range from simple text to interactive components. 
    /// Widgets do not contain user interface elements but tell the
    /// client what user interface elements to create.
    /// </summary>
    public abstract class AlfredWidget : NotifyPropertyChangedBase
    {

        private bool _isVisible = true;

        /// <summary>
        /// Gets or sets whether or not the widget is visible. This defaults to <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if this widget is visible; otherwise, <c>false</c>.</value>
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible)
                {
                    return;
                }
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
    }
}