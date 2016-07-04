using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Jasily
{
    public struct StringRange : IEquatable<StringRange>, IEnumerable<char>
    {
        private readonly string document;
        private readonly int startIndex;
        private readonly int length;
        private int? hashCode;

        public StringRange([NotNull] string document)
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

        private void SubRangeCheck(int startIndex)
        {
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex >= this.length) throw new IndexOutOfRangeException();
        }

        private void SubRangeCheck(int startIndex, int length)
        {
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex + length > this.length) throw new IndexOutOfRangeException();
        }

        #region start & end

        public bool StartsWith(char value) => this.length != 0 && this.document[this.startIndex] == value;

        public bool StartsWith(char value, StringComparison comparison)
        {
            if (0 == this.length) return false;
            return string.Compare(this.document, this.startIndex, value.ToString(), 0, 1, comparison) == 0;
        }

        public bool StartsWith([NotNull] string value, StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length > this.length) return false;
            return string.Compare(this.document, this.startIndex, value, 0, value.Length, comparison) == 0;
        }

        public bool StartsWith(StringRange value, StringComparison comparison = StringComparison.Ordinal)
            => this.length >= value.length && this.SubRange(0, value.length).Equals(value, comparison);

        #endregion

        #region index & contain

        [Pure]
        public int IndexOf([NotNull] string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return this.document.IndexOf(value, this.startIndex, this.length, comparisonType);
        }

        [Pure]
        public int IndexOf([NotNull] string value, int startIndex, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            this.SubRangeCheck(startIndex);
            return this.document.IndexOf(value, this.startIndex + startIndex, this.length - startIndex, comparisonType);
        }

        [Pure]
        public int IndexOf([NotNull] string value, int startIndex, int length, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            this.SubRange(startIndex, length);
            return this.document.IndexOf(value, this.startIndex + startIndex, length, comparisonType);
        }

        [Pure]
        public bool Contains([NotNull] string value, StringComparison comparisonType = StringComparison.Ordinal)
            => this.IndexOf(value, comparisonType) > -1;

        [Pure]
        public bool Contains([NotNull] string value, int startIndex,
            StringComparison comparisonType = StringComparison.Ordinal)
            => this.IndexOf(value, startIndex, comparisonType) > -1;

        [Pure]
        public bool Contains([NotNull] string value, int startIndex, int length,
            StringComparison comparisonType = StringComparison.Ordinal)
            => this.IndexOf(value, startIndex, length, comparisonType) > -1;

        #endregion

        #region trim

        [Pure]
        public StringRange TrimStart()
        {
            var index = this.startIndex;
            var maxIndex = this.startIndex + this.length;
            while (index <= maxIndex && char.IsWhiteSpace(this.document, index)) index++;
            return this.SubRange(index - this.startIndex);
        }

        [Pure]
        public StringRange TrimStart([NotNull] params char[] trimChars)
        {
            if (trimChars.Length == 0) return this;
            var index = this.startIndex;
            var maxIndex = this.startIndex + this.length;
            while (index <= maxIndex && trimChars.Contains(this.document[index])) index++;
            return this.SubRange(index - this.startIndex);
        }

        [Pure]
        public StringRange TrimEnd()
        {
            if (this.length == 0) return this;

            var index = this.startIndex + this.length - 1;
            while (index >= this.startIndex && char.IsWhiteSpace(this.document, index)) index--;
            return this.SubRangeOfLength(index - this.startIndex + 1);
        }

        [Pure]
        public StringRange TrimEnd([NotNull] params char[] trimChars)
        {
            if (trimChars.Length == 0) return this;
            if (this.length == 0) return this;

            var index = this.startIndex + this.length - 1;
            while (index >= this.startIndex && trimChars.Contains(this.document[index])) index--;
            return this.SubRangeOfLength(index - this.startIndex + 1);
        }

        [Pure]
        public StringRange Trim() => this.TrimStart().TrimEnd();

        [Pure]
        public StringRange Trim([NotNull] params char[] trimChars)
            => this.TrimStart(trimChars).TrimEnd(trimChars);

        #endregion

        #region string range

        [Pure]
        public StringRange SubRange(int startIndex) => this.SubRange(startIndex, this.length - startIndex);

        [Pure]
        public StringRange SubRange(int startIndex, int length)
        {
            this.SubRangeCheck(startIndex, length);
            return new StringRange(this.document, startIndex + this.startIndex, length);
        }

        [Pure]
        public StringRange SubRangeOfLength(int length) => this.SubRange(0, length);

        #endregion

        #region to string

        [Pure]
        public string Substring(int startIndex)
        {
            this.SubRangeCheck(startIndex);
            return this.document.Substring(this.startIndex + startIndex);
        }

        [Pure]
        public string Substring(int startIndex, int length)
        {
            this.SubRangeCheck(startIndex, length);
            return this.document.Substring(this.startIndex + startIndex, length);
        }

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

        #region equal

        [Pure]
        public override bool Equals(object other)
        {
            var str = other as string;
            if (str != null) return this.Equals(str);
            var range = other as StringRange?;
            if (range != null) return this.Equals(range.Value);
            // others
            return Equals(this, other);
        }

        [Pure]
        public bool Equals(StringRange other) => Equals(this, other);

        [Pure]
        public bool Equals(StringRange other, StringComparison comparison)
            => Equals(this, other, comparison);

        [Pure]
        public bool Equals(string other) => Equals(this, other);

        [Pure]
        public bool Equals(string other, StringComparison comparison)
            => Equals(this, other, comparison);

        [Pure]
        public static bool operator ==(StringRange first, StringRange second) => Equals(first, second);

        [Pure]
        public static bool operator !=(StringRange first, StringRange second) => !(first == second);

        [Pure]
        public static bool operator ==(StringRange first, string second) => Equals(first, second);

        [Pure]
        public static bool operator !=(StringRange first, string second) => !(first == second);

        [Pure]
        public static bool operator ==(string second, StringRange first) => Equals(first, second);

        [Pure]
        public static bool operator !=(string second, StringRange first) => !(first == second);

        [Pure]
        public static bool Equals(StringRange first, StringRange second, StringComparison comparison = StringComparison.Ordinal)
            => first.length == second.length && 0 == string.Compare(
                   first.document, first.startIndex, second.document,
                   second.startIndex, first.length, comparison
               );

        [Pure]
        public static bool Equals(StringRange first, string second, StringComparison comparison = StringComparison.Ordinal)
        {
            if (second == null) return false;
            return first.length == second.Length && 0 == string.Compare(
                first.document, first.startIndex,
                second, 0,
                first.length, comparison);
        }

        #endregion

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public IEnumerator<char> GetEnumerator()
        {
            for (var i = 0; i < this.length; i++)
            {
                yield return this.document[this.startIndex + i];
            }
        }
    }
}