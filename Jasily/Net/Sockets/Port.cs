using System;

namespace Jasily.Net.Sockets
{
    public struct Port
    {
        public int Value { get; }

        public Port(int port)
        {
            if (IsValueVaild(port))
                this.Value = port;
            else
                throw new ArgumentException(nameof(port));
        }

        public bool IsInitialized => this.Value != 0;

        public static implicit operator Port(string port) => new Port(int.Parse(port));

        public static implicit operator Port(int port) => new Port(port);

        public override string ToString() => this.Value.ToString();

        public static bool IsValueVaild(int port) => port > 0 && port < 65536;
    }
}