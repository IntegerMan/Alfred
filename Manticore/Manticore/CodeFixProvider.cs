// ---------------------------------------------------------
// CodeFixProvider.cs
// 
// Created on:      08/30/2015 at 11:14 PM
// Last Modified:   08/30/2015 at 11:35 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;

namespace MattEland.Manticore
{
    /// <summary>
    ///     The Manticore Code Fix Provider which allows the system to inspect the codebase
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ManticoreCodeFixProvider))]
    [Shared]
    public class ManticoreCodeFixProvider : CodeFixProvider
    {
        private const string title = "Make uppercase";

        /// <summary>A list of diagnostic IDs that this provider can provider fixes for.</summary>
        public override sealed ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(ManticoreAnalyzer.DiagnosticId); }
        }

        /// <summary>
        ///     <para>
        ///         Gets an optional <see cref="FixAllProvider" /> that can fix all/multiple
        ///         occurrences of diagnostics fixed by this code fix provider.
        ///     </para>
        ///     <para>
        ///         Return <see langword="null" /> if the provider doesn't support fix all/multiple
        ///         occurrences. Otherwise, you can return any of the well known fix all providers from
        ///         <see cref="WellKnownFixAllProviders" /> or implement your own fix all provider.
        ///     </para>
        /// </summary>
        /// <returns>The fix all provider.</returns>
        public override sealed FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        /// <summary>Computes one or more fixes for the specified <see cref="CodeFixContext" /> .</summary>
        /// <param name="context">
        ///     A <see cref="CodeFixContext" /> containing context information about the
        ///     diagnostics to fix. The context must only contain diagnostics with an
        ///     <see cref="Diagnostic.Id" />
        ///     included in the <see cref="CodeFixProvider.FixableDiagnosticIds" /> for the current provider.
        /// </param>
        /// <returns>A <see cref="Task" /> for the codefix.</returns>
        public override sealed async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var document = context.Document;
            var syntaxRoot = document.GetSyntaxRootAsync(context.CancellationToken);
            var root = await syntaxRoot.ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration =
                root.FindToken(diagnosticSpan.Start)
                    .Parent.AncestorsAndSelf()
                    .OfType<TypeDeclarationSyntax>()
                    .First();

            // Register a code action that will invoke the fix.
            Func<CancellationToken, Task<Solution>> func;
            func = c => MakeUppercaseAsync(document, declaration, c);

            var action = CodeAction.Create(title, func, title);
            context.RegisterCodeFix(action, diagnostic);
        }

        /// <summary>Make the type declaration uppercase in an asynchronous operation.</summary>
        /// <param name="document">The document.</param>
        /// <param name="typeDeclaration">The type declaration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the solution proposed.</returns>
        [NotNull]
        private async Task<Solution> MakeUppercaseAsync(
            [NotNull] Document document,
            [NotNull] TypeDeclarationSyntax typeDeclaration,
            CancellationToken cancellationToken)
        {
            // Compute new uppercase name.
            var identifierToken = typeDeclaration.Identifier;
            var newName = identifierToken.Text.ToUpperInvariant();

            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration, cancellationToken);

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var project = document.Project;
            var originalSolution = project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution =
                await
                Renamer.RenameSymbolAsync(project.Solution,
                                          typeSymbol,
                                          newName,
                                          optionSet,
                                          cancellationToken).ConfigureAwait(false);

            // Return the new solution with the now-uppercase type name.
            return newSolution;
        }
    }
}