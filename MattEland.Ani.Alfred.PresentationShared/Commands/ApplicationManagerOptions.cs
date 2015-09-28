using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.PresentationAvalon.Commands
{
    /// <summary>
    ///     Options to pass in when instantiating an <see cref="ApplicationManager"/>.
    /// </summary>
    public sealed class ApplicationManagerOptions
    {
        /// <summary>
        ///     Gets or sets a value indicating whether speech is enabled.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if a speech is enabled, <see langword="false"/> if not.
        /// </value>
        public bool IsSpeechEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the mind explorer page is shown.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if show mind explorer page, <see langword="false"/> if not.
        /// </value>
        public bool ShowMindExplorerPage { get; set; } = true;

        /// <summary>
        ///     Gets any additional subsystems to load into Alfred at launch time.
        /// </summary>
        /// <value>
        ///     The additional subsystems.
        /// </value>
        [NotNull, ItemNotNull]
        public ICollection<IAlfredSubsystem> AdditionalSubsystems { get; } = new List<IAlfredSubsystem>();

        /// <summary>
        ///     Gets the bing API key.
        /// </summary>
        /// <value>
        ///     The bing API key.
        /// </value>
        public string BingApiKey { get; set; } = "YourBingApiKeyGoesHere";
    }
}