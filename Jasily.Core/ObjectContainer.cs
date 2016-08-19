namespace System
{
    public sealed class ObjectContainer<T>
    {
        private T value;

        public void UnSet()
        {
            this.IsSet = false;
            this.value = default(T);
        }

        public void Set(T value)
        {
            this.value = value;
            this.IsSet = true;
        }

        public ObjectContainer()
        {

        }

        public ObjectContainer(T value)
        {
            this.Set(value);
        }

        public bool IsSet { get; private set; }

        public T Value
        {
            get
            {
                if (this.IsSet) return this.value;
                throw new InvalidOperationException();
            }
            set { this.Set(value); }
        }
    }
}