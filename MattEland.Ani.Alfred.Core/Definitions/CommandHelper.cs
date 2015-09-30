using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    /// A helper class used to construct commands
    /// </summary>
    public static class CommandHelper
    {
        /// <summary>
        /// Creates an <see cref="AlfredCommand" /> with the specified <see cref="Action" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="action">The <see cref="Action" /> to take when the command is executed.</param>
        /// <returns>The new command.</returns>
        [NotNull]
        public static IAlfredCommand CreateCommand([NotNull] IObjectContainer container, [CanBeNull] Action action)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            var command = container.Provide<IAlfredCommand>(action, container);

            return command;
        }

    }
}

