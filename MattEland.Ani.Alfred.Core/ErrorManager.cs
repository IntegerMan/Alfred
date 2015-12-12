using System;
using System.Collections.Generic;
using System.Linq;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Annotations;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     An error manager that categorizes errors into common problems and provides a way to
    ///     acknowledge errors. This class cannot be inherited.
    /// </summary>
    public sealed class ErrorManager : IHasContainer<IAlfredContainer>
    {
        /// <summary>
        ///     Initializes a new instance of the ErrorManager class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="container"/> is <lang keyword="null" />.
        /// </exception>
        /// <param name="container"> The container. </param>
        public ErrorManager([NotNull] IAlfredContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            Container = container;
        }

        private int _nextUnknownErrorCodeId = 1;

        [NotNull, ItemNotNull]
        private readonly IDictionary<string, ErrorCode> _errorCodes = new Dictionary<string, ErrorCode>();

        /// <summary>
        ///     Searches for the first error code for the specified error instance.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="instance"/> is <lang keyword="null" />.
        /// </exception>
        /// <param name="instance"> The error instance. </param>
        /// <returns>
        ///     The found error code for instance.
        /// </returns>
        [NotNull]
        private ErrorCode FindErrorCodeForInstance([NotNull] ErrorInstance instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            // TODO: Support a rules-based engine here

            // When we see a code that wants this error, return that code
            foreach (var code in ErrorCodes.Where(code => code.ShouldInclude(instance)))
            {
                return code;
            }

            // No match, build a new error code
            var codeId = "UNKN-" + _nextUnknownErrorCodeId++;
            var newCode = new ErrorCode(this.Container)
            {
                Identifier = codeId
            };

            // Add the new code to the collection and return it
            _errorCodes.Add(codeId, newCode);

            return newCode;
        }

        /// <summary>
        ///     Gets the error codes tracked by this categorizer.
        /// </summary>
        /// <value>
        ///     The error codes.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<ErrorCode> ErrorCodes
        {
            get { return _errorCodes.Values; }
        }

        /// <summary>
        ///     Registers the error instance and returns an <see cref="ErrorInstance"/> representing that
        ///     instance.
        /// </summary>
        /// <param name="ex"> The exception. </param>
        /// <returns>
        ///     An ErrorInstance.
        /// </returns>
        [NotNull]
        public ErrorInstance RegisterError(Exception ex)
        {
            return RegisterError(ex, DateTime.UtcNow);
        }

        /// <summary>
        ///     Registers the error instance and returns an <see cref="ErrorInstance"/> representing that
        ///     instance.
        /// </summary>
        /// <param name="ex"> The exception. </param>
        /// <param name="timeEncounteredUtc"> The time the error was encountered in UTC Date/Time. </param>
        /// <returns>
        ///     An ErrorInstance.
        /// </returns>
        [NotNull]
        private static ErrorInstance BuildErrorInstance(Exception ex, DateTime timeEncounteredUtc)
        {
            var instance = new ErrorInstance(ex, timeEncounteredUtc);
            return instance;
        }

        /// <summary>
        ///     Registers the error instance and returns an <see cref="ErrorInstance"/> representing that
        ///     instance.
        /// </summary>
        /// <param name="ex"> The exception. </param>
        /// <param name="timeEncounteredUtc"> The time the error was encountered in UTC Date/Time. </param>
        /// <returns>
        ///     An ErrorInstance.
        /// </returns>
        [NotNull]
        public ErrorInstance RegisterError(Exception ex, DateTime timeEncounteredUtc)
        {
            return RegisterError(ex, timeEncounteredUtc, null);
        }

        /// <summary>
        ///     Registers the error instance and returns an <see cref="ErrorInstance"/> representing that
        ///     instance.
        /// </summary>
        /// <param name="ex"> The exception. </param>
        /// <param name="timeEncounteredUtc"> The time the error was encountered in UTC Date/Time. </param>
        /// <param name="errorCodeId"> Identifier for the error code. </param>
        /// <returns>
        ///     An ErrorInstance.
        /// </returns>
        [NotNull]
        public ErrorInstance RegisterError(Exception ex, DateTime timeEncounteredUtc, string errorCodeId)
        {
            var instance = BuildErrorInstance(ex, timeEncounteredUtc);

            ErrorCode code = null;

            if (!string.IsNullOrWhiteSpace(errorCodeId))
            {
                if (_errorCodes.ContainsKey(errorCodeId.ToUpperInvariant()))
                {
                    code = _errorCodes[errorCodeId];
                }
            }

            if (code == null)
            {
                code = FindErrorCodeForInstance(instance);
            }

            // Add the error to its error code
            code.AddError(instance);

            return instance;
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IAlfredContainer Container { get; }
    }
}