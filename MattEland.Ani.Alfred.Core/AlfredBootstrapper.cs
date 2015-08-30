// ---------------------------------------------------------
// AlfredBootstrapper.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/29/2015 at 4:26 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     A utility class used for instantiating the Alfred Framework.
    /// </summary>
    public sealed class AlfredBootstrapper
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredBootstrapper" /> class.
        /// </summary>
        public AlfredBootstrapper() : this(CommonProvider.Container)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredBootstrapper" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="console"> The console. </param>
        public AlfredBootstrapper(
            [NotNull] IObjectContainer container,
            [CanBeNull] IConsole console = null)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }
            Container = container;
            Console = console;
        }


        /// <summary>
        ///     Gets or sets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IObjectContainer Container { get; set; }

        /// <summary>
        ///     Gets or sets the console.
        /// </summary>
        /// <value>
        ///     The console.
        /// </value>
        [CanBeNull]
        public IConsole Console { get; set; }

        /// <summary>
        ///     Creates a new instance of Alfred using this instance's properties and returns it.
        /// </summary>
        /// <returns>
        ///     The Alfred instance.
        /// </returns>
        [NotNull]
        public AlfredApplication Create()
        {
            var alfred = new AlfredApplication(Container) { Console = Console };

            return alfred;
        }
    }
}