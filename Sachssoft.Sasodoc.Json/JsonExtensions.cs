using System;
using System.Globalization;
using System.Numerics;
using System.Text.Json.Nodes;

namespace Sachssoft.Sasodoc.Formats.Json
{
    public static class JsonExtensions
    {
        public static byte ReadByte(this JsonObject jsonItem, string key, byte fallback = default)
        => ReadInteger<byte>(jsonItem, key, fallback);

        public static sbyte ReadSByte(this JsonObject jsonItem, string key, sbyte fallback = default)
            => ReadInteger<sbyte>(jsonItem, key, fallback);

        public static short ReadInt16(this JsonObject jsonItem, string key, short fallback = default)
            => ReadInteger<short>(jsonItem, key, fallback);

        public static ushort ReadUInt16(this JsonObject jsonItem, string key, ushort fallback = default)
            => ReadInteger<ushort>(jsonItem, key, fallback);

        public static int ReadInt32(this JsonObject jsonItem, string key, int fallback = default)
            => ReadInteger<int>(jsonItem, key, fallback);

        public static uint ReadUInt32(this JsonObject jsonItem, string key, uint fallback = default)
            => ReadInteger<uint>(jsonItem, key, fallback);

        public static long ReadInt64(this JsonObject jsonItem, string key, long fallback = default)
            => ReadInteger<long>(jsonItem, key, fallback);

        public static ulong ReadUInt64(this JsonObject jsonItem, string key, ulong fallback = default)
            => ReadInteger<ulong>(jsonItem, key, fallback);

        public static nint ReadNInt(this JsonObject jsonItem, string key, nint fallback = default)
            => ReadInteger<nint>(jsonItem, key, fallback);

        public static nuint ReadNUInt(this JsonObject jsonItem, string key, nuint fallback = default)
            => ReadInteger<nuint>(jsonItem, key, fallback);

        public static float ReadSingle(this JsonObject jsonItem, string key, float fallback = default)
            => ReadFloat<float>(jsonItem, key, fallback);

        public static double ReadDouble(this JsonObject jsonItem, string key, double fallback = default)
            => ReadFloat<double>(jsonItem, key, fallback);

        public static decimal ReadDecimal(this JsonObject jsonItem, string key, decimal fallback = default)
            => ReadFloat<decimal>(jsonItem, key, fallback);

        public static bool ReadBoolean(this JsonObject jsonItem, string key, bool fallback = default)
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonValue value)
                return fallback;

            // echtes JSON-Bool
            if (value.TryGetValue<bool>(out var b))
                return b;

            // numerisch → 0 = false, alles andere = true
            if (value.TryGetValue<int>(out var i))
                return i != 0;

            if (value.TryGetValue<long>(out var l))
                return l != 0;

            if (value.TryGetValue<double>(out var d))
                return Math.Abs(d) > double.Epsilon;

            // String → parse
            if (value.TryGetValue<string>(out var s))
            {
                // direktes bool parsing
                if (bool.TryParse(s, out var parsed))
                    return parsed;

                // numerische Strings
                if (string.Equals(s, "1", StringComparison.OrdinalIgnoreCase))
                    return true;

                if (string.Equals(s, "0", StringComparison.OrdinalIgnoreCase))
                    return false;

                // optional: "yes" / "no" / "true"/"false" etc.
                if (string.Equals(s, "yes", StringComparison.OrdinalIgnoreCase))
                    return true;

                if (string.Equals(s, "no", StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            // alles andere → fallback
            return fallback;
        }

        public static string? ReadString(this JsonObject jsonItem, string key, string? fallback = null)
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonValue value)
                return fallback;

            // JSON string
            if (value.TryGetValue<string>(out var s))
                return s;

            // JSON number → ToString invariant
            if (value.TryGetValue<int>(out var i))
                return i.ToString(CultureInfo.InvariantCulture);

            if (value.TryGetValue<long>(out var l))
                return l.ToString(CultureInfo.InvariantCulture);

            if (value.TryGetValue<double>(out var d))
                return d.ToString(CultureInfo.InvariantCulture);

            // JSON bool → "true"/"false"
            if (value.TryGetValue<bool>(out var b))
                return b ? "true" : "false";

            // fallback
            return fallback;
        }

