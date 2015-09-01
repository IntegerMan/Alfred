namespace MattEland.Ani.Alfred.PresentationShared.Controls
{
    /// <summary>
    /// Represents a user interface component that can be tested via Unit tests to some degree
    /// </summary>
    public interface IUserInterfaceTestable
    {
        /// <summary>
        ///     Simulates the control's loaded event.
        /// </summary>
        void SimulateLoadedEvent();
    }
}