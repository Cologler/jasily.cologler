using System;
using JetBrains.Annotations;

namespace Jasily
{
    public struct StringRange : IEquatable<StringRange>
    {
        private readonly string document;
        private readonly int startIndex;
        private readonly int length;
        private int? hashCode;

        public StringRange(string document)
            : this(document, 0, document.Length)
        {
        }

        public StringRange([NotNull] string document, int startIndex, int length)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (startIndex < 0 || document.Length <= startIndex) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0 || document.Length < length) throw new ArgumentOutOfRangeException(nameof(length));

            this.document = document;
            this.startIndex = startIndex;
            this.length = length;
            this.hashCode = null;
        }

        public bool StartsWith([NotNull] string value, StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length > this.length) return false;
            return string.Compare(this.document, this.startIndex, value, 0, value.Length, comparison) == 0;
        }

        public bool StartsWith(StringRange value, StringComparison comparison = StringComparison.Ordinal)
            => this.length >= value.length && this.SubRange(0, value.length).Equals(value, comparison);

        #region string range

        [Pure]
        public bool Equals(StringRange other) => Equals(this, other);

        [Pure]
        public bool Equals(StringRange other, StringComparison comparison)
            => Equals(this, other, comparison);

        [Pure]
        public static bool Equals(StringRange first, StringRange second, StringComparison comparison = StringComparison.Ordinal)
            => first.length == second.length && 0 == string.Compare(
                   first.document, first.startIndex, second.document,
                   second.startIndex, first.length, comparison
               );

        [Pure]
        public StringRange SubRange(int startIndex, int length)
            => new StringRange(this.document, this.startIndex + startIndex, length);

        #endregion

        #region to string

        [Pure]
        public string SubString(int startIndex, int length)
            => this.document.Substring(this.startIndex + startIndex, length);

        [Pure]
        public override string ToString() => this.document.Substring(this.startIndex, this.length);

        #endregion

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>
        /// 一个 32 位有符号整数，它是该实例的哈希代码。
        /// </returns>
        public override int GetHashCode()
        {
            if (!this.hashCode.HasValue)
            {
                this.hashCode = this.ToString().GetHashCode();
            }
            return this.hashCode.Value;
        }
    }
}