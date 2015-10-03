using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MattEland.Manticore.Analyzers
{
    /// <summary>
    ///     The Manticore analyzer base class.
    /// </summary>
    public abstract class ManticoreAnalyzerBase : DiagnosticAnalyzer
    {
        /// <summary>
        /// Gets the Alfred instance.
        /// </summary>
        /// <value>The Alfred instance.</value>
        [NotNull]
        public IAlfred Alfred { get; }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IAlfredContainer Container { get; }

        /// <summary>
        /// Gets the console.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManticoreAnalyzerBase"/> class.
        /// </summary>
        protected ManticoreAnalyzerBase()
        {
            // Set up the container
            IAlfredContainer container = AlfredContainerHelper.ProvideContainer();
            Container = container;

            if (Container.HasMapping(typeof(IAlfred)))
            {
                Alfred = Container.Provide<IAlfred>();
            }
            else
            {
                Alfred = new AlfredApplication(Container);
                Alfred.RegisterAsProvidedInstance(typeof(IAlfred));
            }

            Console = Container.Console;
        }

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context"/>
        public override void Initialize([NotNull] AnalysisContext context)
        {
            var logHeader = $"Analyzers.{Name.Replace(" ", "")}.Initialize";

            $"Initializing {Name} Analyzer".Log(logHeader, LogLevel.Verbose, Console);

            InitializeProtected(context);

            $"{Name} Analyzer Initialized.".Log(logHeader, LogLevel.Verbose, Console);
        }

        /// <summary>
        /// Gets the name of the analyzer.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public abstract string Name { get; }

        /// <summary>
        ///     Gets the rule identifier.
        /// </summary>
        /// <value>
        ///     The rule identifier.
        /// </value>
        protected abstract string RuleId { get; }

        /// <summary>
        /// Called once on concrete analyzers at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context">The analysis context.</param>
        protected abstract void InitializeProtected([NotNull] AnalysisContext context);

        /// <summary>
        ///     Logs that the analyzer rule fired.
        /// </summary>
        /// <param name="offender">
        ///     The name of the symbol, member, file, or assembly that violated the rule.
        /// </param>
        /// <param name="location"> The location. </param>
        protected void LogRuleFired([NotNull] string offender, [CanBeNull] Location location)
        {
            string message = $"Rule {RuleId} triggered on {offender} at {location.AsNonNullString()}";
            message.Log($"Analyzer.{RuleId}", LogLevel.Info, Console);
        }
    }
}