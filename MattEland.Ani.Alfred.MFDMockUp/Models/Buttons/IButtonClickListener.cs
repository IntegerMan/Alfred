using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    public interface IButtonClickListener
    {
        /// <summary>
        ///     Executes when a button is clicked.
        /// </summary>
        /// <param name="button"> The button. </param>
        void OnButtonClicked([NotNull] ButtonModel button);
    }
}