// ---------------------------------------------------------
// IHasIdentifier.cs
// 
// Created on:      08/20/2015 at 11:56 PM
// Last Modified:   08/20/2015 at 11:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Represents an item that has an identifier. This is used for lookups and unique component
    ///     identification.
    /// </summary>
    public interface IHasIdentifier
    {
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Id { get; }
    }
}