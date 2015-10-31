using JetBrains.Annotations;

namespace System
{
    public struct NotNull<T> where T : class
    {
        [NotNull]
        public T Value { get; }

        private NotNull([NotNull] T value)
        {
            this.Value = value;
        }

        [NotNull]
        public static implicit operator T(NotNull<T> source)
        {
            return source.Value;
        }

        public static explicit operator NotNull<T>([NotNull] T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return new NotNull<T>(value);
        }
    }
}