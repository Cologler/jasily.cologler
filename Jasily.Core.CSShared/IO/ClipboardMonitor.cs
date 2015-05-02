using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
            HandleSource = window;
        }

        public void Begin()
        {
            WindowHelper = new WindowInteropHelper(HandleSource);
            AddClipboardFormatListener(this.WindowHelper.Handle);

            HwndSource = HwndSource.FromHwnd(WindowHelper.Handle);
            HwndSource.AddHook(WndProc);
        }

        public void Stop()
        {
            HwndSource.RemoveHook(WndProc);

            RemoveClipboardFormatListener(this.WindowHelper.Handle);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_CLIPBOARDUPDATE)
            {
                ClipboardUpdated.Fire(this);
            }

            return IntPtr.Zero;
        }

        public event EventHandler ClipboardUpdated;
    }
}
