namespace System.EventArgses
{
    public sealed class ChangingEventArgs<T> : EventArgs
    {
        public T Old { get; }

        public T New { get; }

        public ChangingEventArgs(T old, T @new)
        {
            this.Old = old;
            this.New = @new;
        }
    }
}