        public static TEnum ReadEnum<TEnum>(this JsonObject jsonItem, string key, TEnum fallback = default)
            where TEnum : struct, Enum
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonValue value)
                return fallback;

            // String → Enum Name (case-insensitive)
            if (value.TryGetValue<string>(out var s) &&
                Enum.TryParse<TEnum>(s, ignoreCase: true, out var parsedByName))
            {
                return parsedByName;
            }

            // Integer → Enum underlying type
            if (value.TryGetValue<int>(out var i) && Enum.IsDefined(typeof(TEnum), i))
            {
                return UnsafeCastEnum<TEnum, int>(i);
            }

            if (value.TryGetValue<long>(out var l) && Enum.IsDefined(typeof(TEnum), l))
            {
                return UnsafeCastEnum<TEnum, long>(l);
            }

            // Fallback
            return fallback;
        }

        public static Version? ReadVersion(this JsonObject jsonItem, string key, Version? fallback = null)
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonValue value)
                return fallback;

            // String → Version
            if (value.TryGetValue<string>(out var s) &&
                Version.TryParse(s, out var parsed))
            {
                return parsed;
            }

            // Fallback
            return fallback;
        }

        public static Guid ReadGuid(this JsonObject jsonItem, string key, Guid fallback = default)
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonValue value)
                return fallback;

            if (value.TryGetValue<string>(out var s) && Guid.TryParse(s, out var g))
                return g;

            return fallback;
        }

        public static char ReadChar(this JsonObject jsonItem, string key, char fallback = default)
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonValue value)
                return fallback;

            if (value.TryGetValue<string>(out var s) && s.Length == 1)
                return s[0];

            if (value.TryGetValue<int>(out var i))
                return (char)i; // optional: cast int → char

            return fallback;
        }

        public static TimeSpan ReadTimeSpan(this JsonObject jsonItem, string key, TimeSpan fallback = default)
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonValue value)
                return fallback;

            if (value.TryGetValue<string>(out var s) && TimeSpan.TryParse(s, CultureInfo.InvariantCulture, out var ts))
                return ts;

            if (value.TryGetValue<double>(out var d))
                return TimeSpan.FromSeconds(d);

            if (value.TryGetValue<long>(out var l))
                return TimeSpan.FromSeconds(l);

            return fallback;
        }

        public static DateTime ReadDateTime(this JsonObject jsonItem, string key, DateTime fallback = default)
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonValue value)
                return fallback;

            // ISO8601 / string
            if (value.TryGetValue<string>(out var s) &&
                DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dt))
            {
                return dt;
            }

            // Unix timestamp (seconds)
            if (value.TryGetValue<long>(out var l))
                return DateTimeOffset.FromUnixTimeSeconds(l).UtcDateTime;

            if (value.TryGetValue<double>(out var d))
                return DateTimeOffset.FromUnixTimeSeconds((long)d).UtcDateTime;

            return fallback;
        }

        public static JsonObject? ReadJsonObject(this JsonObject jsonItem, string key)
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonObject obj)
                return null;
            return obj;
        }

        public static JsonArray? ReadJsonArray(this JsonObject jsonItem, string key)
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonArray arr)
                return null;
            return arr;
        }

        private static T ReadInteger<T>(JsonObject jsonItem, string key, T fallback)
            where T : IBinaryInteger<T>, IMinMaxValue<T>
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) ||
                node is not JsonValue value)
                return fallback;

            // Direkter Integer-Typ
            if (value.TryGetValue<T>(out var direct))
                return direct;

            // Zahl (double / float / decimal etc.)
            if (value.TryGetValue<double>(out var d))
                return ClampFromDouble<T>(d);

            // String
            if (value.TryGetValue<string>(out var s) &&
                double.TryParse(
                    s,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var parsed))
            {
                return ClampFromDouble<T>(parsed);
            }

            return fallback;
        }

        private static T ReadFloat<T>(JsonObject jsonItem, string key, T fallback)
            where T : IFloatingPoint<T>, IMinMaxValue<T>
        {
            if (!jsonItem.TryGetPropertyValue(key, out var node) || node is not JsonValue value)
                return fallback;

            // 1️⃣ Direktes Floating-Point
            if (value.TryGetValue<T>(out var direct))
                return direct;

            // 2️⃣ Integer-Typen (int, long, byte, ...) → casten
            if (value.TryGetValue<int>(out var i))
                return ClampToRange<T>(T.CreateChecked(i));

            if (value.TryGetValue<long>(out var l))
                return ClampToRange<T>(T.CreateChecked(l));

            // 3️⃣ String → parse
            if (value.TryGetValue<string>(out var s) &&
                T.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
            {
                return ClampToRange<T>(parsed);
            }

            return fallback;
        }

        private static T ClampFromDouble<T>(double value)
            where T : IBinaryInteger<T>, IMinMaxValue<T>
        {
            if (double.IsNaN(value))
                return T.Zero;

            // Abschneiden wie klassische Integer-Konvertierung
            value = Math.Truncate(value);

            // Clamp auf T.MinValue / T.MaxValue
            if (value < double.CreateChecked(T.MinValue))
                return T.MinValue;

            if (value > double.CreateChecked(T.MaxValue))
                return T.MaxValue;

            return T.CreateChecked(value);
        }

        private static T ClampToRange<T>(T value)
            where T : IFloatingPoint<T>, IMinMaxValue<T>
        {
            if (T.IsNaN(value))
                return T.Zero;

            if (value < T.MinValue)
                return T.MinValue;

            if (value > T.MaxValue)
                return T.MaxValue;

            return value;
        }

        private static TEnum UnsafeCastEnum<TEnum, TUnderlying>(TUnderlying value)
            where TEnum : struct, Enum
            where TUnderlying : struct
        {
            return (TEnum)(object)value;
        }
    }
}
