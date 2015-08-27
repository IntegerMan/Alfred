// ---------------------------------------------------------
// DelegateObjectProvider.cs
// 
// Created on:      08/27/2015 at 6:19 PM
// Last Modified:   08/27/2015 at 7:11 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Linq;

using JetBrains.Annotations;

namespace MattEland.Common.Providers
{
    /// <summary>
    ///     An <see cref="IObjectProvider" /> capable of creating an object
    /// </summary>
    [PublicAPI]
    public class DelegateObjectProvider : IObjectProvider
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="DelegateObjectProvider" /> class.
        /// </summary>
        /// <param name="activationDelegate">The activation delegate.</param>
        /// <param name="args">The arguments to pass to the delegate.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="activationDelegate" /> is
        ///     <see langword="null" />.
        /// </exception>
        public DelegateObjectProvider(
            [NotNull] Delegate activationDelegate,
            [CanBeNull] params object[] args)
        {
            //- Validate
            if (activationDelegate == null)
            {
                throw new ArgumentNullException(nameof(activationDelegate));
            }

            ActivationDelegate = activationDelegate;
            ActivationArguments = args;
        }

        /// <summary>
        ///     The activation arguments to pass to <see cref="ActivationDelegate" /> when
        ///     <see cref="CreateInstance" /> is called.
        /// </summary>
        public object[] ActivationArguments { get; }

        /// <summary>
        ///     Gets the activation delegate that will be invoked when a new instance is required.
        /// </summary>
        /// <value>The activation delegate.</value>
        [NotNull]
        public Delegate ActivationDelegate { get; }

        /// <summary>
        ///     Creates an instance of the requested type.
        /// </summary>
        /// <param name="requestedType">The type that was requested.</param>
        /// <returns>A new instance of the requested type</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public object CreateInstance(Type requestedType)
        {
            var hasArgs = ActivationArguments == null || !ActivationArguments.Any();
            return hasArgs
                       ? ActivationDelegate.DynamicInvoke()
                       : ActivationDelegate.DynamicInvoke(new object[] { ActivationArguments });
        }
    }
}