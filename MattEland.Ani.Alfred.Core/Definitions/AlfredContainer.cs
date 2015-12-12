using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common.Providers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Diagnostics.Contracts;
namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     An Alfred-specific container implementation that provides access to the common Alfred
    ///     properties. This class cannot be inherited.
    /// </summary>
    public sealed class AlfredContainer : CommonContainer, IAlfredContainer
    {
        /// <summary>
        ///     The localization culture.
        /// </summary>
        [CanBeNull]
        private CultureInfo _locale;

        /// <summary>
        ///     The Alfred instance. Can be null when not lazy-loaded.
        /// </summary>
        [CanBeNull]
        private IAlfred _alfred;

        /// <summary>
        ///     The console.
        /// </summary>
        [CanBeNull]
        private IConsole _console;


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredContainer"/> class.
        /// </summary>
        public AlfredContainer() : this(null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredContainer"/> class.
        /// </summary>
        /// <param name="parent">The parent container.</param>
        public AlfredContainer(IObjectContainer parent) : base(parent)
        {
            this.ErrorManager = new ErrorManager(this);
        }


        #endregion

        /// <summary>
        /// Gets the Alfred instance.
        /// </summary>
        /// <value>
        /// The Alfred.
        /// </value>
        [NotNull]
        public IAlfred Alfred
        {
            get
            {
                // Lazy load
                if (_alfred == null)
                {
                    _alfred = Provide<IAlfred>();
                }

                return _alfred;
            }
            set
            {
                // Replace the cached value. Null is an acceptable way to clear cache.
                _alfred = value;

                // This is now the de-facto Alfred instance
                if (value != null)
                {
                    value.RegisterAsProvidedInstance(typeof(IAlfred), this);
                }
            }
        }

        /// <summary>
        ///     Gets the shell.
        /// </summary>
        /// <value>
        ///     The shell.
        /// </value>
        [NotNull]
        public IShellCommandRecipient Shell
        {
            get
            {
                return Alfred.ShellCommandHandler;
            }
        }

        /// <summary>
        ///     Gets the chat provider.
        /// </summary>
        /// <value>
        ///     The chat provider.
        /// </value>
        [CanBeNull]
        public IChatProvider ChatProvider
        {
            get
            {
                return Alfred.ChatProvider;
            }
        }

        /// <summary>
        ///     Gets or sets the console.
        /// </summary>
        /// <value>
        ///     The console.
        /// </value>
        [CanBeNull]
        public IConsole Console
        {
            get
            {
                // Lazy load - this is not guaranteed to provide a value
                if (_console == null)
                {
                    _console = TryProvide<IConsole>();
                }

                return _console;
            }
            set
            {
                Contract.Requires(value != null, "value is null.");

                _console = value;

                // Register the console into the container
                value.RegisterAsProvidedInstance(typeof(IConsole), this);
            }
        }


        /// <summary>
        /// Gets or sets the localization culture.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
        public CultureInfo Locale
        {
            get
            {
                // Lazy load the culture
                if (_locale == null) _locale = CultureInfo.CurrentCulture;

                return _locale;
            }
            set
            {
                _locale = value;

                // Register the value if a value was provided
                if (value != null)
                {
                    value.RegisterAsProvidedInstance(typeof(CultureInfo), this);
                }
            }

        }

        /// <summary>
        ///     Gets or sets the command router responsible for routing <see cref="ChatCommand"/> objects
        ///     to their intended recipient.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when this property is set to null.
        /// </exception>
        /// <value>
        ///     The command router.
        /// </value>
        [NotNull]
        public IAlfredCommandRecipient CommandRouter
        {
            get
            {
                return Provide<IAlfredCommandRecipient>();
            }
            set
            {
                Contract.Requires(value != null, "value is null.");

                value.RegisterAsProvidedInstance(this);
            }
        }

        /// <summary>
        ///     Gets the search controller.
        /// </summary>
        /// <value>
        ///     The search controller.
        /// </value>
        [NotNull]
        public ISearchController SearchController
        {
            get
            {
                return Provide<ISearchController>();
            }
            set
            {
                Contract.Requires(value != null, "value is null.");

                value.RegisterAsProvidedInstance(typeof(ISearchController), this);
            }
        }

        /// <summary>
        ///     Builds a command.
        /// </summary>
        /// <param name="action"> The action the command will execute. </param>
        /// <returns>
        ///     A command.
        /// </returns>
        [NotNull]
        public IAlfredCommand BuildCommand([CanBeNull] Action action = null)
        {
            IAlfredCommand command = Provide<IAlfredCommand>(action, this);

            return command;
        }

        /// <summary>
        ///     Gets the error manager.
        /// </summary>
        /// <value>
        ///     The error manager.
        /// </value>
        public ErrorManager ErrorManager { get; }

    }
}