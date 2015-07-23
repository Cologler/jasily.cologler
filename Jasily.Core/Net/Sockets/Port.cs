namespace System.Net.Sockets
{
    public struct Port
    {
        public int Value { get; }

        public Port(int port)
        {
            if (IsValueVaild(port))
                this.Value = port;
            else
                throw new ArgumentException("port");
        }

        public static implicit operator Port(string port)
        {
            return new Port(int.Parse(port));
        }

        public static implicit operator Port(int port)
        {
            return new Port(port);
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public static bool IsValueVaild(int port)
        {
            return port > 0 && port < 65536;
        }
    }
}