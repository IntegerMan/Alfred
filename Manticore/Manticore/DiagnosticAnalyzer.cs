using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

using JetBrains.Annotations;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MattEland.Manticore
{
    /// <summary>
    ///     A Visual Studio Analyzer that lets Alfred get information on the codebase.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    [UsedImplicitly]
    public class ManticoreAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        ///     The diagnostic identifier.
        /// </summary>
        public const string DiagnosticId = "Manticore Analyzer";
        internal static readonly LocalizableString Title = "Manticore Analyzer Engine";

        /// <summary>
        ///     The message format.
        /// </summary>
        internal static readonly LocalizableString MessageFormat = "Manticore '{0}'";

        /// <summary>
        ///     The category.
        /// </summary>
        internal const string Category = "Experimental";

        /// <summary>
        ///     The rule associated with this analyzer.
        /// </summary>
        internal static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        /// <summary>
        /// Returns a set of descriptors for the diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context"> The symbol analysis context. </param>
        public override void Initialize([NotNull] AnalysisContext context)
        {
            Debug.Assert(context != null);

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        /// <summary>
        ///     Analyzes a symbol.
        /// </summary>
        /// <param name="context"> The symbol analysis context. </param>
        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            // Find just those named type symbols with names containing lowercase letters.
            Debug.Assert(namedTypeSymbol?.Name != null);
            if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }

    }
}