// ---------------------------------------------------------
// RedirectTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:10 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler for the AIML "srai" tag governing recursive / redirect style searches.
    /// </summary>
    [HandlesAimlTag(@"srai"), PublicAPI]
    public sealed class RedirectTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public RedirectTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters) { }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            //- Basic validation
            if (Contents.IsEmpty()) { return string.Empty; }

            // Spawn a new request as a child of this current request and execute it
            var request = new Request(Contents, User, ChatEngine, Request);
            var result = ChatEngine.ProcessRedirectChatRequest(request);
            Debug.Assert(result != null);

            // This could have taken awhile. Check it for timeout
            request.CheckForTimedOut();

            // Return the result of the inner request
            return result.Output;
        }
    }
}