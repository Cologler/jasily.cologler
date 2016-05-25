namespace System
{
    public static class Tuple2
    {
        public static Tuple2<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
            => new Tuple2<T1, T2>(item1, item2);

        public static Tuple2<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
            => new Tuple2<T1, T2, T3>(item1, item2, item3);
    }

    public struct Tuple2<T1, T2>
    {
        public Tuple2(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }
    }

    public struct Tuple2<T1, T2, T3>
    {
        public Tuple2(T1 item1, T2 item2, T3 item3)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }
    }
}