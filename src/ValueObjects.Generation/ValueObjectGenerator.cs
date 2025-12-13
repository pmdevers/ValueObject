using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;
using System.Threading;

namespace ValueObjects.Generation;

[Generator]
public sealed class ValueObjectGenerator : IIncrementalGenerator
{
    private record struct ValueObjectParam(string Namespace, string RawName, string Self, string Value);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var valueObjects = context.SyntaxProvider
            .CreateSyntaxProvider(Filter, Transform)
            .Where(IsValidParam)
            .Collect();

        context.RegisterSourceOutput(valueObjects, GenerateSource);
    }

    private static bool Filter(SyntaxNode node, CancellationToken _)
    {
        return node is RecordDeclarationSyntax { ClassOrStructKeyword.RawKind: (int)SyntaxKind.StructKeyword } record &&
               record.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static ValueObjectParam Transform(GeneratorSyntaxContext ctx, CancellationToken ct)
    {
        if (ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, ct) is not INamedTypeSymbol { IsRecord: true, TypeKind: TypeKind.Struct } symbol)
        {
            return default;
        }

        var interfaceSymbol = symbol.AllInterfaces.FirstOrDefault(i =>
            i.OriginalDefinition is { MetadataName: "IValueObject`2", ContainingNamespace.Name: "ValueObjects" });

        if (interfaceSymbol is null || interfaceSymbol.TypeArguments.Length != 2)
        {
            return default;
        }

        var self = interfaceSymbol.TypeArguments[0];
        var value = interfaceSymbol.TypeArguments[1];

        return new ValueObjectParam(
            Namespace: symbol.ContainingNamespace.ToDisplayString(),
            RawName: symbol.Name,
            Self: self.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            Value: value.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
    }

    private static bool IsValidParam(ValueObjectParam p)
    {
        return p != default;
    }

    private static void GenerateSource(SourceProductionContext spc, System.Collections.Immutable.ImmutableArray<ValueObjectParam> items)
    {
        foreach (var vo in items)
        {
            var template = TemplateReader.ReadTemplate("ValueObject");
            var code = template
                .Replace("@namespace", vo.Namespace)
                .Replace("@RawName", vo.RawName)
                .Replace("@Self", vo.Self)
                .Replace("@Value", vo.Value);

            spc.AddSource($"{vo.RawName}_ValueObject.g.cs", SourceText.From(code, Encoding.UTF8));
        }
    }
}
