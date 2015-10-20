
namespace System.Collections.Generic
{
    public struct NameValuePair<TName, TValue> : IEquatable<NameValuePair<TName, TValue>>
    {
        public NameValuePair(TName name, TValue value)
        {
            this.Name = name;
            this.Value = value;
        }

        public TName Name { get; }

        public TValue Value { get; }

        /// <summary>
        /// 指示当前对象是否等于同一类型的另一个对象。
        /// </summary>
        /// <returns>
        /// 如果当前对象等于 <paramref name="other"/> 参数，则为 true；否则为 false。
        /// </returns>
        /// <param name="other">与此对象进行比较的对象。</param>
        public bool Equals(NameValuePair<TName, TValue> other)
        {
            if (ReferenceEquals(this.Name, other.Name) && ReferenceEquals(this.Value, other.Value))
                return true;

            return Equals(this.Name, other.Name) && Equals(this.Value, other.Value);
        }

        public override string ToString() => this.Name?.ToString() ?? this.ToString();
    }
}
