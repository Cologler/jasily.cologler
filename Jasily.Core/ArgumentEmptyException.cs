namespace System
{
    public class ArgumentEmptyException : ArgumentException
    {
        public ArgumentEmptyException()
        {
        }

        public ArgumentEmptyException(string message)
            : base(message)
        {
        }

        public ArgumentEmptyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ArgumentEmptyException(string message, string paramName)
            : base(message, paramName)
        {
        }

        public ArgumentEmptyException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException)
        {
        }
    }
}