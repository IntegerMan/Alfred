﻿using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.PresentationAvalon.Commands;

namespace MattEland.Ani.Alfred.VisualStudio
{

    /// <summary>
    /// Interaction logic for AlfredChatWindowControl.
    /// </summary>
    public partial class AlfredChatWindowControl
    {
        [NotNull]
        private readonly ApplicationManager _app;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredChatWindowControl"/> class.
        /// </summary>
        public AlfredChatWindowControl()
        {
            InitializeComponent();

            _app = AlfredPackage.AlfredInstance;

            // DataBindings rely on Alfred presently as there hasn't been a need for a page ViewModel yet
            DataContext = _app;

        }

    }
}