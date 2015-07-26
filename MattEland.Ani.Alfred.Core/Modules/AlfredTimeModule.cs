using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    /// A module that keeps track of time and date information and presents it to the user.
    /// </summary>
    public class AlfredTimeModule : AlfredModule
    {
        /// <summary>
        /// Gets the name and version of the module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public override string NameAndVersion => "Time 0.1 Alpha";
    }
}