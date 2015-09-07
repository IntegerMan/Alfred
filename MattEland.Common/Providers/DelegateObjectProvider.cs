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
    ///     An <see cref="IObjectProvider" /> capable of creating an object.
    /// </summary>
    [PublicAPI]
    public sealed class DelegateObjectProvider : IObjectProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DelegateObjectProvider" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="activationDelegate" /> is
        ///     <see langword="null" />.
        /// </exception>
        /// <param name="activationDelegate"> The activation delegate. </param>
        public DelegateObjectProvider([NotNull] Delegate activationDelegate)
        {
            //- Validate
            if (activationDelegate == null)
            {
                throw new ArgumentNullException(nameof(activationDelegate));
            }

            ActivationDelegate = activationDelegate;
        }

        /// <summary>
        ///     Gets the activation <see langword="delegate"/> that will be invoked when a new instance
        ///     is required.
        /// </summary>
        /// <value>
        ///     The activation <see langword="delegate"/>.
        /// </value>
        [NotNull]
        public Delegate ActivationDelegate { get; }

        /// <summary>
        ///     Creates an instance of the requested <paramref name="requestedType"/>.
        /// </summary>
        /// <exception cref="Exception">
        ///     A delegate callback throws an exception.
        /// </exception>
        /// <param name="requestedType"> The <see cref="Type"/> that was requested. </param>
        /// <param name="args"> The arguments. </param>
        /// <returns>
        ///     The new instance.
        /// </returns>
        public object CreateInstance(Type requestedType, params object[] args)
        {
            var hasArgs = !args.Any();
            return hasArgs
                       ? ActivationDelegate.DynamicInvoke()
                       : ActivationDelegate.DynamicInvoke(new object[] { args });
        }
    }
}