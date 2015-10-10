// ---------------------------------------------------------
// LowercaseTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:07 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler responding to the AIML "lowercase" tag. This formats all text in the node in
    ///     lower case.
    /// </summary>
    [HandlesAimlTag("lowercase")]
    [UsedImplicitly]
    internal sealed class LowercaseTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public LowercaseTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters) { }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            if (NodeName.Matches("lowercase"))
            {
                // This is a very simple operation using the user's locale
                return Contents.ToLower(Locale);
            }

            return string.Empty;
        }
    }
}