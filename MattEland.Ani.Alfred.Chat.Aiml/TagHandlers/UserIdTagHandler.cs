// ---------------------------------------------------------
// UserIdTagHandler.cs
// 
// Created on:      08/12/2015 at 10:47 PM
// Last Modified:   08/14/2015 at 11:28 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler for the AIML "id" tag that outputs the user's identifier found in
    ///     <see cref="User.UniqueId"/>.
    /// </summary>
    [HandlesAimlTag("id"), UsedImplicitly]
    public sealed class UserIdTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public UserIdTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            return User.UniqueId.ToString();
        }
    }
}