using System;

namespace Jasily.Desktop
{
    public class ConsoleTempEnvironment : IDisposable
    {
        private readonly ConsoleColor foregroundColor;
        private readonly ConsoleColor backgroundColor;

        public ConsoleTempEnvironment()
        {
            this.foregroundColor = Console.ForegroundColor;
            this.backgroundColor = Console.BackgroundColor;
        }

        public void Dispose()
        {
            Console.ForegroundColor = this.foregroundColor;
            Console.BackgroundColor = this.backgroundColor;
        }
    }
}