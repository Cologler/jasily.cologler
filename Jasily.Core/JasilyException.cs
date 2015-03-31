
namespace System
{
    public class JasilyException : Exception
    {
        public JasilyException(Guid id)
            : base()
        {
            this.Id = id;
        }

        public JasilyException(Guid id, string message)
            : base(message)
        {
            this.Id = id;
        }

        public JasilyException(Guid id, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Id = id;
        }

        public Guid Id { get; private set; }
    }
}
