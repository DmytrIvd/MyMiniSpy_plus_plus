using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniSpy__
{

    //Unsafe code

    //delegetes
    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    public delegate IntPtr LowLevelMouseProc(int nCode, UIntPtr wParam, IntPtr lParam);
    public delegate int EnumWindowsProc(IntPtr hwnd, int lParam);
    //All functions
    public static class Win32Functions
    {
        #region Functions
        #region GetWindowInfo
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("User32.Dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void GetClassName(int hWnd, StringBuilder s, int nMaxCount);
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int32 SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int32 SendMessage(int hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern long GetWindowRect(int hWnd, ref Rectangle lpRect);
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        #endregion
        #region GetAllWindows
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        [DllImport("user32.dll")]
        public static extern int EnumWindows(EnumWindowsProc x, int y);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumChildWindows(IntPtr window, EnumWindowsProc callback, int lParam);
        #endregion
        #region HookMouse
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(HookType idHook, LowLevelMouseProc lpfn, IntPtr hMod, int dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int UnhookWindowsHookEx(IntPtr hHook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr _, int nCode, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point point);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto,SetLastError =true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion
        #endregion


        #region Constraints
        public const int WM_GETTEXT = 0x000D;
        public const int WM_GETTEXTLENGTH = 0x000E;
        #endregion
    }

    //P/Invoked structures
    public enum GWL
    {
        GWL_WNDPROC = (-4),
        GWL_HINSTANCE = (-6),
        GWL_HWNDPARENT = (-8),
        GWL_STYLE = (-16),
        GWL_EXSTYLE = (-20),
        GWL_USERDATA = (-21),
        GWL_ID = (-12)
    }
    public enum HookType
    {
        LowLevelKeyboard = 13,
        LowLevelMouse = 14
    }
    #region MouseHelpers
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEHOOKSTRUCT
    {
        public Point pt;
        public IntPtr hwnd;
        public uint wHitTestCode;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseHookStructEx
    {
        public MOUSEHOOKSTRUCT mouseHookStruct;
        public int MouseData;
    }
    public enum MouseMessage
    {
        MouseMove = 0x200,
        LButtonDown = 0x201,
        LButtonUp = 0x202,
        RButtonDown = 0x204,
        RButtonUp = 0x205,
        MouseWheel = 0x20a,
        MouseHWheel = 0x20e
    }
    #endregion
}
