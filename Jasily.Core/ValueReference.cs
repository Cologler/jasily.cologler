using JetBrains.Annotations;

namespace System
{
    public sealed class ValueReference<T>
        where T : struct
    {
        public T Value { get; }

        public ValueReference(T value)
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

        public static implicit operator ValueReference<T>(T value) => new ValueReference<T>(value);

        public static explicit operator T([NotNull] ValueReference<T> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.Value;
        }
    }
}