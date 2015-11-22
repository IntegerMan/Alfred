using System.Collections.Generic;
using System.Diagnostics;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes
{
    /// <summary>
    ///     An organizer master mode.
    /// </summary>
    public class OrganizerMasterMode : MasterModeBase
    {
        [NotNull]
        private readonly NavigationButtonModel _calendarButton;
        [NotNull]
        private readonly NavigationButtonModel _documentsButton;
        [NotNull]
        private readonly NavigationButtonModel _mailButton;
        [NotNull]
        private readonly NavigationButtonModel _notesButton;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="display"> The display. </param>
        public OrganizerMasterMode([NotNull] MultifunctionDisplay display) : base(display)
        {
            var screens = ScreenProvider;

            // Set up buttons
            _calendarButton = new NavigationButtonModel(screens.NotImplementedScreen, this, buttonText: "CAL");
            _mailButton = new NavigationButtonModel(screens.NotImplementedScreen, this, buttonText: "MAIL");
            _notesButton = new NavigationButtonModel(screens.NotImplementedScreen, this, buttonText: "NOTE");
            _documentsButton = new NavigationButtonModel(screens.NotImplementedScreen, this, buttonText: "DOCS");
        }

        /// <summary>
        ///     Gets the default screen when this master mode is switched to.
        /// </summary>
        /// <value>
        ///     The default screen.
        /// </value>
        public override ScreenModel DefaultScreen { get { return MailButton.Target; } }

        /// <summary>
        ///     Gets screen change buttons.
        /// </summary>
        /// <returns>
        ///     An enumerable of screen change buttons.
        /// </returns>
        public override IEnumerable<ButtonModel> GetScreenChangeButtons()
        {
            yield return MailButton;
            yield return CalendarButton;
            yield return ModeSwitchButton;
            yield return NotesButton;
            yield return DocumentsButton;
        }

        /// <summary>
        ///     Gets the screen command buttons related to the current screen.
        /// </summary>
        /// <returns>
        /// An enumerable of screen command buttons.
        /// </returns>
        public override IEnumerable<ButtonModel> GetScreenCommandButtons()
        {
            yield break;
        }

        /// <summary>
        ///     Gets the calendar button.
        /// </summary>
        /// <value>
        ///     The calendar button.
        /// </value>
        [NotNull]
        public NavigationButtonModel CalendarButton
        {
            [DebuggerStepThrough]
            get
            { return _calendarButton; }
        }

        /// <summary>
        ///     Gets the documents button.
        /// </summary>
        /// <value>
        ///     The documents button.
        /// </value>
        [NotNull]
        public NavigationButtonModel DocumentsButton
        {
            [DebuggerStepThrough]
            get
            { return _documentsButton; }
        }

        /// <summary>
        ///     Gets the mail button.
        /// </summary>
        /// <value>
        ///     The mail button.
        /// </value>
        [NotNull]
        public NavigationButtonModel MailButton
        {
            [DebuggerStepThrough]
            get
            { return _mailButton; }
        }

        /// <summary>
        ///     Gets the notes button.
        /// </summary>
        /// <value>
        ///     The notes button.
        /// </value>
        [NotNull]
        public NavigationButtonModel NotesButton
        {
            [DebuggerStepThrough]
            get
            { return _notesButton; }
        }
    }
}