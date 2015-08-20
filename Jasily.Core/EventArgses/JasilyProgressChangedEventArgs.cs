namespace System.EventArgses
{
    public class JasilyProgressChangedEventArgs : EventArgs
    {
        public long Current { get; }

        public long Total { get; }

        public bool IsCompleted { get; }

        public JasilyProgressChangedEventArgs(long current, long total)
        {
            this.Current = current;
            this.Total = total;
            this.IsCompleted = current == total;
        }

        public double GetPercentage()
        {
            return Convert.ToDouble(this.Current) / Convert.ToDouble(this.Total);
        }
    }
}