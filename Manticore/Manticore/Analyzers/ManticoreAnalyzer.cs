// ---------------------------------------------------------
// DiagnosticAnalyzer.cs
// 
// Created on:      08/30/2015 at 11:47 PM
// Last Modified:   08/31/2015 at 10:26 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MattEland.Manticore.Analyzers
{
    /// <summary>
    ///     A Visual Studio Analyzer that lets Alfred get information on the codebase.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    [UsedImplicitly]
    public class ManticoreAnalyzer : ManticoreAnalyzerBase
    {
        /// <summary>
        ///     The analysis category.
        /// </summary>
        private const string Category = "Experimental";

        /// <summary>
        ///     The diagnostic identifier.
        /// </summary>
        private static string DiagnosticId { get; } = "Manticore Analyzer";

        /// <summary>
        ///     The message format.
        /// </summary>
        private static readonly LocalizableString _messageFormat = "Manticore '{0}'";

        /// <summary>
        ///     The title to use for diagnostic messages in code analysis.
        /// </summary>
        private static readonly LocalizableString _messageTitle = "Manticore Analyzer Engine";

        /// <summary>
        ///     The severity of the diagnostic message.
        /// </summary>
        private static readonly DiagnosticSeverity _severity = DiagnosticSeverity.Warning;

        /// <summary>
        ///     The rule associated with this analyzer.
        /// </summary>
        private static readonly DiagnosticDescriptor _rule = new DiagnosticDescriptor(DiagnosticId,
                                                                                      _messageTitle,
                                                                                      _messageFormat,
                                                                                      Category,
                                                                                      _severity,
                                                                                      true);

        /// <summary>
        ///     Gets the rule identifier.
        /// </summary>
        /// <value>
        ///     The rule identifier.
        /// </value>
        protected override string RuleId { get { return DiagnosticId; } }

        /// <summary>
        ///     Returns a set of descriptors for the diagnostics that this analyzer is capable of
        ///     producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(_rule); }
        }

        /// <summary>
        /// Gets the name of the analyzer.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return "Manticore"; }
        }

        /// <summary>
        /// Called once on concrete analyzers at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context">The analysis context.</param>
        protected override void InitializeProtected(AnalysisContext context)
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

            // TODO: Tell Alfred
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

            // TODO: Tell Alfred
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

            // TODO: Tell Alfred
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

            // TODO: Tell Alfred
        }

        /// <summary>
        ///     Analyzes a symbol.
        /// </summary>
        /// <param name="context"> The symbol analysis context. </param>
        private static void AnalyzeNamedTypeSymbol(SymbolAnalysisContext context)
        {
            var compilation = context.Compilation;
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            // Look over the object for debugging
            var inspect = context;

            // TODO: Tell Alfred
        }
    }
}