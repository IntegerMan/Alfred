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

            InitializeButtonCollections();

        }

        /// <summary>
        ///     Initializes the button collections.
        /// </summary>
        private void InitializeButtonCollections()
        {
            // Build navigation list.
            NavButtons = new List<ButtonModel>
                         {
                             MailButton,
                             CalendarButton,
                             NotesButton,
                             DocumentsButton,
                             BuildEmptyButton()
                         };
        }

        /// <summary>
        ///     Gets the default screen when this master mode is switched to.
        /// </summary>
        /// <value>
        ///     The default screen.
        /// </value>
        public override ScreenModel DefaultScreen { get { return MailButton.Target; } }

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

        /// <summary>
        ///     Gets the text to display on the screen identifying the current mdoe.
        /// </summary>
        /// <value>
        ///     The screen identification text.
        /// </value>
        public override string ScreenIdentificationText { get { return "ORG"; } }
    }
}