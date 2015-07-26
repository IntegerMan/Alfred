﻿using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Modules;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// Coordinates providing personal assistance to a user interface and receiving settings and queries back from the user
    /// interface.
    /// </summary>
    public class AlfredProvider
    {
        /// <summary>
        /// Gets or sets the console provider. This can be null.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console { get; set; }

        /// <summary>
        /// Gets the name and version string for display purposes.
        /// </summary>
        /// <value>The name and version string.</value>
        public static string NameAndVersionString => "Alfred 0.1 Alpha";

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <value>The modules.</value>
        [NotNull]
        public ICollection<AlfredModule> Modules { get; } = new HashSet<AlfredModule>();

        public AlfredStatus Status { get; private set; }

        /// <summary>
        /// Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        public void Initialize()
        {
            // Tell folks we're initializing
            Status = AlfredStatus.Initializing;

            const string LogHeader = "Alfred.Initialize";

            Console?.Log(LogHeader, "Initializing...");

            // TODO: Set things up here

            // We're done. Let the world know.
            Status = AlfredStatus.Online;
            Console?.Log(LogHeader, "Initilization Completed.");
        }

        /// <summary>
        /// Tells Alfred to go ahead and shut down.
        /// </summary>
        public void Shutdown()
        {
            const string LogHeader = "Alfred.Shutdown";

            Console?.Log(LogHeader, "Shutting down...");

            // TODO: Tear things down

            Console?.Log(LogHeader, "Shut down completed.");
        }

        public void AddStandardModules()
        {
            Modules.Add(new AlfredTimeModule());
        }
    }
}