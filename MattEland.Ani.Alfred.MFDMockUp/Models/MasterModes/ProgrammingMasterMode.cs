using System.Collections.Generic;
using System.Diagnostics;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes
{
    /// <summary>
    ///     A master mode related to programming. This class cannot be inherited.
    /// </summary>
    public sealed class ProgrammingMasterMode : MasterModeBase
    {
        /// <summary>
        ///     The context button.
        /// </summary>
        [NotNull]
        private readonly NavigationButtonModel _contextButton;

        /// <summary>
        ///     The clipboard button.
        /// </summary>
        [NotNull]
        private readonly NavigationButtonModel _clipboardButton;

        /// <summary>
        ///     The messages button.
        /// </summary>
        [NotNull]
        private readonly NavigationButtonModel _messagesButton;

        /// <summary>
        ///     The commands button.
        /// </summary>
        [NotNull]
        private readonly NavigationButtonModel _commandsButton;

        /// <summary>
        ///     Gets the context screen button.
        /// </summary>
        /// <value>
        ///     The context button.
        /// </value>
        [NotNull]
        public NavigationButtonModel ContextButton
        {
            [DebuggerStepThrough]
            get
            { return _contextButton; }
        }

        /// <summary>
        ///     Gets the clipboard screen button.
        /// </summary>
        /// <value>
        ///     The clipboard button.
        /// </value>
        [NotNull]
        public NavigationButtonModel ClipboardButton
        {
            [DebuggerStepThrough]
            get
            { return _clipboardButton; }
        }

        /// <summary>
        ///     Gets the message screen button.
        /// </summary>
        /// <value>
        ///     The messages button.
        /// </value>
        [NotNull]
        public NavigationButtonModel MessagesButton
        {
            [DebuggerStepThrough]
            get
            { return _messagesButton; }
        }

        /// <summary>
        ///     Gets the command execution screen button.
        /// </summary>
        /// <value>
        ///     The commands button.
        /// </value>
        [NotNull]
        public NavigationButtonModel CommandsButton
        {
            [DebuggerStepThrough]
            get
            { return _commandsButton; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProgrammingMasterMode"/> class.
        /// </summary>
        /// <param name="display"> The display. </param>
        public ProgrammingMasterMode([NotNull] MultifunctionDisplay display) : base(display)
        {

            var screens = ScreenProvider;

            // Set up buttons
            _contextButton = new NavigationButtonModel(screens.NotImplementedScreen, this, buttonText: "CTXT");
            _clipboardButton = new NavigationButtonModel(screens.NotImplementedScreen, this, buttonText: "CLIP");
            _messagesButton = new NavigationButtonModel(screens.NotImplementedScreen, this, buttonText: "MSGS");
            _commandsButton = new NavigationButtonModel(screens.NotImplementedScreen, this, buttonText: "CMDS");

        }

        /// <summary>
        ///     Gets the default screen when this master mode is switched to.
        /// </summary>
        /// <value>
        ///     The default screen.
        /// </value>
        public override ScreenModel DefaultScreen { get { return _contextButton.Target; } }

        /// <summary>
        ///     Gets screen change buttons.
        /// </summary>
        /// <returns>
        ///     An enumerable of screen change buttons.
        /// </returns>
        public override IEnumerable<ButtonModel> GetScreenChangeButtons()
        {
            yield return ContextButton;
            yield return CommandsButton;
            yield return ModeSwitchButton;
            yield return MessagesButton;
            yield return ClipboardButton;
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
    }
}