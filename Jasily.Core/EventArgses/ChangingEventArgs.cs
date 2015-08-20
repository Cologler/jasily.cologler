namespace System.EventArgses
{
    public sealed class ChangingEventArgs<T> : EventArgs
    {
        public T Old { get; private set; }

        public T New { get; private set; }

        public ChangingEventArgs(T old, T @new)
        {
            this.Old = old;
            this.New = @new;
        }
    }
}