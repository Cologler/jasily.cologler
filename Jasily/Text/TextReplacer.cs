using System;
using System.Collections.Generic;
using System.Text;
using Jasily.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public sealed class TextReplacer : IStringFinderSource
    {
        public static string Replace([NotNull] string text, [NotNull] IReadOnlyDictionary<string, string> replacements,
            StringComparison comparison = StringComparison.Ordinal)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (replacements == null) throw new ArgumentNullException(nameof(replacements));

            if (text.Length == 0 || replacements.Count == 0) return text;

            var comparer = new CachedLastEqualityComparer<string>(JasilyComparer.GetStringComparer(comparison));
            var hashSet = new HashSet<string>(comparer);
            var replacer = new TextReplacer(text, comparison);
            var finders = new List<ReplaceFinder>(replacements.Count);
            foreach (var pair in replacements)
            {
                if (string.IsNullOrEmpty(pair.Key))
                    throw new ArgumentException("replacement source can not be empty", nameof(replacements));
                if (pair.Value == null)
                    throw new ArgumentNullException(nameof(replacements), "replacement dest can not be empty");
                if (!hashSet.Add(pair.Key))
                    throw new ArgumentException($"replacement source [{comparer.LastCompareItem1}] was conflict with [{comparer.LastCompareItem2}]",
                        nameof(replacements));

                finders.Add(new ReplaceFinder(replacer, pair.Key, pair.Value));
            }
            return replacer.Replace(finders);
        }

        private TextReplacer([NotNull] string originText, StringComparison comparison)
        {
            this.OriginalString = originText;
            this.Comparison = comparison;
            this.StartIndex = 0;
        }

        public string OriginalString { get; }

        public StringComparison Comparison { get; }

        public int StartIndex { get; private set; }

        private string Replace(List<ReplaceFinder> finders)
        {
            foreach (var finder in finders)
            {
                finder.MoveNext();
            }
            finders.RemoveAll(z => z.Current < 0);
            if (finders.Count == 0) return this.OriginalString;
            finders.Sort(ReplaceFinder.Comparer);

            var sb = new StringBuilder();
            while (true)
            {
                if (finders.Count == 0) break;
                var first = finders[0];

                // add replacement
                if (first.Current > this.StartIndex) sb.Append(this.OriginalString, this.StartIndex, first.Current - this.StartIndex);
                sb.Append(first.To);
                this.StartIndex = first.Current + first.Value.Length;

                // resort
                foreach (var finder in finders.ToArray())
                {
                    if (finder.Current < this.StartIndex)
                    {
                        finders.Remove(finder);
                        if (finder.MoveNext()) finders.Insert(~finders.BinarySearch(finder, ReplaceFinder.Comparer), finder);
                    }
                }
            }
            return sb.Append(this.OriginalString, this.StartIndex).ToString();
        }

        private class ReplaceFinder : StringFinder
        {
            [NotNull]
            public readonly string To;

            public ReplaceFinder([NotNull] IStringFinderSource source, [NotNull] string value, [NotNull] string to)
                : base(source, value)
            {
                this.To = to;
            }

            private static int Compare(ReplaceFinder x, ReplaceFinder y) => x.Current == y.Current
                ? y.Value.Length.CompareTo(x.Value.Length)
                : x.Current.CompareTo(y.Current);

            public static readonly Comparer<ReplaceFinder> Comparer = Comparer<ReplaceFinder>.Create(Compare);
        }
    }
}