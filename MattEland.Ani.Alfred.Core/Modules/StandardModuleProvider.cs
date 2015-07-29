using System;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    /// A utility class for quickly configuring Alfred instances.
    /// </summary>
    public static class StandardModuleProvider
    {
        /// <summary>
        /// Adds standard modules to an Alfred Provider.
        /// </summary>
        /// <param name="alfred">The alfred provider.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddStandardModules([NotNull] AlfredProvider alfred)
        {
            if (alfred == null)
            {
                throw new ArgumentNullException(nameof(alfred));
            }

            alfred.AddModule(new AlfredTimeModule(alfred.CollectionProvider));
        }
    }
}