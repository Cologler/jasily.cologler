using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace System.IO
{
    public sealed class ClipboardMonitor
    {
        private const int WM_CLIPBOARDUPDATE = 0x031D;

        [DllImport("user32.dll")]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        WindowInteropHelper WindowHelper;
        HwndSource HwndSource;
        Window HandleSource;

        public ClipboardMonitor(Window window)
        {
            this.HandleSource = window;
        }

        public void Begin()
        {
            this.WindowHelper = new WindowInteropHelper(this.HandleSource);
            AddClipboardFormatListener(this.WindowHelper.Handle);

            this.HwndSource = HwndSource.FromHwnd(this.WindowHelper.Handle);
            this.HwndSource.AddHook(this.WndProc);
        }

        public void Stop()
        {
            this.HwndSource.RemoveHook(this.WndProc);

            RemoveClipboardFormatListener(this.WindowHelper.Handle);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_CLIPBOARDUPDATE)
            {
                this.ClipboardUpdated.Fire(this);
            }

            return IntPtr.Zero;
        }

        public event EventHandler ClipboardUpdated;
    }
}
