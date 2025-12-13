using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;
using System.Threading;

namespace ValueObject.Generation;

[Generator]
public sealed class IdentityGenerator : IIncrementalGenerator
{
    private struct IdParameter
    {
        public string Namespace;
        public string RawName;
        public string Type;
        public string Behaviour;
    }

    // Correct fully-qualified metadata name to match [Id<...>] usage.
    private readonly string _attributeName = "ValueObjects.Identifiers.IdAttribute`2";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var symbols = context.SyntaxProvider.ForAttributeWithMetadataName(_attributeName, Filter, Collect).Collect();

        context.RegisterSourceOutput(symbols, (spc, item) =>
        {
            foreach (var id in item)
            {
                spc.AddSource(
                    $"{id.RawName}_Identifier.g.cs",
                    SourceText.From(GenerateCode(id), Encoding.UTF8));
            }
        });
    }

    private static bool Filter(SyntaxNode node, CancellationToken ct)
    {
        return true;
    }

    private IdParameter Collect(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        var symbol = (INamedTypeSymbol)context.TargetSymbol;

        // Defensive: use FirstOrDefault to avoid throwing if attributes mismatch.

        var attrClass = context.Attributes.First(m => $"{m.AttributeClass?.ContainingNamespace}.{m.AttributeClass?.MetadataName}" == _attributeName).AttributeClass!;

        if (attrClass is null)
        {
            // If we ever get here, something is off with the metadata name; avoid crashing.
            return new IdParameter
            {
                Namespace = symbol.ContainingNamespace.ToString(),
                RawName = symbol.Name,
                Type = "global::System.Object",
                Behaviour = "global::System.Object"
            };
        }

        return new IdParameter
        {
            Namespace = symbol.ContainingNamespace.ToString(),
            RawName = symbol.Name,
            Type = GetFullTypeName(attrClass.TypeArguments[1]),
            Behaviour = GetFullTypeName(attrClass.TypeArguments[0])
        };
    }

    private static string GetFullTypeName(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
        {
            var genericArgs = string.Join(", ", namedTypeSymbol.TypeArguments.Select(GetFullTypeName));
            return $"{namedTypeSymbol.ContainingNamespace}.{namedTypeSymbol.Name}<{genericArgs}>";
        }
        return $"{typeSymbol.ContainingNamespace}.{typeSymbol.Name}";
    }

    private string GenerateCode(IdParameter id)
    {
        var template = TemplateReader.ReadTemplate("Identifier");
        return template
            .Replace("@namespace", id.Namespace)
            .Replace("@RawName", id.RawName)
            .Replace("@Type", id.Type)
            .Replace("@Behaviour", id.Behaviour);
    }
}
