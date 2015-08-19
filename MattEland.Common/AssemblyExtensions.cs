// ---------------------------------------------------------
// AssemblyExtensions.cs
// 
// Created on:      08/14/2015 at 12:38 AM
// Last Modified:   08/14/2015 at 12:59 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace MattEland.Common
{
    public static class AssemblyHelper
    {
        /// <summary>
        ///     Gets the types in an assembly with an attribute applied to them.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute to search for.</typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <param name="inherit">
        ///     true to search the type's inheritance chain to find the attributes; otherwise, false.
        /// </param>
        /// <returns>The types in the assembly with the requested attribute</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        [NotNull]
        [ItemNotNull]
        [UsedImplicitly]
        public static IEnumerable<Type> GetTypesInAssemblyWithAttribute<TAttribute>([NotNull] this Assembly assembly,
                                                                                    bool inherit)
            where TAttribute : Attribute
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var types = assembly.GetTypes();
            return GetTypesWithAttributes<TAttribute>(types, inherit);
        }

        /// <summary>
        ///     Gets the types in a group of types that have an attribute applied to them.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute to search for.</typeparam>
        /// <param name="types">The set of types</param>
        /// <param name="inherit">
        ///     true to search the type's inheritance chain to find the attributes; otherwise, false.
        /// </param>
        /// <returns>The types in the set of types that have the requested attribute</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        [NotNull]
        [ItemNotNull]
        [UsedImplicitly]
        public static IEnumerable<Type> GetTypesWithAttributes<TAttribute>([NotNull] IEnumerable<Type> types,
                                                                           bool inherit) where TAttribute : Attribute
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            return types.Where(t => t.HasAttribute<TAttribute>(inherit));
        }

        /// <summary>
        ///     Determines whether the specified member has a particular attribute applied to it.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="member">The member.</param>
        /// <param name="inherit">
        ///     true to search the type's inheritance chain to find the attributes; otherwise, false.
        /// </param>
        /// <returns><c>true</c> if the specified inherit has the attribute; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        [UsedImplicitly]
        public static bool HasAttribute<TAttribute>([NotNull] this MemberInfo member, bool inherit)
            where TAttribute : Attribute
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            return member.IsDefined(typeof(TAttribute), inherit);
        }

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