// ---------------------------------------------------------
// AssemblyExtensions.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/23/2015 at 11:32 PM
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
    [PublicAPI]
    public static class AssemblyHelper
    {
        /// <summary>
        ///     Gets the types in an assembly with an attribute applied to them.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="attributeType">The type of the attribute</param>
        /// <param name="inherit">
        ///     true to search the type's inheritance chain to find the attributes; otherwise, false.
        /// </param>
        /// <returns>The types in the assembly with the requested attribute</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<Type> GetTypesInAssemblyWithAttribute(
            [NotNull] this Assembly assembly,
            [NotNull] Type attributeType,
            bool inherit)
        {
            //- Validate
            if (assembly == null) { throw new ArgumentNullException(nameof(assembly)); }
            if (attributeType == null) { throw new ArgumentNullException(nameof(attributeType)); }

            // Grab the types defined in the assembly
            var types = assembly.GetTypes();

            // Filter down to those with  attributes we want
            return GetTypesWithAttributes(types, attributeType, inherit);
        }

        /// <summary>
        ///     Gets the types in a group of types that have an attribute applied to them.
        /// </summary>
        /// <param name="types">The set of types</param>
        /// <param name="attributeType">The attribute type.</param>
        /// <param name="inherit">
        ///     true to search the type's inheritance chain to find the attributes; otherwise, false.
        /// </param>
        /// <returns>The types in the set of types that have the requested attribute</returns>
        /// <exception cref="System.ArgumentNullException">types, attributeType</exception>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<Type> GetTypesWithAttributes([NotNull] IEnumerable<Type> types,
                                                               [NotNull] Type attributeType,
                                                               bool inherit)
        {
            //- Validate
            if (types == null) { throw new ArgumentNullException(nameof(types)); }
            if (attributeType == null) { throw new ArgumentNullException(nameof(attributeType)); }

            return types.Where(t => t != null && t.HasAttribute(attributeType, inherit));
        }

        /// <summary>
        ///     Determines whether the specified member has a particular attribute applied to it.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="attributeType">The attribute type</param>
        /// <param name="inherit">
        ///     true to search the type's inheritance chain to find the attributes; otherwise, false.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified inherit has the attribute; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">member, attributeType</exception>
        [UsedImplicitly]
        public static bool HasAttribute([NotNull] this MemberInfo member,
                                        [NotNull] Type attributeType,
                                        bool inherit)
        {
            //- Validate
            if (member == null) { throw new ArgumentNullException(nameof(member)); }
            if (attributeType == null) { throw new ArgumentNullException(nameof(attributeType)); }

            return member.IsDefined(attributeType, inherit);
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
            if (caller == null) { throw new ArgumentNullException(nameof(caller)); }

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