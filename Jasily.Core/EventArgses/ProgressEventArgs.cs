namespace System.EventArgses
{
    public class ProgressEventArgs : EventArgs
    {
        public long Current { get; }

        public long Total { get; }

        public ProgressEventArgs(long current, long total)
        {
            this.Current = current;
            this.Total = total;
        }

        public double GetPercent()
        {
            return Convert.ToDouble(this.Current) / Convert.ToDouble(this.Total);
        }
    }
}