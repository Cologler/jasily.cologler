using System;

namespace Jasily
{
    public sealed class Container<T>
    {
        private T value;

        public void RemoveValue()
        {
            this.HasValue = false;
            this.value = default(T);
        }

        public void SetValue(T value)
        {
            this.value = value;
            this.HasValue = true;
        }

        public Container()
        {

        }

        public Container(T value)
        {
            this.SetValue(value);
        }

        public bool HasValue { get; private set; }

        public T Value
        {
            get
            {
                if (this.HasValue) return this.value;
                throw new InvalidOperationException();
            }
            set { this.SetValue(value); }
        }
    }
}