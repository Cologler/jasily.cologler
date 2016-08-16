namespace System
{
    /// <summary>
    /// current path should never reachable
    /// </summary>
    public sealed class ReachableException : Exception
    {
        public ReachableException()
        {
        }

        public ReachableException(string message)
            : base(message)
        {
        }
    }
}