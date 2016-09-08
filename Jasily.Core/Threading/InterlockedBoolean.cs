using JetBrains.Annotations;

namespace System.Threading
{
    public sealed class InterlockedBoolean
    {
        private const int True = 1;
        private const int False = 0;

        private int value;

        public InterlockedBoolean()
        {

        }

        public InterlockedBoolean(bool value)
        {
            this.value = value ? True : False;
        }

        public bool Value => this.value == True;

        public bool SlimRead()
            => this.value == True;

        public bool Read()
            => Volatile.Read(ref this.value) == True;

        public void Write(bool value)
            => Volatile.Write(ref this.value, value ? True : False);

        public bool CompareExchange(bool value, bool compared)
            => Interlocked.CompareExchange(ref this.value, value ? True : False, compared ? True : False) == True;

        public static implicit operator bool([NotNull] InterlockedBoolean value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.Value;
        }
    }
}