using System;

namespace Sachssoft.Documents
{
    /// <summary>
    /// Represents a format-independent identifier for data.
    /// </summary>
    /// <remarks>
    /// A data identifier consists of its textual representation and its semantic kind.
    /// Formats may interpret special identifier kinds differently from regular identifiers.
    /// The default value of the structure represents an empty identifier with
    /// <see cref="DataIdentifierKind.Default"/> semantics.
    /// </remarks>
    public readonly struct DataIdentifier : IEquatable<DataIdentifier>
    {
        private readonly string? _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataIdentifier"/> structure.
        /// </summary>
        /// <param name="value">The textual representation of the identifier.</param>
        /// <param name="kind">The semantic kind of the identifier.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public DataIdentifier(string value, DataIdentifierKind kind = DataIdentifierKind.Default)
        {
            ArgumentNullException.ThrowIfNull(value);

            _value = value;
            Kind = kind;
        }

        /// <summary>
        /// Gets the textual representation of the identifier.
        /// </summary>
        public string Value => _value ?? string.Empty;

        /// <summary>
        /// Gets the semantic kind of the identifier.
        /// </summary>
        public DataIdentifierKind Kind { get; }

        /// <inheritdoc/>
        public bool Equals(DataIdentifier other) => Kind == other.Kind && string.Equals(Value, other.Value, StringComparison.Ordinal);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is DataIdentifier other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Value, Kind);

        /// <inheritdoc/>
        public override string ToString() => Value;

        /// <summary>
        /// Determines whether two identifiers are equal.
        /// </summary>
        /// <param name="left">The first identifier to compare.</param>
        /// <param name="right">The second identifier to compare.</param>
        /// <returns><see langword="true"/> if both identifiers are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(DataIdentifier left, DataIdentifier right) => left.Equals(right);

        /// <summary>
        /// Determines whether two identifiers are not equal.
        /// </summary>
        /// <param name="left">The first identifier to compare.</param>
        /// <param name="right">The second identifier to compare.</param>
        /// <returns><see langword="true"/> if the identifiers are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(DataIdentifier left, DataIdentifier right) => !left.Equals(right);

        /// <summary>
        /// Converts a string to a default data identifier.
        /// </summary>
        /// <param name="value">The textual representation of the identifier.</param>
        /// <returns>A data identifier with <see cref="DataIdentifierKind.Default"/> semantics.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public static implicit operator DataIdentifier(string value) => new(value);
    }
}
