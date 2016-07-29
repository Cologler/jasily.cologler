namespace System
{
    public class ArgumentWhiteSpaceException : ArgumentException
    {
        public ArgumentWhiteSpaceException()
        {
        }

        public ArgumentWhiteSpaceException(string message)
            : base(message)
        {
        }

        public ArgumentWhiteSpaceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ArgumentWhiteSpaceException(string message, string paramName)
            : base(message, paramName)
        {
        }

        public ArgumentWhiteSpaceException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException)
        {
        }
    }
}