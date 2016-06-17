using System.Runtime.InteropServices;

namespace Jasily
{
    public class ConsoleActivator
    {
        private static readonly object SyncRoot = new object();
        private static bool isAlloced;

        [DllImport("Kernel32.dll")]
        private static extern bool AttachConsole(int processId);

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        public static void CreateOrAttachConsole()
        {
            lock (SyncRoot)
            {
                if (isAlloced) return;

                if (AttachConsole(-1)) // attach to parent
                {

                }
                else
                {
                    AllocConsole();
                    isAlloced = true;
                }
            }
        }

        public static void ReleaseConsole()
        {
            if (!isAlloced) return;

            lock (SyncRoot)
            {
                if (isAlloced)
                {
                    FreeConsole();
                }
            }
        }
    }
}