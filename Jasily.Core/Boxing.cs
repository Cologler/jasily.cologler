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
        /// ���ر�ʾ��ǰ������ַ�����
        /// </summary>
        /// <returns>
        /// ��ʾ��ǰ������ַ�����
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