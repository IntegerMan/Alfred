using System.Collections.Generic;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Subsystems
{
    /// <summary>
    ///     A simple subsystem that provides pages to Alfred. This is intended to be overridden.
    /// </summary>
    public class SimpleSubsystem : AlfredSubsystem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="name"> The name of the subsystem. </param>
        /// <param name="id"> The identifier for the subsystem. </param>
        public SimpleSubsystem([NotNull] IAlfredContainer container, string name, string id = null) : base(container)
        {
            Name = name;

            // ReSharper disable once AssignNullToNotNullAttribute
            Id = id.IsEmpty() ? name : id;

            PagesToRegister = container.ProvideCollection<IPage>();
        }

        /// <summary>
        ///     Gets the pages to register on startup.
        /// </summary>
        /// <value>
        ///     The pages to register on startup.
        /// </value>
        [NotNull, ItemNotNull]
        public ICollection<IPage> PagesToRegister
        {
            get;
        }

        /// <summary>
        ///     Gets the name of the subsystem.
        /// </summary>
        /// <value>The name of the subsystem.</value>
        public override string Name { get; }

        /// <summary>
        ///     Registers all pages in <see cref="PagesToRegister"/>.
        /// </summary>
        protected override void RegisterControls()
        {
            base.RegisterControls();

            foreach (var page in PagesToRegister) { Register(page); }
        }

        /// <summary>
        ///     Gets the identifier for the <see cref="IAlfredSubsystem"/> to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        public override string Id { get; }
    }
}