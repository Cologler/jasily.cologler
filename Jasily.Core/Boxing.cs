using JetBrains.Annotations;

namespace System
{
    public sealed class Boxing<T>
        where T : struct
    {
        public T Value { get; set; }

        public Boxing(T value = default(T))
        {
            this.Value = value;
        }

        #region Overrides of Object

        /// <summary>
        /// 返回表示当前对象的字符串。
        /// </summary>
        /// <returns>
        /// 表示当前对象的字符串。
        /// </returns>
        public override string ToString() => this.Value.ToString();

        #endregion

        public static implicit operator Boxing<T>(T value) => new Boxing<T>(value);

        public static implicit operator T([NotNull] Boxing<T> box)
        {
            if (box == null) throw new ArgumentNullException(nameof(box));
            return box.Value;
        }
    }
}