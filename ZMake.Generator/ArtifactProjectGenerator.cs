using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using PillarOfPillar;

namespace ZMake.Generator;

[Generator]
public class ArtifactProjectGenerator : IIncrementalGenerator
{
    public string GeneratedCodeAttribute { get; }

    public string GeneratorHitName { get; }

    public ArtifactProjectGenerator()
    {
        GeneratedCodeAttribute = Helper.GetGeneratedCodeAttribute(typeof(ArtifactProjectGenerator));
        GeneratorHitName = Helper.GetHintNameOfGenerator(typeof(ArtifactProjectGenerator));
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
    }

    public const string OverQualifiedAttributeName = "global::ZMake.Api.ArtifactProjectAttribute";

    public const string FullyQualifiedMetadataName = "ZMake.Api.ArtifactProjectAttribute";

    public const string InheritFrom = "global::ZMake.Api.IArtifactProject";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.ForAttributeWithMetadataName(
                FullyQualifiedMetadataName,
                (_, _) => true,
                (syntaxContext, _) => GetClassDeclarationForSourceGen(syntaxContext)
            ).Where(t => t is not null)
            .Select((t, _) => t!);
        context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()),
            (ctx, t) => GenerateCode(ctx, t.Left, t.Right));
    }


	/// <summary>
	/// Checks whether the Node is annotated with the [Report] attribute and maps syntax context to the specific node type (ClassDeclarationSyntax).
	/// </summary>
	/// <param name="context">Syntax context, based on CreateSyntaxProvider predicate</param>
	/// <returns>The specific cast and whether the attribute was found.</returns>
	private static ClassDeclarationSyntax? GetClassDeclarationForSourceGen(
		GeneratorAttributeSyntaxContext context)
	{
		// Go through all attributes of the class.
		foreach (var attributeSyntax in context.Attributes)
		{
			var name = attributeSyntax?
                .AttributeClass?
                .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

			if (name is null)
            {
                continue;
            }

			if (!name.StartsWith("global::"))
			{
				name = $"global::{name}";
			}

			if (name != OverQualifiedAttributeName)
            {
                continue;
            }

			if (context.TargetNode is not ClassDeclarationSyntax classDeclaration)
            {
                continue;
            }

			return classDeclaration;
		}

		return null;
	}

	private void GenerateCode(SourceProductionContext context, Compilation compilation,
		ImmutableArray<ClassDeclarationSyntax> classDeclarationSyntaxes)
	{
		try
		{
			// Go through all filtered class declarations.
			foreach (var declarator in classDeclarationSyntaxes)
            {
                var syntanTree = declarator.SyntaxTree;

                var semantic = compilation.GetSemanticModel(syntanTree);

                var symbolInfo = semantic.GetDeclaredSymbol(declarator);

                if (symbolInfo is null)
                {
                    continue;
                }

                CSharpBuilder builder = new();

                builder.EnterNamespace(symbolInfo);
                builder.EnterNamedType(symbolInfo);
                builder.PutHeadAndEndWithIndent($"partial class {symbolInfo.Name} : {InheritFrom} {{", "}");

                if (syntanTree.FilePath != string.Empty)
                {
                    builder.PutBody(
                        $$"""
                        public static string CurrentSourceDirectory { get; } = {{SymbolDisplay.FormatLiteral(syntanTree.FilePath,true)}};
                        public global::ZMake.Api.ArtifactName Name { get; } = GetArtifactName();
                        """);
                }

				context.AddSource($"{GeneratorHitName}.{Helper.GetHintNameOfType(symbolInfo)}.g.cs",
					SourceText.From(builder.ToString(), Encoding.UTF8));
			}
		}
		catch (Exception ex)
		{
			context.Debug(ex.ToString().Replace("\n", ";;;"));
		}
	}
}
