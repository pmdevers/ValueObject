using System;
using System.IO;

namespace ValueObjects.Generation;

public static class TemplateReader
{
    public static string ReadTemplate(string templateName)
    {
        var type = typeof(TemplateReader);

        var resourceName = $"{type.Namespace}.Templates.{templateName}.cs";

        using var stream = type.Assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Resource '{resourceName}' not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
