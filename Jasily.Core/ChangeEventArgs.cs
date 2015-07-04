namespace System
{
    public sealed class ChangeEventArgs<T> : EventArgs
    {
        public T Old { get; private set; }

        public T New { get; private set; }

        public ChangeEventArgs(T old, T @new)
        {
            this.Old = old;
            this.New = @new;
        }
    }
}