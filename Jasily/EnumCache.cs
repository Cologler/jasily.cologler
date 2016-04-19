using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily
{
    public class EnumCache<T>
        where T : struct, IComparable, IFormattable
    {
        private readonly bool isFlags;
        private readonly EnumItem[] items;

        private class EnumItem
        {
            internal readonly ulong Value;

            internal readonly T Item;

            internal readonly string Name;

            public EnumItem(ulong value, T item, string name)
            {
                this.Value = value;
                this.Item = item;
                this.Name = name;
            }
        }

        public static EnumCache<T> Default { get; } = new EnumCache<T>();

        private EnumCache()
        {
            var ti = typeof(T).GetTypeInfo();
            if (!ti.IsEnum) throw new InvalidOperationException();
            this.isFlags = ti.HasCustomAttribute<FlagsAttribute>();
            this.items = JasilyEnum.GetValues<T>()
                .Select(z => new EnumItem(z.Casting<T>().UncheckedTo<ulong>(), z, z.ToString()))
                .OrderBy(z => z.Value)
                .ToArray();
        }

        private EnumItem TryGetEnumItem(T e)
        {
            var val = e.Casting().UncheckedTo<ulong>();
            return this.items.FirstOrDefault(z => z.Value == val);
        }

        public bool IsDefined(T e) => this.TryGetEnumItem(e) != null;

        public string ToString(T e)
        {
            if (!this.isFlags)
            {
                return this.TryGetEnumItem(e)?.Name ?? e.ToString();
            }
            else // for flag
            {
                var val = e.Casting().UncheckedTo<ulong>();

                if (val == 0)
                {
                    return this.items.Length > 0 && this.items[0].Value == 0 ? this.items[0].Name : "0";
                }

                var matchs = new List<EnumItem>();
                foreach (var item in this.items.Reverse())
                {
                    if (item.Value == 0) break;

                    if ((item.Value & val) == item.Value)
                    {
                        val -= item.Value;
                        matchs.Insert(0, item);
                    }
                }
                return val != 0 ? e.ToString() : matchs.Select(z => z.Name).JoinWith(", ");
            }
        }

        public IEnumerable<T> All() => this.items.Select(z => z.Item);

        public bool TryParse([NotNull] string value, StringComparison comparison, ref T result)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var matchs = this.items.Where(z => string.Compare(z.Name, value, comparison) == 0).ToArray();
            if (matchs.Length == 0)
            {
                result = default(T);
                return false;
            }
            else
            {
                result = matchs[0].Item;
                return true;
            }
        }
    }
}