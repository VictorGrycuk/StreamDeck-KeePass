using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace streamdeck_keepass.Services
{
    public static class ProcessHelper
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static string GetActiveProcessFileName()
        {
            var hwnd = GetForegroundWindow();
            GetWindowThreadProcessId(hwnd, out uint pid);
            var p = Process.GetProcessById((int)pid);
            
            return Path.GetFileName(p.MainModule.FileName);
        }
    }
}
