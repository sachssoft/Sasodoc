using System;
using System.Collections.Generic;

namespace Sachssoft.Documents.Formats.Ini;

/// <summary>
/// Represents the internal section and value model of an INI document.
/// </summary>
internal sealed class IniDocument
{
    private readonly Dictionary<string, Dictionary<string, string>> _sections = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets all sections contained in the document.
    /// </summary>
    public IReadOnlyDictionary<string, Dictionary<string, string>> Sections => _sections;

    /// <summary>
    /// Gets or creates a section.
    /// </summary>
    public Dictionary<string, string> GetOrCreateSection(string name)
    {
        if (!_sections.TryGetValue(name, out var section))
        {
            section = new Dictionary<string, string>(StringComparer.Ordinal);
            _sections.Add(name, section);
        }
        return section;
    }

    /// <summary>
    /// Attempts to get a section.
    /// </summary>
    public bool TryGetSection(string name, out Dictionary<string, string>? section) => _sections.TryGetValue(name, out section);

    /// <summary>
    /// Removes a section and all nested sections below it.
    /// </summary>
    public void RemoveSectionTree(string name, char separator)
    {
        var prefix = name + separator;
        var remove = new List<string>();
        foreach (var key in _sections.Keys)
        {
            if (string.Equals(key, name, StringComparison.Ordinal) || key.StartsWith(prefix, StringComparison.Ordinal)) remove.Add(key);
        }
        foreach (var key in remove) _sections.Remove(key);
    }
}
