using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// Represents a subsystem on the Alfred Framework. Subsystems are ways of providing multiple related modules and capabilities to Alfred.
    /// </summary>
    public abstract class AlfredSubSystem : NotifyPropertyChangedBase
    {

        /// <summary>
        /// Gets the name of the subsystems.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public abstract string Name { get; }

    }
}