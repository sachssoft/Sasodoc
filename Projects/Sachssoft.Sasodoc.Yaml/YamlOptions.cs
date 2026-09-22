namespace Sachssoft.Documents.Formats.Yaml;

/// <summary>
/// Provides YAML-specific options used when reading and writing YAML documents.
/// </summary>
public sealed class YamlOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether YAML aliases are allowed by higher-level processing.
    /// </summary>
    public bool AllowAliases { get; set; } = true;
}
