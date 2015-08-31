// ---------------------------------------------------------
// DiagnosticAnalyzer.cs
// 
// Created on:      08/30/2015 at 11:47 PM
// Last Modified:   08/31/2015 at 10:26 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
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
        ///     The analysis category.
        /// </summary>
        internal const string Category = "Experimental";

        /// <summary>
        ///     The diagnostic identifier.
        /// </summary>
        internal const string DiagnosticId = "Manticore Analyzer";

        /// <summary>
        ///     The message format.
        /// </summary>
        internal static readonly LocalizableString MessageFormat = "Manticore '{0}'";

        /// <summary>
        ///     The title to use for diagnostic messages in code analysis.
        /// </summary>
        internal static readonly LocalizableString MessageTitle = "Manticore Analyzer Engine";

        /// <summary>
        ///     The severity of the diagnostic message.
        /// </summary>
        private static readonly DiagnosticSeverity Severity = DiagnosticSeverity.Warning;

        /// <summary>
        ///     The rule associated with this analyzer.
        /// </summary>
        internal static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId,
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

        /// <summary>Called once at session start to register actions in the analysis context.</summary>
        /// <param name="context"> The symbol analysis context. </param>
        public override void Initialize([NotNull] AnalysisContext context)
        {
            context.RegisterCodeBlockAction(AnalyzeCodeBlock);
            context.RegisterCompilationAction(AnalyzeCompilation);
            context.RegisterSemanticModelAction(AnalyzeSemanticModel);
            context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
            context.RegisterSymbolAction(AnalyzeNamedTypeSymbol, SymbolKind.NamedType);
        }

        /// <summary>
        /// Analyzes a syntax tree.
        /// </summary>
        /// <param name="context">The context.</param>
        private void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var tree = context.Tree;

            // Look over the object for debugging
            var inspect = context;
        }

        /// <summary>
        /// Analyzes a semantic model.
        /// </summary>
        /// <param name="context">The context.</param>
        private void AnalyzeSemanticModel(SemanticModelAnalysisContext context)
        {
            var model = context.SemanticModel;

            // Look over the object for debugging
            var inspect = context;
        }

        /// <summary>
        /// Analyzes a compilation.
        /// </summary>
        /// <param name="context">The context.</param>
        private void AnalyzeCompilation(CompilationAnalysisContext context)
        {
            var compilation = context.Compilation;

            // Look over the object for debugging
            var inspect = context;
        }

        /// <summary>
        /// Analyzes a code block.
        /// </summary>
        /// <param name="context">The context.</param>
        private void AnalyzeCodeBlock(CodeBlockAnalysisContext context)
        {
            var owner = context.OwningSymbol;
            var block = context.CodeBlock;
            var model = context.SemanticModel;

            // Look over the object for debugging
            var inspect = context;
        }

        /// <summary>
        ///     Analyzes a symbol.
        /// </summary>
        /// <param name="context"> The symbol analysis context. </param>
        private static void AnalyzeNamedTypeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            // Find classes ending in "Test" that are not part of an assembly ending in "Tests".
            if (namedTypeSymbol.Name.EndsWith("Test", StringComparison.Ordinal) &&
                !namedTypeSymbol.ContainingAssembly.Name.EndsWith("Tests", StringComparison.OrdinalIgnoreCase))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule,
                                                   namedTypeSymbol.Locations[0],
                                                   namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}