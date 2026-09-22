using System;
using System.Collections.Generic;
using System.Text;

namespace Sachssoft.Documents.Formats.Ini;

/// <summary>
/// Provides escaping and array encoding helpers for INI values.
/// </summary>
internal static class IniCodec
{
    public static string EncodeValue(string value, char arraySeparator)
    {
        bool quote = value.Length == 0 || char.IsWhiteSpace(value[0]) || char.IsWhiteSpace(value[^1]) || value.IndexOfAny(new[] { arraySeparator, ';', '#', '"', '\r', '\n' }) >= 0;
        if (!quote) return value;
        var builder = new StringBuilder(value.Length + 2);
        builder.Append('"');
        foreach (var character in value)
        {
            switch (character)
            {
                case '\\': builder.Append("\\\\"); break;
                case '"': builder.Append("\\\""); break;
                case '\r': builder.Append("\\r"); break;
                case '\n': builder.Append("\\n"); break;
                case '\t': builder.Append("\\t"); break;
                default: builder.Append(character); break;
            }
        }
        builder.Append('"');
        return builder.ToString();
    }

    public static string DecodeValue(string value)
    {
        value = value.Trim();
        if (value.Length < 2 || value[0] != '"' || value[^1] != '"') return value;
        var builder = new StringBuilder(value.Length - 2);
        bool escape = false;
        for (int i = 1; i < value.Length - 1; i++)
        {
            char character = value[i];
            if (escape)
            {
                builder.Append(character switch { 'r' => '\r', 'n' => '\n', 't' => '\t', _ => character });
                escape = false;
            }
            else if (character == '\\') escape = true;
            else builder.Append(character);
        }
        if (escape) builder.Append('\\');
        return builder.ToString();
    }

    public static string EncodeArray(IEnumerable<string> values, char separator)
    {
        var builder = new StringBuilder();
        bool first = true;
        foreach (var value in values)
        {
            if (!first) builder.Append(separator);
            builder.Append(EncodeValue(value, separator));
            first = false;
        }
        return builder.ToString();
    }

    public static string[] DecodeArray(string value, char separator)
    {
        if (value.Length == 0) return Array.Empty<string>();
        var result = new List<string>();
        var builder = new StringBuilder();
        bool quoted = false;
        bool escape = false;
        foreach (char character in value)
        {
            if (escape) { builder.Append('\\'); builder.Append(character); escape = false; continue; }
            if (character == '\\' && quoted) { escape = true; continue; }
            if (character == '"') { quoted = !quoted; builder.Append(character); continue; }
            if (character == separator && !quoted) { result.Add(DecodeValue(builder.ToString())); builder.Clear(); continue; }
            builder.Append(character);
        }
        if (quoted) throw new FormatException("The INI array contains an unterminated quoted value.");
        result.Add(DecodeValue(builder.ToString()));
        return result.ToArray();
    }
}
