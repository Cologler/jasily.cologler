using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public class StringFinder : IEnumerator<int>
    {
        [NotNull]
        private readonly IStringFinderSource source;

        [NotNull]
        public string Value { get; }

        public StringFinder([NotNull] IStringFinderSource source, [NotNull] string value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(value)) throw new ArgumentEmptyException(nameof(value));
            if (source.OriginalString == null)
                throw new ArgumentException("original string of source cannot be null.", nameof(source.OriginalString));

            this.source = source;
            this.Value = value;
            this.Reset();
        }

        public bool MoveNext()
        {
            var text = this.source.OriginalString;
            var startIndex = this.source.StartIndex;
            if (text.Length - startIndex < this.Value.Length) return false; // length not enough.
            this.Current = text.IndexOf(this.Value, startIndex, this.source.Comparison);
            return this.Current >= 0;
        }

        public void Reset() => this.Current = -1;

        public int Current { get; private set; }

        object IEnumerator.Current => this.Current;

        public void Dispose() => this.Reset();
    }
}