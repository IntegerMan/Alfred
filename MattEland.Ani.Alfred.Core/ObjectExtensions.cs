// ---------------------------------------------------------
// ObjectExtensions.cs
// 
// Created on:      08/05/2015 at 3:22 PM
// Last Modified:   08/05/2015 at 3:24 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.IO;
using System.Reflection;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// Extension methods related to general objects
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Gets the Version of this module's assembly based on the AssemblyVersionAttribute.
        /// </summary>
        /// <param name="caller">The caller.</param>
        /// <returns>The Version of this module's assembly</returns>
        /// <exception cref="System.ArgumentNullException">caller</exception>
        [CanBeNull]
        public static Version GetAssemblyVersion([NotNull] this object caller)
        {
            if (caller == null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            try
            {
                var assembly = caller.GetType().Assembly;
                var assemblyName = new AssemblyName(assembly.FullName);
                return assemblyName.Version;
            }
            catch (IOException)
            {
                return null;
            }
        }
    }
}