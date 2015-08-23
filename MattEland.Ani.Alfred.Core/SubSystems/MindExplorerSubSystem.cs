// ---------------------------------------------------------
// MindExplorerSubSystem.cs
// 
// Created on:      08/22/2015 at 10:48 PM
// Last Modified:   08/22/2015 at 10:50 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.SubSystems
{
    /// <summary>
    ///     A SubSystem that pokes around at the internal state of each of Alfred's SubSystems and their
    ///     sub-components. This SubSystem has a particular focus towards anything AI-releted.
    /// </summary>
    public class MindExplorerSubSystem : AlfredSubsystem
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="console">The console.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public MindExplorerSubSystem([NotNull] IPlatformProvider provider,
                                     [CanBeNull] IConsole console = null) : base(provider, console)
        {
        }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        public override string Name
        {
            get { return "Mind Explorer"; }
        }

        /// <summary>
        ///     Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        public override string Id
        {
            get { return "Mind"; }
        }
    }
}