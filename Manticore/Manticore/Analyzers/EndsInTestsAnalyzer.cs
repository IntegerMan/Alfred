using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MattEland.Manticore.Analyzers
{
    /// <summary>
    ///     An analyzer to find classes with names ending in "Tests" that are not in Test assemblies.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp), UsedImplicitly]
    public class EndsInTestsAnalyzer : ManticoreAnalyzerBase
    {
        /// <summary>
        ///     The analysis category.
        /// </summary>
        private const string Category = "Design";

        /// <summary>
        ///     The diagnostic identifier.
        /// </summary>
        public const string DiagnosticId = "MANT-1000";

        /// <summary>
        ///     Gets the rule identifier.
        /// </summary>
        /// <value>
        ///     The rule identifier.
        /// </value>
        protected override string RuleId { get { return DiagnosticId; } }

        /// <summary>
        ///     The message format.
        /// </summary>
        private static readonly LocalizableString MessageFormat = "The class '{0}' ends in 'Tests' but is not in a Test assembly.";

        /// <summary>
        ///     The title to use for diagnostic messages in code analysis.
        /// </summary>
        private static readonly LocalizableString MessageTitle = "Test classes should be in test assemblies";

        /// <summary>
        ///     The severity of the diagnostic message.
        /// </summary>
        private static readonly DiagnosticSeverity Severity = DiagnosticSeverity.Warning;

        /// <summary>
        ///     The rule associated with this analyzer.
        /// </summary>
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId,
                                                                                      MessageTitle,
                                                                                      MessageFormat,
                                                                                      Category,
                                                                                      Severity,
                                                                                      true);

        /// <summary>
        ///     Returns a set of descriptors for the diagnostics that this analyzer is capable of
        ///     producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        /// <summary>
        ///     Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context"> The symbol analysis context. </param>
        protected override void InitializeProtected([NotNull] AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeNamedTypeSymbol, SymbolKind.NamedType);
        }

        /// <summary>
        /// Gets the name of the analyzer.
        /// </summary>
        /// <value>The name.</value>
        public override string Name { get { return "Ends in Test"; } }

        /// <summary>
        ///     Analyzes a symbol.
        /// </summary>
        /// <param name="context"> The symbol analysis context. </param>
        private void AnalyzeNamedTypeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = context.Symbol as INamedTypeSymbol;
            var name = namedTypeSymbol?.Name;

            // Early exit
            if (name == null)
            {
                return;
            }

            // Find classes ending in "Test" that are not part of an assembly ending in "Tests".
            if (name.EndsWith("Test", StringComparison.Ordinal) ||
                name.EndsWith("Tests", StringComparison.Ordinal))
            {
                var assemblyname = namedTypeSymbol.ContainingAssembly?.Name;

                if (assemblyname != null && !assemblyname.EndsWith("Tests", StringComparison.OrdinalIgnoreCase))
                {
                    // For all such symbols, produce a diagnostic.
                    var location = namedTypeSymbol.Locations[0];
                    var diagnostic = Diagnostic.Create(Rule,
                                                       location,
                                                       name);

                    LogRuleFired(name, location);

                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

    }
}