// ---------------------------------------------------------
// IAlfredModule.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:19 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Core.Modules;

namespace MattEland.Ani.Alfred.Core.Interfaces
{
    /// <summary>
    ///     Represents a module belonging to a page or subsystem in Alfred.
    /// </summary>
    /// <remarks>
    ///     TODO: This is a marker interface at present. I'd like to see some methods or reasons not to just use
    ///     IAlfredComponent
    /// </remarks>
    public interface IAlfredModule : IAlfredComponent
    {
    }
}