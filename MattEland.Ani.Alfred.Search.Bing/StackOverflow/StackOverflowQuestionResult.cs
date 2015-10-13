using System;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Annotations;
using MattEland.Common;
using StackExchange.StacMan;
using System.Diagnostics.Contracts;

namespace MattEland.Ani.Alfred.Search.StackOverflow
{
    /// <summary>
    ///     Represents a stack overflow question as a search result. This class cannot be inherited.
    /// </summary>
    public sealed class StackOverflowQuestionResult : AlfredSearchResult
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="StackOverflowQuestionResult"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="question"> The question. </param>
        internal StackOverflowQuestionResult([NotNull] IAlfredContainer container,
                    [NotNull] Question question)
            : base(container, question.Title)
        {
            //- Validation
            Contract.Requires(container != null);
            Contract.Requires(question != null);

            // Set misc. properties from the question object
            Description = question.Body;
            LocationText = question.Tags.BuildCommaDelimitedString();
            Url = question.Link;
            MoreDetailsText = "View Question";
        }
    }
}

