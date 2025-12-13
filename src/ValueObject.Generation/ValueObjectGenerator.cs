using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;
using System.Threading;

namespace ValueObject.Generation;

[Generator]
public sealed class ValueObjectGenerator : IIncrementalGenerator
{
    private struct ValueObjectParam
    {
        public string Namespace;
        public string RawName;
        public string Self;
        public string Value;
    }

    private const string _interfaceNamespace = "ValueObjects";
    private const string _interfaceMetadataName = "IValueObject`2";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidates = context.SyntaxProvider.CreateSyntaxProvider(
            IsPartialRecordStruct,
            static (ctx, ct) =>
            {
                if (ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, ct) is not INamedTypeSymbol symbol || !IsRecordStruct(symbol))
                {
                    return (found: false, param: default);
                }

                var iface = symbol.AllInterfaces.FirstOrDefault(i =>
                    i.OriginalDefinition.MetadataName == _interfaceMetadataName &&
                    i.OriginalDefinition.ContainingNamespace.ToDisplayString() == _interfaceNamespace);

                if (iface is null || iface.TypeArguments.Length != 2)
                {
                    return (found: false, param: default);
                }

                var p = new ValueObjectParam
                {
                    Namespace = symbol.ContainingNamespace.ToString(),
                    RawName = symbol.Name,
                    Self = GetFullTypeName(iface.TypeArguments[0]),
                    Value = GetFullTypeName(iface.TypeArguments[1])
                };
                return (found: true, param: p);
            })
            .Where(static r => r.found)
            .Select(static (r, _) => r.param)
            .Collect();

        context.RegisterSourceOutput(candidates, static (spc, items) =>
        {
            foreach (var vo in items)
            {
                spc.AddSource(
                    $"{vo.RawName}_ValueObject.g.cs",
                    SourceText.From(GenerateCode(vo), Encoding.UTF8));
            }
        });
    }

    private static bool IsPartialRecordStruct(SyntaxNode node, CancellationToken ct)
    {
        if (node is not RecordDeclarationSyntax record)
        {
            return false;
        }

        if (!record.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword))
        {
            return false;
        }

        var isPartial = record.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
        return isPartial;
    }

    private static bool IsRecordStruct(INamedTypeSymbol symbol)
    {
        return symbol.IsRecord && symbol.TypeKind == TypeKind.Struct;
    }

    private static string GetFullTypeName(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is INamedTypeSymbol named && named.IsGenericType)
        {
            var genericArgs = string.Join(", ", named.TypeArguments.Select(GetFullTypeName));
            return $"{named.ContainingNamespace}.{named.Name}<{genericArgs}>";
        }

        return $"{typeSymbol.ContainingNamespace}.{typeSymbol.Name}";
    }

    private static string GenerateCode(ValueObjectParam vo)
    {
        var template = TemplateReader.ReadTemplate("ValueObject");
        return template
            .Replace("@namespace", vo.Namespace)
            .Replace("@RawName", vo.RawName)
            .Replace("@Self", vo.Self)
            .Replace("@Value", vo.Value);
    }
}
