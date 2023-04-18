using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyMouseAndKeyboard
{
    public enum MouseOperate
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        Wheel = 0x0800,
        Absolute = 0x8000
    }

    public class MouseAndKeyboard
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point point);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern void GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWnd, IntPtr hParent);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);


        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public static extern bool SetCursorPos(int dx, int dy);

        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        public static extern void mouse_event(int deFlags, int dx, int dy, int dwData, ulong dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetForeggroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, ulong dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        public static extern int PostMessage(IntPtr hWnd, uint msg, Keys wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage", SetLastError = true)]
        public static extern int PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
    }
}
